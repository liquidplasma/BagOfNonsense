using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Magic
{
    public class AirControlUnit : ModItem
    {
        private static int SpectreCenter => ModContent.ProjectileType<SpectreCenter>();

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.damage = 24;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item43;
            Item.shootSpeed = 20f;
            Item.shoot = SpectreCenter;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, SpectreCenter, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 15)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}