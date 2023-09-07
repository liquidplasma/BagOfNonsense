using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class CursedItems : ModPlayer
    {
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            for (int l = 3; l < 8 + Player.extraAccessorySlots; l++)
            {
                if (!Player.armor[l].expertOnly || Main.expertMode)
                {
                    if (Player.armor[l].type == ModContent.ItemType<IchorSack>())
                    {
                        npc.AddBuff(BuffID.Ichor, 420, false);
                    }
                    if (Player.armor[l].type == ModContent.ItemType<CursedEye>())
                    {
                        npc.AddBuff(BuffID.CursedInferno, 420, false);
                    }
                }
            }
        }
    }

    public class CursedEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cursed Eye");
            /* Tooltip.SetDefault("Grants immunity to Cursed Inferno debuff\n" +
                "Inflicts Cursed Inferno debuff for 7 seconds to enemies who damage you\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.CursedInferno] = true;
        }
    }
}