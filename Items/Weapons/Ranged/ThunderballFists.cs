using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class ThunderballFists : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Thunderball Fists");
            // Tooltip.SetDefault("Spawns a eletric ball on hit");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.width = 44;
            Item.height = 28;
            Item.shoot = ModContent.ProjectileType<ThunderballProj>();
            Item.knockBack = 0.3f;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item41;
            Item.damage = 82;
            Item.shootSpeed = 13f;
            Item.noMelee = true;
            Item.value = 250000;
            Item.scale = 1f;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(2));
            Projectile.NewProjectile(source, position.X, position.Y - 8f, spread.X, spread.Y, ModContent.ProjectileType<ThunderballProj>(), damage, knockback, Main.myPlayer);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PhoenixBlaster)
                .AddIngredient(ItemID.MartianConduitPlating, 33)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}