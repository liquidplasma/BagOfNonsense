using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class WormyShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wormy Shield");
            // Tooltip.SetDefault("Allows the player to dash into the enemy\nDouble tap a direction\nReduces damage taken by 17%");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Expert;
            Item.knockBack = 9f;
            Item.defense = 3;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.accessory = true;
            Item.expert = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.dashType = 2;
            player.endurance += 0.17f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EoCShield)
                .AddIngredient(ItemID.WormScarf)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}