using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class ChloroMinigun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chloro Rifle");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Uzi);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.width = 96;
            Item.height = 36;
            Item.shoot = AmmoID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item41;
            Item.damage = 21;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.knockBack = 1.75f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void HoldItem(Player player)
        {
            SoundStyle sound = new("BagOfNonsense/Sounds/Custom/sg552-1");
            Item.UseSound = sound with
            {
                Volume = 0.266f,
                Pitch = Main.rand.NextFloat(-.08f, .08f)
            };
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;
            if (Collision.CanHit(position, 1, 1, position + muzzleOffset, 1, 1))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2));
            float scale = 1f - Main.rand.NextFloat() * .3f;
            perturbedSpeed *= scale;
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * Main.rand.NextFloat(0.7f, 1.3f), perturbedSpeed.Y * Main.rand.NextFloat(0.7f, 1.3f), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.ChlorophyteBar, 18)
               .AddIngredient(ItemID.Minishark)
               .AddTile(TileID.MythrilAnvil)
               .Register();
        }
    }
}