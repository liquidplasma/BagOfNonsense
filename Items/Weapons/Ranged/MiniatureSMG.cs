using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class MiniatureSMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Miniature SMG");
            // Tooltip.SetDefault("45% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.knockBack = 1f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 4;
            Item.reuseDelay = 14;
            Item.width = 54;
            Item.height = 30;
            Item.shoot = AmmoID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item31;
            Item.damage = 8;
            Item.shootSpeed = 12.75f;
            Item.noMelee = true;
            Item.value = 150000;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.scale = 0.8f;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextFloat(1f) < 0.45f) return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(4));
            Projectile.NewProjectile(source, position.X, position.Y, spread.X, spread.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.IllegalGunParts, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}