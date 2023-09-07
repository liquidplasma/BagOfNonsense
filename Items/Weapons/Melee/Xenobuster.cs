using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Melee
{
    public class Xenobuster : ModItem
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Xenobuster");

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<XenobusterProjGreen>();
            Item.shootSpeed = 16f;
            Item.knockBack = 4.5f;
            Item.width = 72;
            Item.height = 72;
            Item.damage = 150;
            Item.scale = 1.05f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
        }

        public override void HoldItem(Player player)
        {
            float mult = 1f;
            if (player.statDefense > 50) mult = 1.15f;
            if (player.statDefense > 100) mult = 1.3f;
            if (player.statDefense > 150) mult = 1.45f;
            if (player.statDefense > 200) mult = 1.6f;
            if (player.statDefense > 250) mult = 1.75f;
            if (player.statDefense > 300) mult = 1.9f;
            if (player.statDefense > 350) mult = 2.05f;
            Item.damage = (int)(150 * mult);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (!Main.rand.NextBool(2))
            {
                for (int i = 0; i < 8; i++)
                {
                    int type = Utils.SelectRandom(Main.rand, new int[2] { 229, 107 });
                    int dusty = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, type, Item.direction * 2, 0.0f, 150, default, 1f);
                    var dust = Main.dust[dusty];
                    Vector2 vector2 = Vector2.Multiply(dust.velocity, 0.2f);
                    dust.velocity = vector2;
                    Main.dust[dusty].noGravity = true;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousepos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            if (Main.player[player.whoAmI].gravDir == -1f)
                mousepos.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y;
            Vector2 spawn = player.Center + new Vector2(0, -Main.rand.Next(6, 12));
            Vector2 gotomouse = Vector2.Normalize(mousepos - player.Center);
            float randspawn = Main.rand.NextFloat(6f, 18f);
            float randspeed = Main.rand.NextFloat(1.4f, 1.5f);
            Projectile.NewProjectile(source, spawn.X + randspawn, spawn.Y + randspawn, gotomouse.X * (Item.shootSpeed * randspeed), gotomouse.Y * (Item.shootSpeed * randspeed), ModContent.ProjectileType<XenobusterProjGreen>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.InfluxWaver)
                .AddIngredient(ItemID.TerraBlade)
                .AddIngredient(ItemID.FragmentSolar, 20)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}