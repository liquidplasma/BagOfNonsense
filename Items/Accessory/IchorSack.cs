using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class IchorSack : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ichor Sack");
            /* Tooltip.SetDefault("Grants immunity to Ichor debuff\n" +
                "Inflicts Ichor debuff for 7 seconds to enemies who damage you\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = 4;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[69] = true;
        }
    }
}