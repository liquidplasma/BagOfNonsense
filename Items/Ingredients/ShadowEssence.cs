using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class ShadowEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shadow Core");
            // Tooltip.SetDefault("'A purple smoke comes out of this stone'\n" + "[c/2E86C1:ZoaklenMod Port]");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 11));
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 27;
            Item.maxStack = 99;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
               .AddIngredient(ItemID.ShadowFlameKnife)
               .AddTile(TileID.CrystalBall)
               .Register();

            CreateRecipe(5)
                .AddIngredient(ItemID.ShadowFlameBow)
                .AddTile(TileID.CrystalBall)
                .Register();

            CreateRecipe(3)
                .AddIngredient(ItemID.ShadowbeamStaff)
                .AddTile(TileID.CrystalBall)
                .Register();

            CreateRecipe(5)
                .AddIngredient(ItemID.ShadowFlameHexDoll)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}