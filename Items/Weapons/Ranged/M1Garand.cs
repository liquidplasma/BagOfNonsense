using BagOfNonsense.Items.Ammo;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using System.Security.AccessControl;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class M1Garand : ModItem
    {
        private int M1Type => ModContent.ProjectileType<M1GarandHeld>();

        public override void SetDefaults()
        {            
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.width = 70;
            Item.height = 16;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 21;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Ranged;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[M1Type] < 1 && player.whoAmI == Main.myPlayer)
            {
                int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Item.damage);
                Projectile shot = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, Item.useAmmo), player.Center, Vector2.Zero, M1Type, damage, Item.knockBack, player.whoAmI);
                shot.originalDamage = damage;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Musket)
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}