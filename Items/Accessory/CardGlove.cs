using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class CardGlove : ModItem
    {
        /*public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Back);
			return true;
		}*/

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Card Glove");
            /* Tooltip.SetDefault("10% increased cards damage\n" +
                "33% chance to not consume thrown cards"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 1000;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        /*public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 10)
            .AddIngredient(ItemID.Cobweb, 3)
            .AddTile(TileID.Loom)
            .Register();
        }*/
    }
}