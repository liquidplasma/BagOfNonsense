using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class TikiLordEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tiki Lord Emblem");
            /* Tooltip.SetDefault("Increases your max number of minions by 6\n" +
                "45% increased minion damage and minions apply a variety of debuffs\n" +
                "The ultimate summoning\n"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 6;
            player.GetDamage(DamageClass.Summon) += 0.45f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SummonerNecklace>())
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 20)
                .AddIngredient(ItemID.FragmentStardust, 20)
                .AddTile(TileID.BewitchingTable)
                .Register();
        }
    }
}