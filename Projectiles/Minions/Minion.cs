using BagOfNonsense.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    public abstract class Minion : ModProjectile
    {
        public override void AI()
        {
            CheckActive();
            //Behavior();
        }

        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<BInvader>());
            }
            if (player.HasBuff(ModContent.BuffType<BInvader>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        //public abstract void Behavior();
    }
}