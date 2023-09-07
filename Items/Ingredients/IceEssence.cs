using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class IceEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Core");
            // Tooltip.SetDefault("'A blue smoke comes out of this stone'\n" + "[c/2E86C1:ZoaklenMod Port]");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 11));
        }

        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 25;
            Item.maxStack = 99;
            Item.value = 100000;
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.TitaniumBar, 3)
                .AddIngredient(ModContent.ItemType<IceEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();
            Recipe.Create(ItemID.TitaniumBar, 3)
                .AddIngredient(ModContent.ItemType<ShadowEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();
            Recipe.Create(ItemID.TitaniumBar, 3)
                .AddIngredient(ModContent.ItemType<FireEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();

            Recipe.Create(ItemID.AdamantiteBar, 3)
                .AddIngredient(ModContent.ItemType<IceEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();
            Recipe.Create(ItemID.AdamantiteBar, 3)
                .AddIngredient(ModContent.ItemType<ShadowEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();
            Recipe.Create(ItemID.AdamantiteBar, 3)
                .AddIngredient(ModContent.ItemType<FireEssence>())
                .AddIngredient(ItemID.FallenStar, 3)
                .Register();

            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.Amarok);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.Frostbrand);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.IceSickle);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(20);
            recipe.AddIngredient(ItemID.NorthPole);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.IceBoomerang);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.IceBow);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(20);
            recipe.AddIngredient(ItemID.BlizzardStaff);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.FlowerofFrost);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.FrostCore);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}