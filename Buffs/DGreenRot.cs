using BagOfNonsense.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class DGreenRot : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("GreenRot");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCsDebuffLogic>().greenrotDebuff = true;
        }
    }
}