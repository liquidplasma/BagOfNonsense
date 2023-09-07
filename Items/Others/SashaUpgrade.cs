using BagOfNonsense.Helpers;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    internal class SandvichNPCDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPCExtensions.VortexPillarEnemies.Contains(npc.type))
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SashaUpgrade>(), 40, 1, 1));
        }
    }

    public class SashaUpgrade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sandvich");
            /* Tooltip.SetDefault("Sandvich makes me strong!\n" +
                "Place in piggy bank to increase Sasha power"); */
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}