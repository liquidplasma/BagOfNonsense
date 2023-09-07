using BagOfNonsense.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BPacman : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pacman");
            // Description.SetDefault("The eater of bits will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PlayerChanges modPlayer = player.GetModPlayer<PlayerChanges>();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Pacman>()] > 0)
            {
                modPlayer.pacMinion = true;
            }
            if (!modPlayer.pacMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}