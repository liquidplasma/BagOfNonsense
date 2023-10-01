using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class CorruptedSac : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupted Sac");

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.damage = 1;
            Projectile.width = 11;
            Projectile.height = 11;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Dust dust;
            dust = Dust.NewDustPerfect(Projectile.Center, 16, Vector2.One, 0, Color.MediumPurple, 1f);
            dust.noGravity = true;
            Projectile.velocity.X = Projectile.velocity.X * 0.97f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Main.rand.Next(3) + 3; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_None(), Projectile.position.X, Projectile.position.Y, Main.rand.Next(-35, 36) * 0.02f * 10f, Main.rand.Next(-35, 36) * 0.02f * 10f, ProjectileID.TinyEater, (int)(Projectile.damage * 0.7), (int)(Projectile.knockBack * 0.35), Main.myPlayer, 0, 0.0f);
                }
            }

            int chance = Main.rand.Next(2);
            if (chance == 1)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
            }
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }
    }
}