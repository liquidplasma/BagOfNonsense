using BagOfNonsense.Dusts;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    [AutoloadEquip(EquipType.Wings)]
    public class HardCoded : ModItem
    {
        private int frameCounter = 0;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 38;
            Item.value = 300000;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            frameCounter++;
            player.wingTimeMax = 180000;
            if (!hideVisual)
            {
                if (frameCounter >= 15)
                {
                    frameCounter = 0;
                    for (int i = 0; i < Utils.SelectRandom(Main.rand, 3, 5, 7, 9); i++)
                    {
                        int deloc;
                        if (player.direction < 0)
                            deloc = Main.rand.Next(12, 25);
                        else
                            deloc = Main.rand.Next(-24, -11);
                        int randY = Main.rand.Next(-16, 17);
                        int dust = Dust.NewDust(new Vector2(player.Center.X + deloc, player.Center.Y + randY), 6, 6, ModContent.DustType<HCP>(), (float)(deloc * 0.1f), (float)(randY * 0.1f), 15, default, 1f);
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1.1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9f;
            acceleration = 1.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WingsSolar)
                .AddIngredient(ItemID.WingsNebula)
                .AddIngredient(ItemID.WingsStardust)
                .AddIngredient(ItemID.WingsVortex)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}