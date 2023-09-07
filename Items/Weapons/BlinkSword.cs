using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons
{
    public class BlinkSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blink Blade");
            // Tooltip.SetDefault("'The closer you are, the less you see'" + "\n[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 36;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.crit = 25;
            Item.holdStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 5f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CyberCut>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            bool can = false;
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.damage = 800;
                Item.useTime = 60;
                Item.useAnimation = 60;
                Item.useTurn = true;
                Item.crit = 100;
                Item.shoot = ModContent.ProjectileType<CyberCut>();

                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                Vector2 vector9;
                vector9.X = (float)Main.mouseX + Main.screenPosition.X;
                vector9.Y = (float)Main.mouseY + Main.screenPosition.Y;
                float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                Vector2 vector14;
                vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                if (player.gravDir == 1f)
                {
                    vector14.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
                }
                else
                {
                    vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                }
                vector14.X -= (float)(player.width / 2);
                if (vector14.X > 50f && vector14.X < (float)(Main.maxTilesX * 16 - 50) && vector14.Y > 50f && vector14.Y < (float)(Main.maxTilesY * 16 - 50))
                {
                    int num245 = (int)(vector14.X / 16f);
                    int num246 = (int)(vector14.Y / 16f);
                    if ((Main.tile[num245, num246].WallType != 87 || (double)num246 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector14, player.width, player.height))
                    {
                        if (Collision.CanHit(player.Center, 1, 1, vector14, 1, 1))
                        {
                            can = true;
                        }
                    }
                    else
                    {
                        can = false;
                    }
                }
                else
                {
                    can = false;
                }
            }
            else
            {
                Item.noMelee = false;
                Item.damage = 400;
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.useTurn = true;
                Item.shoot = 0;
                Item.crit = 4;
                can = true;
            }
            return can;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                Vector2 vector9;
                vector9.X = (float)Main.mouseX + Main.screenPosition.X;
                vector9.Y = (float)Main.mouseY + Main.screenPosition.Y;
                float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                Vector2 vector14;
                vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                if (player.gravDir == 1f)
                {
                    vector14.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
                }
                else
                {
                    vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                }
                vector14.X -= (float)(player.width / 2);
                if (vector14.X > 50f && vector14.X < (float)(Main.maxTilesX * 16 - 50) && vector14.Y > 50f && vector14.Y < (float)(Main.maxTilesY * 16 - 50))
                {
                    int num245 = (int)(vector14.X / 16f);
                    int num246 = (int)(vector14.Y / 16f);
                    if ((Main.tile[num245, num246].WallType != 87 || (double)num246 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector14, player.width, player.height))
                    {
                        player.Teleport(vector14, 1, 0);
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, vector14.X, vector14.Y, 1, 0, 0);
                    }
                }
            }
            return true;
        }

        private void Teleport(Vector2 newPos, int Style = 0, int extraInfo = 0)
        {
            Player player = Main.player[Main.myPlayer];
            try
            {
                player.grappling[0] = -1;
                player.grapCount = 0;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
                    {
                        Main.projectile[i].Kill();
                    }
                }
                int extraInfo2 = 0;
                if (Style == 4)
                {
                    extraInfo2 = player.lastPortalColorIndex;
                }
                float num = Vector2.Distance(player.position, newPos);
                player.position = newPos;
                player.fallStart = (int)(player.position.Y / 16f);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (num < new Vector2((float)Main.screenWidth, (float)Main.screenHeight).Length() / 2f + 100f)
                    {
                        int time = 0;
                        if (Style == 1)
                        {
                            time = 10;
                        }
                        Main.SetCameraLerp(0.1f, time);
                    }
                    else
                    {
                        Main.BlackFadeIn = 255;
                        Lighting.Clear();
                        Main.screenLastPosition = Main.screenPosition;
                        Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
                        Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
                        Main.instantBGTransitionCounter = 10;
                    }
                    if (Main.mapTime < 5)
                    {
                        Main.mapTime = 5;
                    }
                    Main.maxQ = true;
                    Main.renderNow = true;
                }
                if (Style == 4)
                {
                    player.lastPortalColorIndex = extraInfo;
                    extraInfo2 = player.lastPortalColorIndex;
                    player.portalPhysicsFlag = true;
                    player.gravity = 0f;
                }
                for (int j = 0; j < 3; j++)
                {
                    player.UpdateSocialShadow();
                }
                player.oldPosition = player.position;
                player.teleportTime = 1f;
                player.teleportStyle = Style;
            }
            catch
            {
            }
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation = player.Center;
            switch (player.direction)
            {
                case -1:
                    player.itemRotation = 109.7f;
                    player.itemLocation.X -= 10f;
                    break;

                case 1:
                    player.itemRotation = 66.2f;
                    player.itemLocation.X += 10;
                    break;
            }
        }

        private int GetWeaponCrit(Player player)
        {
            Item item = player.inventory[player.selectedItem];
            int crit = item.crit;
            if (item.CountsAsClass(DamageClass.Melee))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Melee));
            }
            else if (item.CountsAsClass(DamageClass.Magic))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Magic));
            }
            else if (item.CountsAsClass(DamageClass.Ranged))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Ranged));
            }
            else if (item.CountsAsClass(DamageClass.Throwing))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Throwing));
            }
            return crit;
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.Meowmere)
            .AddIngredient(ItemID.StarWrath)
            .AddIngredient(ModContent.ItemType<MoonFragment>(), 20)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}