using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Items.Others;
using BagOfNonsense.Projectiles;
using BagOfNonsense.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    public class DoomArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 8.6f;
            Item.shoot = ModContent.ProjectileType<DoomArrowProj>();
            Item.damage = 16;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 7f;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.value = 1000;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CrackedDoomArrow>())
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 30)
                .AddIngredient(ItemID.LunarBar, 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}