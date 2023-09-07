using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Pets
{
    public class PracticalCube : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Practical Cube");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CompanionCube);
            AIType = ProjectileID.CompanionCube;
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.companionCube = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            PlayerChanges modPlayer = player.GetModPlayer<PlayerChanges>();
            if (player.dead)
            {
                modPlayer.practicalCube = false;
            }
            if (modPlayer.practicalCube)
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}