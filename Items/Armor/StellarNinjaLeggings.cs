using BagOfNonsense.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StellarNinjaLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Ninja Leggings");
            /* Tooltip.SetDefault("20% increased throwing critical strike chance\n" +
                "You are now light as a ninja."); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.accRunSpeed = 10f;
            player.moveSpeed += 1.5f;
            player.rocketBoots += 1;
            player.pickSpeed *= 0.33f;
            player.GetCritChance(DamageClass.Throwing) += 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 12)
                .AddIngredient(ModContent.ItemType<StellarFragment>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}