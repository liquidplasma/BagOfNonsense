using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    internal class CrackedDoomArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cracked Doom Arrow");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 16;
            Item.rare = ItemRarityID.Cyan;
            Item.value = 500;
        }
    }
}