using BagOfNonsense.Items.Accessory;
using BagOfNonsense.Items.Weapons;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class MagicalCubeTreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
            // Tooltip.SetDefault("Right Click to open");
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.expert = true;
            Item.rare = ItemRarityID.Red;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<SuspiciousLookingJoystick>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlinkSword>(), 3, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TechSmite>(), 3, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BitCannon>(), 3, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CyberStaff>(), 3, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<HardCoded>()));
            base.ModifyItemLoot(itemLoot);
        }
    }
}