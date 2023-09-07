using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class HolyCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Holy Crossbow");
            // Tooltip.SetDefault("Unleashes tiny crosses that homes in on enemies");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenBow);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 94;
            Item.height = 30;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.damage = 40;
            Item.shootSpeed = 14f;
            Item.knockBack = 6f;
            Item.rare = ItemRarityID.LightPurple;
            Item.noMelee = true;
            Item.value = 108000;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
            if (Collision.CanHit(position, 1, 1, position + muzzleOffset, 1, 1))
            {
                position += muzzleOffset;
            }
            Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(2));
            velocity.X = spread.X;
            velocity.Y = spread.Y;
            type = ModContent.ProjectileType<CrossbowBolt>();
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeesKnees)
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}