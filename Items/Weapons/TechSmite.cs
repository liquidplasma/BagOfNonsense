using BagOfNonsense.Dusts;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons
{
    public class TechSmite : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tech Smite");
            // Tooltip.SetDefault("'Do you think even the worst person can change...?'" + "\n[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            Item.mana = 12;
            Item.channel = true;
            Item.damage = 500;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 26;
            Item.height = 24;
            Item.UseSound = SoundID.Item20;
            Item.useAnimation = 61;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 0f;
            Item.noMelee = true;
            Item.useTime = 61;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = 200000;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 vector15 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector15.X = (float)player.bodyFrame.Width - vector15.X;
            }
            if (player.gravDir != 1f)
            {
                vector15.Y = (float)player.bodyFrame.Height - vector15.Y;
            }
            vector15 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            Vector2 position17 = player.RotatedRelativePoint(player.position + vector15, true) - player.velocity;
            for (int num247 = 0; num247 < 1; num247++)
            {
                Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, ModContent.DustType<Neon>(), (float)(player.direction * 2), 0f, 100, default(Color), 0.5f)];
                dust.position = position17;
                dust.velocity *= 0f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.color = new Color(0, 255, 255);
                dust.velocity += player.velocity;
                if (Main.rand.NextBool(2))
                {
                    dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
                    dust.scale += Main.rand.NextFloat();
                    if (Main.rand.NextBool(2))
                    {
                        dust.customData = player;
                    }
                }
            }

            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 cursor;
            cursor.X = (float)Main.mouseX + Main.screenPosition.X;
            cursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
            if (player.itemAnimation == 60)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), cursor.X, 30, 0, 5, ModContent.ProjectileType<CyberInstaF>(), player.GetWeaponDamage(Item), 0, player.whoAmI);
            }
            else if (player.itemAnimation == 40)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), cursor.X, 30, 0, 5, ModContent.ProjectileType<CyberInstaF>(), player.GetWeaponDamage(Item), 0, player.whoAmI);
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), cursor.X + (float)Main.rand.Next(-128, -33), 30, 0, 5, ModContent.ProjectileType<CyberInstaF>(), player.GetWeaponDamage(Item), 0, player.whoAmI);
                }
            }
            else if (player.itemAnimation == 20)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), cursor.X, 30, 0, 5, ModContent.ProjectileType<CyberInstaF>(), player.GetWeaponDamage(Item), 0, player.whoAmI);
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), cursor.X + (float)Main.rand.Next(32, 129), 30, 0, 5, ModContent.ProjectileType<CyberInstaF>(), player.GetWeaponDamage(Item), 0, player.whoAmI);
                }
            }
        }

        public static Vector2 CalculateCenter(Dust dust)
        {
            return new Vector2(dust.position.X + 2, dust.position.Y + 2);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 vector15 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector15.X = (float)player.bodyFrame.Width - vector15.X;
            }
            if (player.gravDir != 1f)
            {
                vector15.Y = (float)player.bodyFrame.Height - vector15.Y;
            }
            vector15 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            Vector2 position17 = player.RotatedRelativePoint(player.position + vector15, true) - player.velocity;
            for (int num247 = 0; num247 < 1; num247++)
            {
                Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, ModContent.DustType<Neon>(), (float)(player.direction * 2), 0f, 100, default(Color), 0.5f)];
                dust.position = position17;
                dust.velocity *= 0f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.color = new Color(0, 255, 255);
                dust.velocity += player.velocity;
                if (Main.rand.NextBool(2))
                {
                    dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
                    dust.scale += Main.rand.NextFloat();
                    if (Main.rand.NextBool(2))
                    {
                        dust.customData = player;
                    }
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 cursor;
			cursor.X = (float)Main.mouseX + Main.screenPosition.X;
			cursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
			if(player.itemAnimation % 10 == 0)
			{
				Projectile.NewProjectile(cursor.X, 30, 0, 5, mod.ProjectileType("CyberInstaF"), damage, 0, 0);
			}*/
            return false;
        }
    }
}