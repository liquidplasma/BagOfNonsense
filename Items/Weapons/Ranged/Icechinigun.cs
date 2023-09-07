using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class Icechinigun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Icechinigun");
            // Tooltip.SetDefault("40% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.width = 68;
            Item.height = 38;
            Item.shoot = ModContent.ProjectileType<IcegunProj1>();
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item11;
            Item.damage = 24;
            Item.shootSpeed = 14f;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.knockBack = 1.75f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextFloat(1f) <= 0.4f) return false;
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Utils.SelectRandom(Main.rand,
                           ModContent.ProjectileType<IcegunProj1>(),
                           ModContent.ProjectileType<IcegunProj2>(),
                           ModContent.ProjectileType<IcegunProj3>()
                           );
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(3));
            Projectile.NewProjectile(source, position, spread, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-15, 2);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FrostCore)
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.IllegalGunParts, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}