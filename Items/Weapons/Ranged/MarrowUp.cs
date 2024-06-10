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
        private float
            timer,
            useTimeReduction,
            inaccuracy;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Marrow);
            Item.damage = 38;
            Item.knockBack = 2f;
            Item.useTime = Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 11f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.scale = 0.9f;
            Item.channel = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                timer++;
                if (timer <= 450)
                    useTimeReduction = timer / 225f; // climbs to 0.75f over 7.5 seconds
                else
                    inaccuracy = 3f;
            }
            else // reset if player stops firing the weapon
            {
                timer = useTimeReduction = 0;
                inaccuracy = 0f;
            }
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextBool())
                return false;
            return true;
        }
        public override float UseSpeedMultiplier(Player player)
        {
            return 1f + useTimeReduction; // item will be 75% faster when fully wind up
        }
      
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ProjectileID.BoneArrow;
            float spread = inaccuracy + Main.rand.NextFloat(0.1f, 2.4f);
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(spread));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speedy = Main.rand.NextFloat(0.85f, 1.25f);
            Projectile.NewProjectile(source, position, velocity * speedy, ProjectileID.BoneArrow, damage, knockback, player.whoAmI);
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