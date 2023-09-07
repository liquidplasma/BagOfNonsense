using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Throwing
{
    public class ElementalDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Dagger");
            // Tooltip.SetDefault("'It's temperature changes everytime'\n" + "[c/2E86C1:ZoaklenMod Port]");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 9));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ShadowFlameKnife);
            Item.DamageType = DamageClass.Throwing;
            Item.width = 14;
            Item.height = 28;
            Item.rare = ItemRarityID.LightPurple;
            Item.mana = 0;
            Item.shootSpeed = 24f;
            Item.damage = 60;
            Item.autoReuse = true;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.shoot = ModContent.ProjectileType<ElementalDaggerProj>();
            Item.crit = 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MagicDagger)
                .AddIngredient(ItemID.ShadowFlameKnife)
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ModContent.ItemType<FireEssence>(), 5)
                .AddIngredient(ModContent.ItemType<ShadowEssence>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}