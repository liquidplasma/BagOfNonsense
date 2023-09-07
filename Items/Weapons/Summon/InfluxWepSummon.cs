using BagOfNonsense.Buffs;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Summon
{
    public class InfluxWepSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Influx Waver");
            // Tooltip.SetDefault("Influx Waver but good");
        }

        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 100000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item44;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<BWaver>();
            Item.shoot = ModContent.ProjectileType<WaverSummon>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer, ai2: player.ownedProjectileCounts[Item.shoot] + 1);
            projectile.originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.InfluxWaver)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}