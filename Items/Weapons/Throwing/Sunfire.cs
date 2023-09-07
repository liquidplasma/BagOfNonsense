using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Throwing
{
    public class Sunfire : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunfire");
            // Tooltip.SetDefault("Explodes on hit");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BoneJavelin);
            Item.width = 68;
            Item.height = 62;
            Item.damage = 36;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<SunfireProj>();
            Item.shootSpeed = 20f;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Throwing;
            Item.consumable = false;
            Item.useTime = 10;
            Item.useAnimation = 10;
        }

        public override void HoldItem(Player player)
        {
            int rndust = Utils.SelectRandom(Main.rand, 6, 259, 158);
            if (Main.rand.NextBool(12))
            {
                int dusty = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, rndust, 0f, 0f, 100);
                Main.dust[dusty].alpha = 200;
                Dust dust2 = Main.dust[dusty];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[dusty];
                dust2.scale += Main.rand.NextFloat(0.1f, 0.3f);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FireEssence>(), 6)
                .AddIngredient(ItemID.TitaniumBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}