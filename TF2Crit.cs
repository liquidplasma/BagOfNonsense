using BagOfNonsense.Items.Armor;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace BagOfNonsense
{
    public class TF2Crit
    {
        private static int totaldamage;

        /// <summary>
        /// MINI CRIT!
        /// </summary>
        private static SoundStyle MiniCritHit => new("BagOfNonsense/Sounds/Crit/crit_hit_mini", 5)
        {
            Pitch = Main.rand.NextFloat(-0.15f, 0.15f),
            Volume = Main.rand.NextFloat(0.6f, 0.7f)
        };

        /// <summary>
        /// CRITICAL HIT!!!
        /// </summary>
        private static SoundStyle CritHit => new("BagOfNonsense/Sounds/Crit/crit_hit", 5)
        {
            Pitch = Main.rand.NextFloat(-0.15f, 0.15f),
            Volume = Main.rand.NextFloat(0.8f, 0.9f)
        };

        /// <summary>
        /// Makes a mini crit effect for NPCs
        /// </summary>
        /// <param name="target"></param>
        public static void MiniCritFX(NPC target)
        {
            SoundEngine.PlaySound(MiniCritHit, target.position);
            int critText = CombatText.NewText(target.getRect(), Color.Yellow, "MINI \nCRIT!", true);
            if (critText < 100)
            {
                Main.combatText[critText].lifeTime = 15;
                Main.combatText[critText].velocity.Y -= 24f;
            }
        }

        /// <summary>
        /// Makes a mini crit effect for players
        /// </summary>
        /// <param name="player"></param>
        public static void MiniCritFX(Player player)
        {
            SoundEngine.PlaySound(MiniCritHit, player.position);
            int critText = CombatText.NewText(player.getRect(), Color.Yellow, "MINI \nCRIT!", true);
            if (critText < 100)
            {
                Main.combatText[critText].lifeTime = 15;
                Main.combatText[critText].velocity.Y -= 24f;
            }
        }

        /// <summary>
        /// Used to create the crit effect from TF2
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="crit"></param>
        public static void CritSFXandText(NPC target, int damage)
        {
            SoundEngine.PlaySound(CritHit, target.position);
            if (target.type != NPCID.SolarCrawltipedeTail)
            {
                int critText = CombatText.NewText(target.getRect(), Color.LimeGreen, "CRITICAL\n  HIT!!!", true);
                if (critText < 100)
                {
                    Main.combatText[critText].lifeTime = 18;
                    Main.combatText[critText].velocity.Y -= 28f;
                }
                for (int i = 0; i < Main.combatText.Length; i++)
                {
                    if (Main.combatText[i].active == true && Main.combatText[i].text == damage.ToString() && (Main.combatText[i].color == CombatText.DamagedHostile || Main.combatText[i].color == CombatText.DamagedHostileCrit))
                    {
                        Main.combatText[i].active = false;
                    }
                }
                int damageText = CombatText.NewText(target.getRect(), Color.Red, damage.ToString(), true);
                if (damageText < 100)
                {
                    Main.combatText[damageText].lifeTime = 60;
                }
                if (Main.LocalPlayer.GetModPlayer<SuperCrit>().SuperCritBool)
                    totaldamage += damage;
            }
        }

        public static int UpdateNumber()
        {
            return totaldamage;
        }
    }
}