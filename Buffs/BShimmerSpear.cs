using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BShimmerSpear : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Awakening");
            // Description.SetDefault("Power increased");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
    }
}