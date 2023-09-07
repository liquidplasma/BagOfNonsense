using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class FireBird : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Bird");
            // Tooltip.SetDefault("Rain down phoenixes on your foes\nDeals double damage to ice enemies");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.knockBack = 3.75f;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.channel = true;
            Item.width = 54;
            Item.height = 54;
            Item.shoot = ModContent.ProjectileType<FireBirdProj>();
            Item.UseSound = null;
            Item.damage = 17;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.sellPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FireBirdBase>()] <= 1 && player.whoAmI == Main.myPlayer)
            {
                for (int i = 1; i < 9; i++)
                {
                    var shot = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, Item.useAmmo), player.Center, Vector2.Zero, ModContent.ProjectileType<FireBirdBase>(), Item.damage, Item.knockBack, player.whoAmI, 0, i);
                    shot.originalDamage = Item.damage;
                }
            }
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<FireBirdBase>() && proj.owner == player.whoAmI && proj.active) return true;
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FireEssence>(), 8)
                .AddIngredient(ItemID.TitaniumBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}