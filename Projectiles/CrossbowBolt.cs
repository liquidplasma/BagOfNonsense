using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class CrossbowBolt : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Crossbow Bolt");

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Main.rand.NextFloat(1f) < 0.4f)
            {
                int dusttype;
                if (Main.rand.NextBool())
                    dusttype = 6;
                else
                    dusttype = 75;
                float x = Projectile.oldVelocity.X * (30f / 24f);
                float y = Projectile.oldVelocity.Y * (30f / 24f);
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X - x, Projectile.position.Y - y), 8, 8, dusttype, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, 100, default(Color), 1.8f);
                Main.dust[dusty].velocity *= 0.25f;
            }
            Projectile.FaceForward();
            float light = Projectile.alpha / 255f;
            Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 0.5f);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            if (Main.myPlayer == Projectile.owner)
            {
                var owner = Main.player[Projectile.owner];
                int damage = owner.inventory[owner.selectedItem].damage;
                float knockback = owner.inventory[owner.selectedItem].knockBack;
                int num630 = Main.rand.Next(5, 6); ;
                int num3;
                for (int num631 = 0; num631 < num630; num631 = num3 + 1)
                {
                    if (num631 % 2 != 1 || Main.rand.NextBool(3))
                    {
                        Vector2 vector22 = Projectile.position;
                        Vector2 value15 = Projectile.oldVelocity;
                        value15.Normalize();
                        value15 *= 8f;
                        float num632 = (float)Main.rand.Next(-35, 36) * 0.01f;
                        float num633 = (float)Main.rand.Next(-35, 36) * 0.01f;
                        vector22 -= value15 * (float)num631;
                        num632 += Projectile.oldVelocity.X / 5f;
                        num633 += Projectile.oldVelocity.Y / 5f;
                        int num634 = Projectile.NewProjectile(owner.GetSource_FromThis(), vector22.X, vector22.Y, num632, num633, ModContent.ProjectileType<HolyCross>(), (int)(damage * 0.8f), knockback * 0.33f, Main.myPlayer, 0f, 0f); ;
                        Main.projectile[num634].DamageType = DamageClass.Ranged;
                        Main.projectile[num634].penetrate = 2;
                    }
                    num3 = num631;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, Projectile.direction * 2, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}