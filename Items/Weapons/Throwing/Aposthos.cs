using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Throwing
{
    public class Aposthos : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aposthos");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BoneJavelin);
            Item.width = 68;
            Item.height = 62;
            Item.damage = 67;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<AposthosProj>();
            Item.shootSpeed = 24f;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Throwing;
            Item.consumable = false;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override void HoldItem(Player player)
        {
            int rndust = Utils.SelectRandom(Main.rand, 74, 75);
            if (Main.rand.NextBool(12))
            {
                int dusty = Dust.NewDust(player.Center, player.width, player.height, rndust, 0f, 0f, 100);
                Main.dust[dusty].alpha = 200;
                Dust dust2 = Main.dust[dusty];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[dusty];
                dust2.scale += Main.rand.NextFloat(0.1f, 0.3f);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var projy = Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);
            projy.velocity = (projy.DirectionFrom(Main.MouseWorld) * Item.shootSpeed).RotatedByRandom(MathHelper.ToRadians(45));
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Sunfire>())
                .AddIngredient(ModContent.ItemType<Nightfire>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}