using Microsoft.Xna.Framework;
using Terraria;

namespace BagOfNonsense
{
    

    public class BiomeInformations
    {
        public string currentbuff = "???";
        public string name = "???";
        public Color Color = new(255, 0, 0);
        public Player Player { get; set; }

        /// <summary>
        /// Updates information according to player location.
        /// </summary>
        public void Update()
        {
            if (Player.Center.Y <= Main.worldSurface * 0.35)
            {
                currentbuff = "Increases flight time by 50%";
                name = "Space";
                Color = new Color(0, 62, 132);
            }
            else if (Player.position.Y < 2500)
            {
                currentbuff = "Increases flight time by 50%";
                name = "Sky";
                Color = new Color(255, 255, 194);
            }
            else if (Player.ZoneUnderworldHeight)
            {
                currentbuff = "Walk and swim on lava";
                name = "Underworld";
                Color = new Color(255, 101, 31);
            }
            else if (Player.ZoneDungeon)
            {
                currentbuff = "+8 defense, +8 life regen and +50 max health";
                name = "Dungeon";
                Color = new Color(0, 74, 140);
            }
            else if (Player.ZoneJungle)
            {
                currentbuff = "Covered in honey and can see enemies";
                name = "Jungle";
                Color = new Color(15, 194, 2);
            }
            else if (Player.ZoneCorrupt)
            {
                currentbuff = "+5 defense and life regen, +10% damage, +10% crit";
                name = "Corruption";
                Color = new Color(128, 0, 167);
            }
            else if (Player.ZoneCrimson)
            {
                currentbuff = "+5 defense and life regen, +10% damage, +10% crit";
                name = "Crimson";
                Color = Color.Red;
            }
            else if (Player.ZoneHallow)
            {
                currentbuff = "You're happy and can run faster";
                name = "Hallow";
                Color = Color.Cyan;
            }
            else if (Player.ZoneGlowshroom)
            {
                currentbuff = "Go into stealth automatically and increase damage and crit";
                name = "Mushroom";
                Color = new Color(0, 62, 236);
            }
            else if (Player.ZoneDesert || Player.ZoneUndergroundDesert)
            {
                currentbuff = "Can see creatures and dangers";
                name = "Desert";
                Color = new Color(194, 182, 140);
            }
            else if (Player.ZoneSnow)
            {
                currentbuff = "Immunity to cold debuffs";
                name = "Snow";
                Color = Color.White;
            }
            else
            {
                if (Player.ZoneRockLayerHeight || Player.ZoneNormalCaverns || Player.ZoneNormalUnderground)
                {
                    currentbuff = "15% mining speed and can see ore";
                    name = "Caverns";
                    Color = new Color(116, 66, 80);
                }
                else if (Player.ZoneBeach)
                {
                    currentbuff = "Fishing buffs";
                    name = "Ocean";
                    Color = new Color(43, 140, 255);
                }
                else
                {
                    currentbuff = "+10 defense and +15% damage";
                    name = "Forest";
                    Color = new Color(43, 163, 80);
                }
            }
        }
    }
}