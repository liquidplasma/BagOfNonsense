using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class PhantasmalCelebration : ModItem
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Phantasm Buster");

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Phantasm);
            Item.damage = 45;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.noUseGraphic = false;
            Item.shoot = ModContent.ProjectileType<PhantasmalGun>();
            Item.value = Item.sellPrice(0, 50, 0, 0);
        }

        public override void HoldItem(Player player)
        {
            player.GetArmorPenetration(DamageClass.Generic) = 1000;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => false;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseposX = Main.mouseX + Main.screenPosition.X - position.X;
            float mouspoxY = Main.mouseY + Main.screenPosition.Y - position.Y;
            int getdamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            int getknockback = (int)player.inventory[player.selectedItem].knockBack;
            int bow = Projectile.NewProjectile(source, position.X, position.Y, mouseposX, mouspoxY, ModContent.ProjectileType<PhantasmalCelebrationArrow>(), getdamage, getknockback, player.whoAmI, 0f, 0f);
            int gun = Projectile.NewProjectile(source, vector2.X, vector2.Y, mouseposX, mouspoxY, ModContent.ProjectileType<PhantasmalGun>(), getdamage, getknockback, player.whoAmI, 5 * Main.rand.Next(0, 10), 0f);
            Main.projectile[bow].noDropItem = true;
            Main.projectile[gun].alpha = 0;
            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Phantasm)
                .AddIngredient(ItemID.VortexBeater)
                .AddIngredient(ItemID.LunarBar, 45)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 30)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}