using BagOfNonsense.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PacPellets : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pac Pellets");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.noDropItem = true;
            Projectile.minion = true;
            AIType = 14;
        }

        public override void AI()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.timeLeft > 0 && proj.type == ModContent.ProjectileType<Pacman>() && proj.Distance(Projectile.Center) < 30)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}