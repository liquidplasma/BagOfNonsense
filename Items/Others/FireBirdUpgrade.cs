using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class FireBirdUpgrade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bird Snack");
            // Tooltip.SetDefault("Place into piggy bank to increase Fire Bird power");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 16;
            Item.height = 16;
            Item.value = 1000;
            Item.rare = ItemRarityID.Expert;
        }
    }
}