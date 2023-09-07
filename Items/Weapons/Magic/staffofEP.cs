﻿using BagOfNonsense.Buffs;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Magic
{
    public class staffofEP : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Staff of Extreme Prejudice");
            /* Tooltip.SetDefault("From another world.\n" +
                                "Holding it seems to make you stronger"); */
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WandofSparking);
            Item.width = 34;
            Item.height = 34;
            Item.damage = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Yellow;
            Item.mana = 9;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spread = MathHelper.ToRadians(360);
            float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            double startAngle = Math.Atan2(velocity.X, velocity.Y) - spread / 2;
            double deltaAngle = spread / 12;
            double offsetAngle;
            for (int j = 0; j < 12; j++)
            {
                offsetAngle = startAngle + deltaAngle * j;
                Projectile.NewProjectile(source, position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<staffproj>(), damage, 0, player.whoAmI);
            }
            return false;
        }

        public override void HoldItem(Player player) => player.AddBuff(ModContent.BuffType<Bstaffbuff>(), 2, true);

        public override Vector2? HoldoutOffset() => new Vector2(x: -8, y: 1);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WandofSparking)
                .AddIngredient(ItemID.DarkShard, 4)
                .AddIngredient(ItemID.SoulofNight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}