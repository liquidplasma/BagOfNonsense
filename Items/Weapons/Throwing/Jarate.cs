using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Throwing
{
    public class Jarate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jarate");
            // Tooltip.SetDefault("Coated enemies take 35% more damage");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.HolyWater);
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Throwing;
            Item.damage = 29;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.shoot = ModContent.ProjectileType<JarateProjBottle>();
            Item.shootSpeed = 15f;
            Item.autoReuse = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.MountedCenter;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            proj.velocity = proj.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.YellowDye)
                .AddIngredient(ItemID.Daybloom)
                .Register();
        }
    }
}