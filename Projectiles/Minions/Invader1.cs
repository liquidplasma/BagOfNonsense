using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    public class Invader1 : InvaderAI
    {
        public override void CustomDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
        }

        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == player.whoAmI)
            {
                int a2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y + 5, 0, 10f, ModContent.ProjectileType<Invader2Shot>(), (int)(Projectile.damage * 0.4f), 0, player.whoAmI);
                Main.projectile[a2].DamageType = DamageClass.Summon;
                Main.projectile[a2].CritChance = 0;
            }
        }
    }
}