using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class M1GarandHeld : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private Item HeldItem => Player.HeldItem;

        private static SoundStyle Fire => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.2f, 0.2f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private static SoundStyle GarandPing => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/ping", 5)
        {
            MaxInstances = 0,
            Volume = 0.25f
        };

        private static SoundStyle Empty => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/empty");
        private static SoundStyle MagIn => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/magin");
        private static SoundStyle BoltLock => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/bltrel");

        private int CurrentAmmo
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        private int ShotDelay
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        private int ReloadTimer
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        
        private int ShouldHide
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        private Vector2 MouseAim, idlePos;
        private bool CanFire => ShotDelay >= HeldItem.useAnimation;
        private bool EnblocIn = true;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 70;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ShotDelay <= 6)
            {
                Vector2 muzzleDrawPos = Player.MountedCenter;
                muzzleDrawPos += muzzleDrawPos.DirectionTo(MouseAim) * 52f;
                Texture2D muzzleFlash = HelperStats.MuzzleFlash;
                Rectangle rect = muzzleFlash.Frame(verticalFrames: 6, frameY: ShotDelay);
                Main.EntitySpriteDraw(muzzleFlash, muzzleDrawPos - Main.screenPosition, rect, Color.Yellow, Projectile.rotation + (MathHelper.PiOver2 * -Player.direction), rect.Size() / 2, 0.66f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
            return true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = 8;
            ShotDelay = HeldItem.useAnimation;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreAI()
        {
            Projectile.CheckPlayerActiveAndNotDead(Player);
            return base.PreAI();
        }

        public override void AI()
        {
            if (ShotDelay <= HeldItem.useAnimation)
                ShotDelay++;

            if (Player.whoAmI == Main.myPlayer)
            {
                MouseAim = Main.MouseWorld;
                if (!Player.mouseInterface)                
                    Main.instance.MouseText(CurrentAmmo + " / 8");                
                Projectile.netUpdate = true;
            }

            //Shooting logic
            #region
            if (Player.channel && Player.HasAmmo(HeldItem) && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Vector2 aim = Projectile.Center.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(2))) * HeldItem.shootSpeed;
                Item ammo = Player.ChooseAmmo(HeldItem);
                int damage = (int)((Projectile.damage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(ammo.damage)) * Player.GetPlayerStealth());
                int type = ammo.shoot;
                if (type == ProjectileID.Bullet) type = ModContent.ProjectileType<NormalBullet>();
                if (Player.whoAmI == Main.myPlayer)
                    Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo), Projectile.Center, aim, type, damage, HeldItem.knockBack, Player.whoAmI);
            }
            if (CurrentAmmo == 0 && EnblocIn)
            {
                SoundEngine.PlaySound(GarandPing, Projectile.Center);
                if (Player.whoAmI == Main.myPlayer)
                    Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo), Projectile.Center, new Vector2(0, -Main.rand.NextFloat(4f, 6f)).RotatedByRandom(MathHelper.PiOver4), ModContent.ProjectileType<M1GarandEnbloc>(), 0, 0f, Player.whoAmI);
                ReloadTimer = HeldItem.useTime * 5;
                EnblocIn = false;
            }
            if (ReloadTimer > 0 && Player.channel && CanFire && ReloadTimer % 40 == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
            }
            switch (ReloadTimer)
            {
                case 10:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    CurrentAmmo = 8;
                    EnblocIn = true;
                    break;

                case 20:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    break;
            }
            if (ReloadTimer > 0) ReloadTimer--;
            #endregion

            //positioning / velocity
            #region
            if (Player.direction == -1)
                idlePos = new Vector2(8, -32);
            else
                idlePos = new Vector2(-24, -32);
            if (Player.channel || ReloadTimer > 0)
            {
                Vector2 recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 3f);                
                Vector2 distance = Player.MountedCenter.DirectionTo(MouseAim) * 16f - recoil;
                Projectile.Center = Player.MountedCenter + distance;
                Projectile.velocity = Vector2.Zero;
                int mouseDirection = (Player.DirectionTo(MouseAim).X > 0f) ? 1 : -1;
                Projectile.rotation = Player.AngleTo(MouseAim) + MathHelper.PiOver2;
                Player.ChangeDir(mouseDirection);
                Projectile.spriteDirection = Player.direction;
                Player.heldProj = Projectile.whoAmI;
                Player.HoldOutArm(Projectile);
            }
            else
            {
                Projectile.position = Player.Center + idlePos;
                Projectile.spriteDirection = Player.direction;
                Projectile.rotation = MathHelper.Pi;
            }     
            #endregion

            if (HeldItem.type != ModContent.ItemType<M1Garand>()) Projectile.Kill();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(MouseAim);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseAim = reader.ReadVector2();
        }
    }

    public class M1GarandEnbloc : ModProjectile
    {
        private static SoundStyle Tink => new("BagOfNonsense/Sounds/Weapons/Ins2/garand/gclip", 3)
        {
            Volume = 0.25f,
        };

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 3600;
            Projectile.light = 0;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            if (Projectile.penetrate > 1)
            {
                Projectile.alpha -= 16;
                Projectile.velocity.Y += 0.1f;
                Projectile.RotateBasedOnVelocity();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center, oldVelocity, Projectile.width, Projectile.height);
            Projectile.penetrate--;
            if (Projectile.penetrate == 1)
            {
                Projectile.tileCollide = false;
                Projectile.velocity *= 0;
            }
            else
            {
                Projectile.Bounce(60);
                Projectile.velocity *= 0.5f;
                SoundEngine.PlaySound(Tink, Projectile.Center);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
        }
    }
    public class NormalBullet : ModProjectile
    {
        VertexStrip _vertexStrip = new();
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Bullet;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 500;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            Projectile.alpha = 255;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {            
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.WhiteTrail, ShaderStuff.NormalBulletStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            Projectile.FaceForward();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            return base.OnTileCollide(oldVelocity);
        }
    }
}