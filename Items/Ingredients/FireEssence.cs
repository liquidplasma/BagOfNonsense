using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class FireEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Core");
            // Tooltip.SetDefault("'A red smoke comes out of this stone'\n" + "[c/2E86C1:ZoaklenMod Port]");
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
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.HelFire);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.FieryGreatsword);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Cascade);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.Sunfury);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.HellwingBow);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.MoltenFury);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.PhoenixBlaster);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.Flamethrower);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(20);
            recipe.AddIngredient(ItemID.ElfMelter);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.Flamelash);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.InfernoFork);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.FlowerofFire);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();

            recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.ImpStaff);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}