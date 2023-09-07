using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class Cookie : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cookie");
            /* Tooltip.SetDefault("'Clap along if you feel...'\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.buffType = 26;
            Item.buffTime = 28800;
            Item.value = 1000;
            Item.rare = ItemRarityID.Orange;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Sunflower, 28800, true);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Acorn, 3)
                .AddIngredient(ItemID.Seed, 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}