using BagOfNonsense.Items.Ammo;
using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class MarrowUp : ModItem
    {
        private int timer, actualDamage;

        private float radiansadd;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Marrowned");
            // Tooltip.SetDefault("50% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Marrow);
            Item.damage = 38;
            Item.knockBack = 2f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BoneArrow;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 12f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.scale = 0.9f;
            Item.channel = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.channel)
            {
                timer += 9;
                if (timer > 27)
                {
                    Item.useAnimation = Item.useTime = (int)(20 - timer / 45f * 10);
                    if (Item.useTime < 6)
                    {
                        Item.useAnimation = Item.useTime = 6;
                        actualDamage = (int)((player.HeldItem.damage - 8) * (player.GetDamage(DamageClass.Ranged).Additive + (player.GetDamage(DamageClass.Generic).Additive - 1f)));
                        radiansadd = 3f;
                    }
                }
            }

            return base.UseItem(player);
        }

        public override void HoldItem(Player player)
        {
            if (!player.channel && timer > 540)
            {
                timer = 0;
                Item.useAnimation = Item.useTime = 20;
                radiansadd = 0f;
                actualDamage = (int)(player.HeldItem.damage * (player.GetDamage(DamageClass.Ranged).Additive + (player.GetDamage(DamageClass.Generic).Additive - 1f)));
            }
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextBool())
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speedy = Main.rand.NextFloat(0.9f, 1.25f);
            float radians = radiansadd + Main.rand.NextFloat(0.1f, 2.4f);
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(radians));
            Projectile.NewProjectile(source, position, perturbedSpeed * speedy, ProjectileID.BoneArrow, actualDamage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(x: 3, y: 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ItemID.Marrow)
                 .AddIngredient(ItemID.ChlorophyteBar, 15)
                 .AddIngredient(ModContent.ItemType<ShadowEssence>(), 4)
                 .AddIngredient(ModContent.ItemType<ActualBoneArrow>(), 50)
                 .Register();

            Recipe.Create(ModContent.ItemType<ActualBoneArrow>(), 50)
                 .AddIngredient(ItemID.WoodenArrow, 50)
                 .AddIngredient(ItemID.Bone, 5)
                 .AddTile(TileID.WorkBenches)
                 .Register();
        }
    }
}