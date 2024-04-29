using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Accessory;
using BagOfNonsense.Items.Ammo;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Items.Others;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.NPCs
{
    public class BossBagAddtions : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.DestroyerBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ProjectileDeflectorAccessory>(), 5, 1, 1));

            if (item.type == ItemID.WallOfFleshBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BullseyeEmblem16>(), 3, 1, 1));

            if (item.type == ItemID.MoonLordBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BullseyeEmblem16>(), 1, 20, 34));
        }
    }

    public class BagOfNonsenseDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPCExtensions.SkeletonGunners.Contains(npc.type))
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GhostlyArrow>(), 8, 1, 1));

            if (npc.type == NPCID.TheDestroyer)
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ProjectileDeflectorAccessory>(), 5, 1, 1));
        }
    }

    public class ZoaklenDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Clinger)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedEye>(), 15, 1, 1));

            if (npc.type == NPCID.IchorSticker)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IchorSack>(), 15, 1, 1));

            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrackedDoomArrow>(), 3, 1, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonFragment>(), 1, 17, 40));
            }

            if (npc.type == NPCID.Paladin)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PaladinBar>(), 1, 14, 27));

            if (npc.type == NPCID.SkeletonArcher)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ActualBoneArrow>(), 2, 39, 66));
        }
    }
}