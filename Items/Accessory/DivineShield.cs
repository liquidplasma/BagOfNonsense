using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class DivineShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Divine Shield");
            /* Tooltip.SetDefault("Absorbs 25% of damage done to players on your team\n" +
                "Only active above 25% life\n" +
                "Grants immunity to knockback and fire blocks\n" +
                "Grants immunity to most debuffs\n" +
                "Puts a shell around the owner when below 50% life that reduces damage\n" +
                "'Also grants immunity to myopia'"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 14;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[46] = true;
            player.noKnockback = true;
            player.fireWalk = true;
            player.buffImmune[33] = true;
            player.buffImmune[36] = true;
            player.buffImmune[30] = true;
            player.buffImmune[20] = true;
            player.buffImmune[32] = true;
            player.buffImmune[31] = true;
            player.buffImmune[35] = true;
            player.buffImmune[23] = true;
            player.buffImmune[22] = true;
            player.AddBuff(BuffID.Lifeforce, 2, true);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AnkhShield)
                .AddIngredient(ItemID.PaladinsShield)
                .AddIngredient(ItemID.FrozenTurtleShell)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}