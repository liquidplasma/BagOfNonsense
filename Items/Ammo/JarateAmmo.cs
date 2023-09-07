using BagOfNonsense.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    public class JarateAmmo : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jarate Bullet");
            // Tooltip.SetDefault("Coated enemies take 35% more damage");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MusketBall);
            Item.damage = 2;
            Item.width = 10;
            Item.height = 14;
            Item.shoot = ModContent.ProjectileType<JarateBullet>();
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe(999)
                .AddIngredient(ItemID.YellowDye)
                .AddIngredient(ItemID.EmptyBullet, 999)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}