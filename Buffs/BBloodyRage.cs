using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BBloodyRage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bloody Orb");
            // Description.SetDefault("Increased damage due to fallen teammate");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
    }
}