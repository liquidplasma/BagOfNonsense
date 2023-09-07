using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class Bouncer : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 3.75f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 14;
            Item.height = 20;
            Item.shoot = ModContent.ProjectileType<BouncerProjectile>();
            Item.UseSound = SoundID.Item1;
            Item.damage = 142;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.maxStack = 120;
            Item.consumable = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.originalDamage = damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(6)
                .AddIngredient(ItemID.Grenade, 3)
                .AddIngredient(ItemID.ExplosivePowder, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}