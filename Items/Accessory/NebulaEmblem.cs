using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class NebulaEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nebula Emblem");
            /* Tooltip.SetDefault("25% increased magic damage\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .25f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SorcererEmblem);
            recipe.AddIngredient(ItemID.FragmentNebula, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}