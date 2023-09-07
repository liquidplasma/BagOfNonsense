using BagOfNonsense.NPCs.Boss.MagicalCube;
using BagOfNonsense.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class ComplexCube : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Complex Cube");
            // Tooltip.SetDefault("Summons the techno doom");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 100;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<MagicalCube>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<MagicalCube>());
            /*Main.NewText("The cube gets bigger, and it's not happy.", 32, 255, 32);*/
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

        /*public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 10)
                .AddIngredient(ModContent.ItemType<StellarFragment>(), 3)
                .AddIngredient(ItemID.FragmentSolar, 3)
                .AddIngredient(ItemID.FragmentStardust, 3)
                .AddIngredient(ItemID.FragmentNebula, 3)
                .AddIngredient(ItemID.FragmentVortex, 3)
                .Register();
        }*/
    }
}