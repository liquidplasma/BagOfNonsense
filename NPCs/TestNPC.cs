using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.NPCs
{
    //The ExampleZombieThief is essentially the same as a regular Zombie, but it steals ExampleItems and keep them until it is killed, being saved with the world if it has enough of them.
    public class TestNPC : ModNPC
    {
        private int state;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("TestNPC");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 107;
            NPC.height = 86;
            NPC.damage = 14;
            NPC.defense = 0;
            NPC.lifeMax = 9999999;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 0; // Fighter AI, important to choose the aiStyle that matches the NPCID that we want to mimic
            NPC.knockBackResist = 0f;

            AIType = NPCID.TargetDummy; // Use vanilla zombie's type when executing AI code. (This also means it will try to despawn during daytime)
            AnimationType = NPCID.Zombie; // Use vanilla zombie's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
            Banner = Item.NPCtoBanner(NPCID.Zombie); // Makes this NPC get affected by the normal zombie banner.
            BannerItem = Item.BannerToItem(Banner); // Makes kills of this NPC go towards dropping the banner it's associated with.
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("TestNPC"),
            });
        }

        public override void AI()
        {
            NPC.frameCounter = 0;
            Player player = Main.player[NPC.target];
            state = 0;
            //Factors for calculations
            if (state == 1)
            {
                double deg = NPC.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 408; //Distance away from the player
                NPC.ai[1] += 3;
                /*Position the player based on where the player is, the Sin/Cos of the angle times the /
                /distance for the desired distance away from the player minus the projectile's width   /
                /and height divided by two so the center of the projectile is at the right place.     */
                NPC.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                NPC.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
        }
    }
}