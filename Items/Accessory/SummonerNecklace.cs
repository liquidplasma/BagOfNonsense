using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class SummonerNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Invoker Emblem");
            /* Tooltip.SetDefault("Increases your max number of minions by 3\n" +
                "15% increased minion damage\n" +
                "'If the pygmies knew what you did'\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<TikiLordEmblem>())
                return false;
            return true;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.GetDamage(DamageClass.Summon) += .15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PygmyNecklace);
            recipe.AddIngredient(ItemID.SummonerEmblem);
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.SummoningPotion, 30);
            recipe.AddTile(TileID.BewitchingTable);
            recipe.Register();
        }
    }
}