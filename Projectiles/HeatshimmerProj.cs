using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class HeatshimmerProj : ModProjectile
    {
        public int awokenHP;

        public bool awokenToggle;

        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heatshimmer Spear");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MonkStaffT3);
            Projectile.width = 360;
            Projectile.height = 360;
            Projectile.aiStyle = 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 25;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
            }
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3());
            float num = 50f;
            float num2 = 2f;
            float num3 = 20f;
            float num4 = -(float)Math.PI / 4f;
            int num11 = Math.Sign(Projectile.velocity.X);
            Vector2 vector = Player.RotatedRelativePoint(Player.MountedCenter);
            Vector2 vector2;
            Projectile.velocity = new Vector2(num11, 0f);
            if (Player.dead)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.rotation = new Vector2(num11, 0f - Player.gravDir).ToRotation() + num4 + (float)Math.PI;
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.rotation -= (float)Math.PI / 2f;
                }
            }
            Projectile.alpha -= 128;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            _ = Projectile.ai[0] / num;
            float num12 = 1f;
            Projectile.ai[0] += num12;
            Projectile.rotation += (float)Math.PI * 3f * num2 / num * num11;
            bool flag2 = Projectile.ai[0] == (int)(num / 2f);
            if (Projectile.ai[0] >= num || (flag2 && !Player.controlUseItem))
            {
                Projectile.Kill();
                Player.reuseDelay = 2;
            }
            else if (flag2)
            {
                Vector2 mouseWorld2 = Main.MouseWorld;
                int num13 = (Player.DirectionTo(mouseWorld2).X > 0f) ? 1 : (-1);
                if (num13 != Projectile.velocity.X)
                {
                    Player.ChangeDir(num13);
                    Projectile.velocity = new Vector2(num13, 0f);
                    Projectile.netUpdate = true;
                    Projectile.rotation -= (float)Math.PI;
                }
            }
            if ((Projectile.ai[0] == num12 || (Projectile.ai[0] == (int)(num / 2f) && Projectile.active)) && Projectile.owner == Main.myPlayer)
            {
                Vector2 mouseWorld3 = Main.MouseWorld;
                _ = Player.DirectionTo(mouseWorld3) * 0f;
            }
            float num14 = Projectile.rotation - (float)Math.PI / 4f * num11;
            vector2 = (num14 + ((num11 == -1) ? ((float)Math.PI) : 0f)).ToRotationVector2() * (Projectile.ai[0] / num) * num3;
            Vector2 vector4 = Projectile.Center + (num14 + ((num11 == -1) ? ((float)Math.PI) : 0f)).ToRotationVector2() * 30f;
            Vector2 vector5 = num14.ToRotationVector2();
            Vector2 vector6 = vector5.RotatedBy((float)Math.PI / 2f * Projectile.spriteDirection);
            for (int j = 0; j < 4; j++)
            {
                float num15 = 1f;
                float num16 = 1f;
                switch (j)
                {
                    case 1:
                        num16 = -1f;
                        break;

                    case 2:
                        num16 = 1.25f;
                        num15 = 0.5f;
                        break;

                    case 3:
                        num16 = -1.25f;
                        num15 = 0.5f;
                        break;
                }
                if (!Main.rand.NextBool(6))
                {
                    int type = Utils.SelectRandom(Main.rand, 6, 259, 31);
                    Dust dust5 = Dust.NewDustDirect(Projectile.position, 0, 0, type, 0f, 0f, 100);
                    dust5.position = Projectile.Center + vector5 * (60f + Main.rand.NextFloat() * 20f) * num16;
                    dust5.velocity = vector6 * (4f + 4f * Main.rand.NextFloat()) * num16 * num15;
                    dust5.noGravity = true;
                    dust5.noLight = true;
                    dust5.scale = 0.5f;
                    dust5.customData = this;
                    if (Main.rand.NextBool(4))
                    {
                        dust5.noGravity = false;
                    }
                }
            }
            Projectile.position = vector - Projectile.Size / 2;
            Projectile.position += vector2;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Player.ChangeDir(Projectile.direction);
            Player.heldProj = Projectile.whoAmI;
            Player.SetDummyItemTime(2);
            Player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
        }
    }
}