using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ingredients
{
    public class StellarFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Fragment");
            // Tooltip.SetDefault("'It is pulsing out'\n" + "[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = ItemRarityID.Cyan;
            Item.scale = 1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentVortex);
            recipe.AddIngredient(ItemID.FragmentNebula);
            recipe.AddIngredient(ItemID.FragmentSolar);
            recipe.Register();

            /*recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex);
			recipe.AddIngredient(ItemID.FragmentNebula);
			recipe.AddIngredient(ItemID.FragmentStardust);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex);
			recipe.AddIngredient(ItemID.FragmentSolar);
			recipe.AddIngredient(ItemID.FragmentStardust);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentNebula);
			recipe.AddIngredient(ItemID.FragmentSolar);
			recipe.AddIngredient(ItemID.FragmentStardust);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Item.scale -= 0.02f;
            if (Item.scale <= 0.5f)
                Item.scale = 1f;
            scale = Math.Abs(Item.scale);
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.scale -= 0.02f;
            if (Item.scale <= 0.5f)
                Item.scale = 1f;
            scale = Math.Abs(Item.scale);
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.position, Color.Gold.ToVector3());
            gravity = 0.5f;
            maxFallSpeed = 0.0f;
        }
    }
}