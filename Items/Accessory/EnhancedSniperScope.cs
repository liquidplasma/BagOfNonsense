using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class EnhancedSniperScope : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Enhanced Sniper Scope");
            /* Tooltip.SetDefault("Increases view range for guns (Right click to zoom out)\n" +
                "15% increased ranged damage and critical strike chance\n" +
                "Enemies are less likely to target you\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = 100000;
            Item.rare = ItemRarityID.Cyan;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetCritChance(DamageClass.Ranged) += .15f;
            player.aggro -= 1200;
            Item heldItem = player.HeldItem;
            if (heldItem.useAmmo == AmmoID.Bullet || heldItem.useAmmo == AmmoID.Stake || heldItem.useAmmo == 323 || heldItem.useAmmo == 23 || heldItem.useAmmo == 771)
                player.scope = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ReconScope);
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}