using BagOfNonsense.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class DColdtouch : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cold touch");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCsDebuffLogic>().coldtouch = true;
        }
    }
}