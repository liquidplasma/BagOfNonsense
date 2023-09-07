using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class HolyCross : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Holy Cross");

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bee);
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 36;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 2000;
            Projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
        }

        public override void AI()
        {
            if (Main.rand.NextFloat(1f) < 0.1f)
            {
                int dusttype;
                if (Main.rand.NextBool(2))
                {
                    dusttype = 6;
                }
                else
                {
                    dusttype = 75;
                }
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dusttype, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.9f);
                Main.dust[dust].velocity *= 0.25f;
            }
            Projectile.FaceForward();
            float light = Projectile.alpha / 255f;
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.3f * light, 0.4f * light, 1f * light);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
    }
}