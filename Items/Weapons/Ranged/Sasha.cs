using BagOfNonsense.Helpers;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class SashaDamageShow : InfoDisplay
    {
        public override bool Active()
        {
            return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Sasha>();
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            return "Damage done: " + Main.LocalPlayer.GetModPlayer<SashaCritTime>().damageTracker.ToString();
        }
    }

    public class Sasha : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 40;
            Item.knockBack = 1f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.width = 52;
            Item.height = 40;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 27;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.sellPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SashaProj>()] < 1 && player.whoAmI == Main.myPlayer)
                Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, Item.useAmmo), player.Center, Vector2.Zero, ModContent.ProjectileType<SashaProj>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(tip => tip.Name.StartsWith("Damage"));
            TooltipLine actualDamage = new(Mod, "actualDamage", (Main.LocalPlayer.HasSashaUpgrade() ? 54 : 27) + " ranged damage");
            tooltips.Insert(1, actualDamage);
            base.ModifyTooltips(tooltips);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.type == ModContent.ProjectileType<SashaProj>() && proj.owner == player.whoAmI)
                {
                    SashaProj heldProj = proj.ModProjectile as SashaProj;
                    return heldProj.animationControl == 1;
                }
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddIngredient(ItemID.RangerEmblem)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}