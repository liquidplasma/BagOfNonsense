using BagOfNonsense.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Flasks
{
    public class JarateFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flask of Jarate");
            // Tooltip.SetDefault("Melee attacks makes enemies take 35% more damage");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FlaskofIchor);
            Item.width = 22;
            Item.height = 28;
            Item.maxStack = 30;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.buffTime = 72000;
            Item.buffType = ModContent.BuffType<BWeaponJarate>();
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(Item.buffType);
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(Item.buffType, Item.buffTime);
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.YellowDye)
                .AddIngredient(ItemID.BottledWater, 15)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }
    }
}