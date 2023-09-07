using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Magic
{
    public class MonkeyGlasses : ModItem
    {
        private static int SunGod => ModContent.ProjectileType<SunGod>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Golden Monkey Glasses");
            // Tooltip.SetDefault("From another world");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.damage = 7;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.shootSpeed = 20f;
            Item.shoot = SunGod;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[SunGod] < 1)
            {
                var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
                projectile.originalDamage = damage;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 15)
                .AddRecipeGroup("Any evil material", 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class SuperMonkeyGlasses : ModItem
    {
        public override string Texture => "BagOfNonsense/Items/Weapons/Magic/MonkeyGlasses";

        private static int SunGod => ModContent.ProjectileType<SunGod>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Super Golden Monkey Glasses");
            // Tooltip.SetDefault("From another world");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.damage = 18;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.shootSpeed = 20f;
            Item.shoot = SunGod;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[SunGod] < 1)
            {
                var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
                projectile.originalDamage = damage;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MonkeyGlasses>())
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddIngredient(ItemID.ChlorophyteBar, 8)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}