using BagOfNonsense.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BGhostlySword : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly Swords");
            // Description.SetDefault("");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GhostlySwordSummonProj>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<GhostlySwordSummonProjShooter>()] > 0)
                player.buffTime[buffIndex] = 18000;
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}