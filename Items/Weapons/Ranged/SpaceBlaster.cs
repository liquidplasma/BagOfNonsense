using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class SpaceBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Space Blaster");
            // Tooltip.SetDefault("'Headshots the Brain of Cthulhu'" + "\n[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.crit = 45;
            Item.width = 70;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useAmmo = 97;
            Item.UseSound = SoundID.Item40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 700;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.value = 200000;
            Item.knockBack = 8f;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
            float num117 = 0.314159274f;
            int num118 = 3;
            Vector2 vector7 = new(num78, num79);
            vector7.Normalize();
            vector7 *= 12f;
            bool flag11 = Collision.CanHit(vector2, 1, 1, vector2 + vector7, 1, 1);
            for (int num119 = 0; num119 < num118; num119++)
            {
                float num120 = num119 - (num118 - 1f) / 2f;
                Vector2 value9 = vector7.RotatedBy((double)(num117 * num120), default);
                if (!flag11)
                {
                    value9 -= vector7;
                }
                int num121 = Projectile.NewProjectile(source, vector2.X + value9.X, vector2.Y + value9.Y, num78, num79, type, damage, knockback, player.whoAmI, 0f, 0f);
                Main.projectile[num121].noDropItem = true;
            }
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.VortexBeater)
                .AddIngredient(ItemID.SniperRifle)
                .AddIngredient(ItemID.IllegalGunParts, 5)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 20)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}