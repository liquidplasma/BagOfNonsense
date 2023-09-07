using BagOfNonsense.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    internal class CombineBalls : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PulseBall>();
            Item.damage = 1000;
            Item.width = 40;
            Item.height = 48;
            Item.maxStack = 999;
            Item.consumable = false;
            Item.ammo = Type;
            Item.knockBack = 7f;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.DamageType = DamageClass.Default;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ItemID.Cog)
                .AddIngredient(ModContent.ItemType<PulseAmmo>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}