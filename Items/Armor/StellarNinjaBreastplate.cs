using BagOfNonsense.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class StellarNinjaBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Ninja Breastplate");
            /* Tooltip.SetDefault("30% increased throwing damage\n" +
                "You are now resistant as a ninja."); */
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Throwing) += 0.3f;
            player.noKnockback = true;
            player.buffImmune[46] = true;
            player.buffImmune[47] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 16)
            .AddIngredient(ModContent.ItemType<StellarFragment>(), 20)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}