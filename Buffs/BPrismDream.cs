using BagOfNonsense.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BPrismDream : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prismatic Dreams");
            // Description.SetDefault("The star will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PrismaticDreamProj>()] > 0)
                player.buffTime[buffIndex] = 18000;
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}