using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class TerrariaBronzePlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terraria Bronze Plate");
            /* Tooltip.SetDefault("'Thanks for downloading ZoaklenMod'\n" +
                "'And for downloading BagOfNonsense'\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.rare = ItemRarityID.Orange;
            Item.width = 18;
            Item.height = 28;
            Item.autoReuse = true;
            Item.value = 420;
        }

        public override bool? UseItem(Player player)
        {
            if (player.itemTime == 0 && player.itemAnimation > 0 && player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = new(player.Center.X + Main.rand.Next(-4, 5), player.Center.Y + Main.rand.Next(-16, -8));
                    int dust = Dust.NewDust(pos, 6, 6, Main.rand.Next(59, 66), 0f, 0f, 6, default, 2f);
                    Main.dust[dust].velocity.X = (pos.X - player.Center.X) / Main.rand.Next(-16, 17);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity.Y = (pos.Y - player.Center.Y) / 4;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar)
                .AddCondition(Condition.NearWater)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}