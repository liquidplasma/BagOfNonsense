using BagOfNonsense.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BInvader : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Invader");
            // Description.SetDefault("The invader will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PlayerChanges modPlayer = player.GetModPlayer<PlayerChanges>();
            if (GetTotalInvaders(player) > 0)
            {
                modPlayer.invaderMinion = true;
            }
            if (!modPlayer.invaderMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }

        private int GetTotalInvaders(Player player)
        {
            return (player.ownedProjectileCounts[ModContent.ProjectileType<Invader1>()] + player.ownedProjectileCounts[ModContent.ProjectileType<Invader2>()] + player.ownedProjectileCounts[ModContent.ProjectileType<Invader3>()]);
        }
    }
}