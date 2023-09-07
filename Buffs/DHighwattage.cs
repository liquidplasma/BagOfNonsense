using BagOfNonsense.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class DHighwattage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("High wattage");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCsDebuffLogic>().highwattage = true;
        }
    }
}