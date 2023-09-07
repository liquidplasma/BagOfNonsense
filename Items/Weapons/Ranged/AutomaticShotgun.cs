using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class AutomaticShotgun : ModItem
    {
        private int timer;

        // public override void SetStaticDefaults() => DisplayName.SetDefault("Automatic Boomstick");

        public override void SetDefaults()
        {
            Item.knockBack = 3.75f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 42;
            Item.useTime = 42;
            Item.width = 50;
            Item.height = 14;
            Item.shoot = AmmoID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item36;
            Item.damage = 9;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.channel = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                timer++;
                if (timer > 15)
                {
                    Item.useAnimation = Item.useTime = (int)(42 - timer / 30f * 8);
                    if (Item.useAnimation < 12)
                        Item.useAnimation = Item.useTime = 12;
                }
            }
            else
            {
                timer = 0;
                Item.useAnimation = Item.useTime = 42;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 10f;
            if (Collision.CanHit(position, 1, 1, position + muzzleOffset, 1, 1))
                position += muzzleOffset;
            if (type == ProjectileID.Bullet)
                type = ProjectileID.BulletHighVelocity;
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                float scale = 1f - Main.rand.NextFloat() * .1575f;
                spread *= scale;
                Projectile.NewProjectile(source, position, spread, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Boomstick)
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}