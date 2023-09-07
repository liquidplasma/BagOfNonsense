using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class PaladinBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Paladin Bar");
            // Tooltip.SetDefault("[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 99;
            Item.value = 10000;
            Item.rare = ItemRarityID.Cyan;
        }
    }
}