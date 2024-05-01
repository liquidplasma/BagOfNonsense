using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class SashaCritTime : ModPlayer
    {
        public int canConsumeAmmo;

        public int damageTracker;

        public bool allowed;

        public int critTimer;

        public override void ResetEffects()
        {
            if (allowed && critTimer > 0 && Player.channel && canConsumeAmmo == 1)
            {
                critTimer--;
            }
            allowed = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (allowed && proj.owner == Player.whoAmI && proj.GetGlobalProjectile<BagOfNonsenseGlobalProjectile>().SashaProjBool)
            {
                if (critTimer == 0)
                    damageTracker += damageDone;
                if (allowed && Main.rand.NextBool(200) && damageTracker >= 10000)
                {
                    damageTracker = 0;
                    critTimer += Main.rand.Next(600, 900);
                }
                if (critTimer > 0)
                {
                    hit.HideCombatText = true;
                    TF2Crit.CritSFXandText(target, damageDone);
                }
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.channel && Player.HeldItem.type == ModContent.ItemType<Sasha>())
                Player.noKnockback = true;
        }
    }

    public class SashaProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private SashaCritTime ControlCritTime => Player.GetModPlayer<SashaCritTime>();
        private bool Critting => ControlCritTime.critTimer > 0;
        private bool NotCritting => ControlCritTime.critTimer <= 0;

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

        private Vector2
            drawPos,
            mousePosSasha;

        private int
            shouldRotate,
            animationSpunUp,
            animationControl;

        private ActiveSound
            aShootLoop,
            aShootLoopCrit;

        private static SoundStyle ShootLoop => new("BagOfNonsense/Sounds/Weapons/sasha_shoot")
        {
            Volume = 0.25f,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        private static SoundStyle ShootLoopCrit => new("BagOfNonsense/Sounds/Weapons/sasha_shoot_crit")
        {
            Volume = 0.25f,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sasha");
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 52;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
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
            if (Critting && shouldRotate != 0)
            {
                int shaderID = ContentSamples.ItemsByType[ItemID.RedDye].dye;
                Main.instance.PrepareDrawnEntityDrawing(Projectile, shaderID, null);
                Texture2D texture = Projectile.MyTexture();
                Rectangle rect = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
                Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, rect, Color.White, Projectile.rotation, rect.Size() / 2, Projectile.scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
                return false;
            }
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            ControlCritTime.allowed = true;
            ControlCritTime.canConsumeAmmo = animationControl;

            aShootLoop = SoundEngine.FindActiveSound(ShootLoop);
            aShootLoopCrit = SoundEngine.FindActiveSound(ShootLoopCrit);

            if (animationControl == 1 && Player.channel) // shooting noises
            {
                if (Critting && aShootLoopCrit == null)
                    SoundEngine.PlaySound(ShootLoopCrit, Player.Center);
                else if (aShootLoop == null)
                    SoundEngine.PlaySound(ShootLoop, Player.Center);

                if (NotCritting) aShootLoopCrit?.Stop();
                if (Critting) aShootLoop?.Stop();
                if (aShootLoop != null) aShootLoop.Position = Player.Center;
                if (aShootLoopCrit != null) aShootLoopCrit.Position = Player.Center;
            }
            else
            {
                aShootLoop?.Stop();
                aShootLoopCrit?.Stop();
            }

            if (shouldRotate > 0) // dust effects
                shouldRotate--;

            if (Player.channel) // rotating barrels
            {
                animationControl = (int)(12 - animationSpunUp / 40f * 6.5f);
                if (animationControl <= 1)
                    animationControl = 1;

                if (++Projectile.frameCounter >= animationControl)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                }
            }
            else animationControl = 0;

            if (Main.myPlayer == Player.whoAmI)
            {
                mousePosSasha = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            if (Player.channel)
                animationSpunUp++;
            else
                animationSpunUp = 0;

            if (Player.channel)
            {
                ShotDelay++;
                if (ShotDelay >= 6 && Player.HasAmmo(Player.HeldItem) && animationControl == 1 && animationSpunUp >= 10)
                {
                    shouldRotate = 6;
                    ShotDelay = 0;
                    Item ammo = Player.ChooseAmmo(Player.HeldItem);
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 aim = (Player.Center.DirectionTo(mousePosSasha) * 16f).RotatedByRandom(MathHelper.ToRadians(8)) * Main.rand.NextFloat(0.95f, 1.05f);
                        int type = ammo.shoot;
                        if (type == ProjectileID.Bullet)
                            type = ProjectileID.GoldenBullet;

                        var shot = ExtensionMethods.BetterNewProjectile(Player, Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), Projectile.Center, aim, type, 9 * (Player.HasSashaUpgrade() ? 2 : 1), Player.HeldItem.knockBack, Player.whoAmI);
                        shot.GetGlobalProjectile<BagOfNonsenseGlobalProjectile>().SashaProjBool = true;
                        shot.ArmorPenetration = 50;
                        if (type == ProjectileID.GoldenBullet)
                            shot.extraUpdates += 1;
                    }
                }
            }
            Vector2 distance = Player.Center.DirectionTo(mousePosSasha) * 32f;
            Projectile.spriteDirection = Player.direction;
            if (Player.direction == 1)
                drawPos = Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + distance;
            else
                drawPos = Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + distance;

            Projectile.velocity = Vector2.Zero;
            if (Player.channel)
            {
                Projectile.rotation = Player.AngleTo(mousePosSasha) + MathHelper.PiOver2;
                Projectile.alpha = 0;
            }
            else
            {
                Projectile.alpha = 255;
            }

            if (Critting && shouldRotate != 0)
            {
                Vector2 randomVel = Utils.NextVector2Circular(Main.rand, -2, 2) * Main.rand.NextFloat();
                int type = Utils.SelectRandom(Main.rand, DustID.RedsWingsRun, DustID.FireworkFountain_Red, DustID.RedTorch);
                Dust critDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type, randomVel.X, randomVel.Y, 0, Player.TeamColor(), 0.4f); ;
                critDust.noLight = true;
                if (Main.rand.NextBool(4))
                    critDust.noGravity = true;
            }

            if (Player.HeldItem.type != ModContent.ItemType<Sasha>())
                Projectile.Kill();

            Player.heldProj = Projectile.whoAmI;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(mousePosSasha);
            writer.Write7BitEncodedInt(shouldRotate);
            writer.Write7BitEncodedInt(animationSpunUp);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mousePosSasha = reader.ReadVector2();
            shouldRotate = reader.Read7BitEncodedInt();
            animationSpunUp = reader.Read7BitEncodedInt();
        }
    }
}