using BagOfNonsense.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class DVirus : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Virus!");
            // Description.SetDefault("The antivirus is killing you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PlayerChanges>().virus = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<VanillaCustomizations>().virus = true;
        }
    }
}