using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Melee
{
    public class CorruptSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Corrupstation");
            /* Tooltip.SetDefault("Ignores enemy defense\n" +
                                 "Killing enemies will spawn tiny eaters defined by\n" +
                                 "the amount of spears stuck into said enemy"); */
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<CorruptSpearProj>();
            Item.damage = 198;
            Item.width = 78;
            Item.height = 78;
            Item.UseSound = SoundID.Item39;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.consumable = false;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
        }

        public override void HoldItem(Player player)
        {
            player.GetArmorPenetration(DamageClass.Generic) = 1500;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CorruptSpearProj>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.DayBreak)
               .AddIngredient(ItemID.ScourgeoftheCorruptor)
               .AddIngredient(ItemID.BoneJavelin, 40)
               .AddIngredient(ItemID.FragmentSolar, 12)
               .AddIngredient(ItemID.LunarBar, 10)
               .AddIngredient(ModContent.ItemType<MoonFragment>(), 20)
               .AddTile(TileID.LunarCraftingStation)
               .Register();
        }
    }
}