using BagOfNonsense.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    public class Consume50 : ModPlayer
    {
        public bool Consume50bool = false;

        public override void ResetEffects()
        {
            Consume50bool = false;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (weapon.CountsAsClass(DamageClass.Ranged)
                && Main.rand.NextBool()
                && Consume50bool)
                return false;
            return true;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class HiddenShooterPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hidden Shooter Pants");
            /* Tooltip.SetDefault("50% chance to not consume thrown or ranged ammo\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player) => player.GetModPlayer<Consume50>().Consume50bool = true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.TitaniumBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.AdamantiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}