using BagOfNonsense.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    public class PulseAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 8.6f;
            Item.shoot = ModContent.ProjectileType<PulseBullet>();
            Item.damage = 9;
            Item.width = 14;
            Item.height = 18;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.ammo = Type;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Cyan;
            Item.value = 800;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            CreateRecipe(90)
                .AddIngredient(ItemID.ChlorophyteBar)
                .AddIngredient(ItemID.Cog, 2)
                .AddIngredient(ItemID.MusketBall, 90)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}