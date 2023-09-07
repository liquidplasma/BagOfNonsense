using BagOfNonsense.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class HiddenShooterCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hidden Shooter Coat");
            /* Tooltip.SetDefault("15% increased throwing and ranged damage\n" +
                "Enemies will focus other person if possible\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Throwing) += 0.15f;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.aggro -= 800;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.TitaniumBar, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.AdamantiteBar, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}