using BagOfNonsense.Buffs;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Summon
{
    public class PrismaticDream : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prismatic Dreams");
            // Tooltip.SetDefault("A star will fight for you\nOnly one can be active.\nDoes not consume minion slots");
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 58;
            Item.height = 60;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 100000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<BPrismDream>();
            Item.shoot = ModContent.ProjectileType<PrismaticDreamProj>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PrismaticDreamProj>()] == 0;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
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
                .AddIngredient(ItemID.CrystalShard, 20)
                .AddIngredient(ModContent.ItemType<PaladinBar>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}