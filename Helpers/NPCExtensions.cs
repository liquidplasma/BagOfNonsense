using System.Linq;
using Terraria;

namespace BagOfNonsense.Helpers
{
    /// <summary>
    /// Usefull stuff related to NPCs
    /// </summary>
    internal class NPCExtensions
    {
        /// <summary>
        /// Disregards enemy damage scaling based on world difficulty
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public static int ActualDamage(int damage) => Main.masterMode ? damage / 6 : Main.expertMode ? damage / 4 : damage / 2;

        /// <summary>
        /// Contains NPCIDs for all stardust pillar enemies.
        /// </summary>
        internal static int[] StardustPillarEnemies => new int[] { 402, 407, 409, 410, 411 };

        /// <summary>
        /// Contains NPCIDs for all nebula pillar enemies.
        /// </summary>
        internal static int[] NebulaPillarEnemies => new int[] { 420, 421, 423, 424 };

        /// <summary>
        /// Contains NPCIDs for all solar pillar enemies.
        /// </summary>
        internal static int[] SolarPillarEnemies => Enumerable.Range(414, 6).ToArray();

        /// <summary>
        /// Contains NPCIDs for all vortex pillar enemies.
        /// </summary>
        internal static int[] VortexPillarEnemies => Enumerable.Range(425, 5).ToArray();

        /// <summary>
        /// Contains NPCIDs for post plantera dungeons gunners (Rocket, shotgun and sniper rifle).
        /// </summary>
        internal static int[] SkeletonGunners => Enumerable.Range(291, 3).ToArray();

        /// <summary>
        /// Contains NPCIDs for all of the post plantera Armored Bones skeletons.
        /// </summary>
        internal static int[] ArmoredBonesPostPlanteraIDs => Enumerable.Range(269, 12).ToArray();
    }
}