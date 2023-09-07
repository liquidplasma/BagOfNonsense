using BagOfNonsense.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class MoonFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Moon Fragment");
            // Tooltip.SetDefault("'Seems to have some imbued power'\n" + "[c/2E86C1:ZoaklenMod Port]");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 3));
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
        }
    }
}