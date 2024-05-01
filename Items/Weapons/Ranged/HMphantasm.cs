using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class HMphantasm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantasmal");
            // Tooltip.SetDefault("33% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Phantasm); // copy defaults from an item, (basic values like damage only)
            Item.noUseGraphic = false;
            Item.damage = 20;
            Item.crit = 14;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.channel = true;
            Item.noMelee = true;
            Item.glowMask = 200;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void HoldItem(Player player) => player.phantasmTime = 2;

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextBool(3))
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 playercenter = player.RotatedRelativePoint(player.MountedCenter, true);
            for (int i = 0; i < 4; i++)
            {
                float speed = 1.225f * Main.rand.NextFloat(0.5f, 1.2f);
                Vector2 projectilev = Vector2.Multiply(Vector2.Multiply(velocity, speed), (float)(0.6 + Main.rand.NextFloat() * 0.8));
                if (float.IsNaN(projectilev.X) || float.IsNaN(projectilev.Y))
                    projectilev = Vector2.Negate(Vector2.UnitY);
                Vector2 playercenterrandom = Vector2.Add(playercenter, Utils.RandomVector2(Main.rand, -15f, 15f));
                int shooty = Projectile.NewProjectile(source, playercenterrandom.X, playercenterrandom.Y, projectilev.X, projectilev.Y, type, damage, 2, player.whoAmI, 5 * Main.rand.Next(0, 20), 0.0f);
                Main.projectile[shooty].noDropItem = true;
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(x: -5, y: 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}