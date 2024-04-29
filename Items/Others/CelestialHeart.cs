using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BagOfNonsense.Items.Others
{
    public class CelestialHeartModPlayer : ModPlayer
    {
        public bool consumedHeart;

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (consumedHeart)
                health.Base += 10 * Player.ConsumedLifeCrystals;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["celestialHeart"] = consumedHeart;
            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            consumedHeart = tag.GetBool("celestialHeart");
            base.LoadData(tag);
        }
    }

    public class CelestialHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeCrystal);
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            base.SetDefaults();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ConsumedLifeCrystals == Player.LifeCrystalMax;
        }

        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<CelestialHeartModPlayer>().consumedHeart)
                return null;

            player.GetModPlayer<CelestialHeartModPlayer>().consumedHeart = true;
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, Color.Cyan.ToVector3() * 0.33f);
            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}