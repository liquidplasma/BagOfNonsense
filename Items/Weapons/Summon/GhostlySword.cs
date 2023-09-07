using BagOfNonsense.Buffs;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Summon
{
    public class GhostlySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly Sword");
            // Tooltip.SetDefault("A ghostly sword will protect you");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item44;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<BGhostlySword>();
            Item.shoot = ModContent.ProjectileType<GhostlySwordSummonProj>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int ThingToShoot() =>
                player.ownedProjectileCounts[ModContent.ProjectileType<GhostlySwordSummonProj>()] >= 1 ?
                ModContent.ProjectileType<GhostlySwordSummonProjShooter>() : ModContent.ProjectileType<GhostlySwordSummonProj>();
            type = ThingToShoot();
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Muramasa)
                .AddIngredient(ItemID.GoldBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}