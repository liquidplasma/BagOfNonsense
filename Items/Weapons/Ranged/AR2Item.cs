using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Ammo;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class AR2Item : ModItem
    {
        private int AR2Type => ModContent.ProjectileType<AR2Held>();

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 40;
            Item.knockBack = 1f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 27;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAmmo = ModContent.ItemType<PulseAmmo>();
            Item.value = Item.sellPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return base.AltFunctionUse(player);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[AR2Type] < 1 && player.whoAmI == Main.myPlayer)
            {
                int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Item.damage);
                Projectile shot = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, Item.useAmmo), player.Center, Vector2.Zero, AR2Type, damage, Item.knockBack, player.whoAmI);
                shot.originalDamage = damage;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ItemID.Megashark)
                 .AddIngredient(ItemID.ChlorophyteBar, 10)
                 .AddIngredient(ModContent.ItemType<PulseAmmo>(), 30)
                 .AddTile(TileID.MythrilAnvil)
                 .Register();
        }
    }
}