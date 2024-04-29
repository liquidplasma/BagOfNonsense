using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Ammo;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BagOfNonsense.Projectiles
{
    public class AR2FireNoiseSelection : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("AR2FireSoundType")]
        [DefaultValue(true)]
        public bool AR2AlternateFireSFXType { get; set; }
    }

    public class AR2Held : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private Item CombineBall { get; set; }
        public bool AllowedToFire => Player.channel && !ChargingAltFire && Player.HasAmmo(Player.HeldItem);
        private bool ChargingAltFire => AltFireDelay >= 60;
        private static Texture2D Glow => ModContent.Request<Texture2D>("BagOfNonsense/Projectiles/AR2Held_Glow").Value;
        private Item Ammo => ContentSamples.ItemsByType[ModContent.ItemType<PulseAmmo>()];
        private bool HasAltFireAmmo => Player.HasItem(ModContent.ItemType<CombineBalls>());
        private int ShotDamage => (int)((Projectile.originalDamage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Ammo.damage)) * Player.GetStealth());

        private int ShotDelay
        {
            get
            {
                return (int)Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }

        private bool RightMousePressed;

        private Vector2
            MouseAim,
            drawPos;

        private int
            AR2Spread,
            AltFireDelay,
            ChargingLightControl;

        private SoundStyle FireVanilla => new("BagOfNonsense/Sounds/Weapons/AR2/fire")
        {
            Volume = 0.25f,
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
        };

        private SoundStyle FireSMODElite => new("BagOfNonsense/Sounds/Weapons/AR2/fire", 4)
        {
            Volume = 0.25f,
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
        };

        private SoundStyle BallLaunch => new("BagOfNonsense/Sounds/Weapons/AR2/ar2_altfire")
        {
            Volume = 0.25f,
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
        };

        private SoundStyle Charging => new("BagOfNonsense/Sounds/Weapons/AR2/charging")
        {
            Volume = 0.25f,
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 68;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            ShotDelay = Player.HeldItem.useTime;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (AllowedToFire)
            {
                Vector2 muzzleDrawPos = Player.MountedCenter;
                Texture2D muzzleFlash = ModContent.Request<Texture2D>("BagOfNonsense/Assets/AR2MuzzleFlashFixed").Value;
                Rectangle rect = muzzleFlash.Frame(verticalFrames: 6, frameY: Projectile.frame - 1);
                Main.EntitySpriteDraw(muzzleFlash, muzzleDrawPos + muzzleDrawPos.DirectionTo(MouseAim) * 60f - Main.screenPosition, rect, Color.Cyan, Projectile.rotation + (MathHelper.PiOver2 * -Player.direction), rect.Size() / 2, 0.8f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            if (AllowedToFire)
            {
                Rectangle glowRect = Glow.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
                Main.EntitySpriteDraw(Glow, drawPos - Main.screenPosition, glowRect, Color.White, Projectile.rotation, glowRect.Size() / 2, Projectile.scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
        }

        public override bool PreAI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            if (AllowedToFire)
                Projectile.frame = Math.Clamp(ShotDelay, 0, Main.projFrames[Type]);
            else
                Projectile.frame = 0;
            return base.PreAI();
        }

        public override void AI()
        {
            if (AltFireDelay > 0)
                AltFireDelay--;

            if (Player.whoAmI == Main.myPlayer)
            {
                MouseAim = Main.MouseWorld;
                if (!Main.mapFullscreen)
                    RightMousePressed = Main.mouseRight;
                Projectile.netUpdate = true;
            }

            //shooting
            #region
            if (AllowedToFire)
            {
                ShotDelay++;
                if (ShotDelay >= Player.HeldItem.useTime)
                {
                    ShotDelay = 0;
                    if (Main.rand.NextBool(10) && AR2Spread <= 3)
                        AR2Spread++;

                    if (ModContent.GetInstance<AR2FireNoiseSelection>().AR2AlternateFireSFXType)
                        SoundEngine.PlaySound(FireVanilla, Player.Center);
                    else
                        SoundEngine.PlaySound(FireSMODElite, Player.Center);

                    Vector2 aim = (Projectile.Center.DirectionTo(MouseAim) * 16f).RotatedByRandom(MathHelper.ToRadians(AR2Spread) * Main.rand.NextFloat(0.95f, 1.05f));

                    if (Player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), Projectile.Center, aim * Main.rand.NextFloat(0.9f, 1.1f), ModContent.ProjectileType<PulseBullet>(), ShotDamage, 3f, Player.whoAmI);
                }
            }
            else
            {
                ShotDelay = 6;
                AR2Spread = 0;
            }
            #endregion

            //alternate fire
            #region
            if (HasAltFireAmmo && RightMousePressed && AltFireDelay == 0 && !Player.mouseInterface)
            {
                AltFireDelay = 120;
                SoundEngine.PlaySound(Charging, Projectile.Center);
            }
            if (AltFireDelay == 60)
            {
                SoundEngine.PlaySound(BallLaunch, Projectile.Center);
                Vector2 aim = Projectile.Center.DirectionTo(MouseAim) * 12f;
                if (HasAltFireAmmo)
                {
                    Player.ConsumeItem(ModContent.ItemType<CombineBalls>());
                    ExtensionMethods.BetterNewProjectile(Player, Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), Projectile.Center, aim * Main.rand.NextFloat(0.9f, 1.1f), ModContent.ProjectileType<PulseBall>(), 1000, 10f, Player.whoAmI);
                }
            }
            if (AltFireDelay >= 60)
            {
                ChargingLightControl++;
                Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3() * (ChargingLightControl / 30f));
            }
            else
                ChargingLightControl = 0;
            #endregion

            // velocity / position
            #region
            Vector2 recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 2f);
            Vector2 distance = Player.Center.DirectionTo(MouseAim) * 16f - recoil;
            drawPos = Projectile.Center = Player.MountedCenter + distance;
            Projectile.velocity = Vector2.Zero;
            int mouseDirection = (Player.DirectionTo(MouseAim).X > 0f) ? 1 : -1;            
            Projectile.spriteDirection = Player.direction;
            Player.heldProj = Projectile.whoAmI;
            Projectile.rotation = Player.AngleTo(MouseAim) + MathHelper.PiOver2;
            if (Player.channel || AltFireDelay >= 60)
            {
                Player.HoldOutArm(Projectile, MouseAim);
                Player.SetDummyItemTime(2);
                Player.ChangeDir(mouseDirection);
                Projectile.alpha = 0;
            }
            else
                Projectile.alpha = 255;
            #endregion

            if (Player.HeldItem.type != ModContent.ItemType<AR2Item>())
                Projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(MouseAim);
            writer.Write(RightMousePressed);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseAim = reader.ReadVector2();
            RightMousePressed = reader.ReadBoolean();
        }
    }
}