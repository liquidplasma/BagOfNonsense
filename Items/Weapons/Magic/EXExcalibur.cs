using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Magic
{
    public class EXExcalibur : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Calesvol");
        }

        public override void SetDefaults()
        {
            Item.width = 91;
            Item.height = 91;
            Item.damage = 47;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.UseSound = null;
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<LightBeamModded>();
            Item.noMelee = true;
        }

        public override bool? UseItem(Player player)
        {
            Vector2 position = player.position + player.Size * Main.rand.NextFloat() + Utils.NextVector2Circular(Main.rand, 30, 30) + new Vector2(0, -64);
            Vector2 velocity = position.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
            int damage = (int)(player.HeldItem.damage * player.GetPlayerDamageMultiplier(DamageClass.Magic) * 1.5f);
            int type = ModContent.ProjectileType<LightBeamModded>();

            if (player.whoAmI == Main.myPlayer)
            {
                var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), position, velocity.RotatedByRandom(MathHelper.ToRadians(2)), type, damage, Item.knockBack, player.whoAmI, 1);
            }

            return base.UseItem(player);
        }

        public override void HoldItem(Player player)
        {
            Item.useTime = Item.useAnimation = 6;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<EXExcaliburHover>()] < 1)
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<EXExcaliburHover>(), 0, 0, player.whoAmI);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TrueExcalibur)
                .AddIngredient(ModContent.ItemType<PaladinBar>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}