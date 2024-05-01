using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class GodlyEmblem : ModItem
    {
        public static readonly Color[] LerpColor = new Color[]
         {
            Color.Red,
            Color.Blue,
            Color.Purple,
            Color.Orange
         };

        private int numColors = LerpColor.Length;

        private int nextIndex => (index + 1) % numColors;
        private int index => (int)((Main.GameUpdateCount / 60) % numColors);

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lunar Emblem");
            // Tooltip.SetDefault("33% increased damage and 15% increased crit chance.");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = 400000;
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Color ambientColor = Color.Lerp(LerpColor[index], LerpColor[nextIndex], Main.GameUpdateCount % 60 / 60f);
            Lighting.AddLight(Item.Center, ambientColor.ToVector3());
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .33f;
            player.GetDamage(DamageClass.Magic) += .33f;
            player.GetDamage(DamageClass.Throwing) += .33f;
            player.GetDamage(DamageClass.Summon) += .33f;
            player.GetDamage(DamageClass.Ranged) += .33f;
            player.GetCritChance(DamageClass.Magic) += 15;
            player.GetCritChance(DamageClass.Generic) += 15;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetCritChance(DamageClass.Magic) += 15;
            player.GetCritChance(DamageClass.Throwing) += 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 30)
                .AddIngredient(ItemID.LunarOre, 20)
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ModContent.ItemType<StardustEmblem>())
                .AddIngredient(ModContent.ItemType<SolarEmblem>())
                .AddIngredient(ModContent.ItemType<VortexEmblem>())
                .AddIngredient(ModContent.ItemType<NebulaEmblem>())
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}