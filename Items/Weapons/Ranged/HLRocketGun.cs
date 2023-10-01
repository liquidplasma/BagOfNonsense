using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class HLRocketGun : ModItem
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Laser Guided Rocket Launcher");

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 58;
            Item.height = 32;
            Item.shoot = ProjectileID.RocketI;
            Item.knockBack = 3f;
            Item.useAmmo = AmmoID.Rocket;
            Item.damage = 115;
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.scale = 1f;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool CanUseItem(Player player)
        {
            return !(player.ownedProjectileCounts[ModContent.ProjectileType<HLRocket>()] >= 1);
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HLRocketGunProj>()] < 1 && player.whoAmI == Main.myPlayer)
            {
                Projectile shot = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, player.HeldItem.useAmmo), player.Center, Vector2.Zero, ModContent.ProjectileType<HLRocketGunProj>(), Item.damage, Item.knockBack, player.whoAmI, 30f);
                shot.originalDamage = Item.damage;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RocketLauncher)
                .AddIngredient(ItemID.Cog, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}