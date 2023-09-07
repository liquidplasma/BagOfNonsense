using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class MoltenSMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Molten SMG");
            // Tooltip.SetDefault("57% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 3;
            Item.reuseDelay = 15;
            Item.width = 62;
            Item.height = 32;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item31;
            Item.damage = 16;
            Item.shootSpeed = 14.5f;
            Item.noMelee = true;
            Item.value = 450000;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.scale = 0.8f;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextFloat(1f) < 0.57f) return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(8))
            {
                type = ProjectileID.BallofFire;
                damage = (int)(damage * 1.5f);
            }
            Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(4));
            var proj = Projectile.NewProjectileDirect(source, position, spread, type, damage, knockback, player.whoAmI);
            proj.DamageType = DamageClass.Ranged;
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-12, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.HellstoneBar, 15)
               .AddIngredient(ModContent.ItemType<MiniatureSMG>())
               .AddTile(TileID.Hellforge)
               .Register();
        }
    }
}