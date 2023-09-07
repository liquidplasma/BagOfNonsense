using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class ThrowingMastery : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Throwing Mastery");
            /* Tooltip.SetDefault("Right-click the mentioned item to convert its damage to throwing\n" +
                "* This is not a material\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(30, 2));
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 99;
            Item.value = 10000;
            Item.rare = ItemRarityID.Pink;
        }

        /*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Shuriken, 100);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
    }
}