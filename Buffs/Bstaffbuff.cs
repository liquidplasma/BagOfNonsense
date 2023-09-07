using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class Bstaffbuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Staff Buff");
            // Description.SetDefault("You feel stronger just by holding it.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 15;
            player.statLifeMax2 += 100;
            player.lifeRegen += 20;
            player.statManaMax2 += 100;
            player.manaRegen += 25;
            player.GetCritChance(DamageClass.Magic) += 8;
            player.noKnockback = true;
        }
    }
}