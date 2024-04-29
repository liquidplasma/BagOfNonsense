using BagOfNonsense.Dusts;
using BagOfNonsense.Items.Armor;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    public class PlayerChanges : ModPlayer
    {
        public bool shadowOrbMinion = false;
        public bool creeperMinion = false;
        public bool sfaMinion = false;
        public bool invaderMinion = false;
        public bool pacMinion = false;
        public bool practicalCube = false;

        public int damageType = -1;

        public bool markActivated = false;
        public int markFrames = 0;
        public int activeMark = -1;
        public int markDuration = 300;

        public bool virus = false;
        public int lastChange = 0;

        public bool blinkDashing = false;
        public int blinkDashingCounter = 0;

        public int stockedTeleports = 0;
        public float stTick = 0f;

        public bool
            ShinyStone,
            DivineShield;

        public override void ResetEffects()
        {
            shadowOrbMinion = false;
            creeperMinion = false;
            sfaMinion = false;
            invaderMinion = false;
            pacMinion = false;
            practicalCube = false;
            virus = false;
            if (blinkDashingCounter > 0)
            {
                blinkDashingCounter--;
            }
            else
            {
                blinkDashing = false;
            }
            if (lastChange > 0)
            {
                lastChange--;
            }
            if (stockedTeleports > 3)
            {
                stockedTeleports = 3;
            }
            if (!StellarNinja(Player))
            {
                stockedTeleports = 0;
            }
        }

        public override void UpdateDead()
        {
            virus = false;
            stockedTeleports = 0;
        }

        public override void UpdateBadLifeRegen()
        {
            if (virus)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 5;
            }
        }

        public override void PreUpdate()
        {
            if (blinkDashing)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Player.Distance(npc.Center) < 100f)
                    {
                        Player.ApplyDamageToNPC(npc, Main.rand.Next(280, 320), 0.5f, Player.direction, true);
                    }
                }
            }
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return blinkDashing;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (virus)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position + new Vector2(Main.rand.Next(-6, 7), 10f), Player.width + 4, Player.height + 4, ModContent.DustType<Neon>(), Player.velocity.X, Player.velocity.Y, 100, default(Color), 0.75f);
                    Main.dust[dust].velocity.Y += 3f;
                    drawInfo.DustCache.Add(dust);
                }
                fullBright = true;
            }
            if (stockedTeleports > 0)
            {
                stTick += 0.005f;
                if (stTick > 1f)
                {
                    stTick = 0f;
                }
                float num1007 = stTick * 6.28318548f;
                int dust = Dust.NewDust(drawInfo.Position, Player.width + 4, Player.height + 4, DustID.YellowTorch, Player.velocity.X, Player.velocity.Y, 100, default(Color), (float)(1f + stockedTeleports / 3f));
                Vector2 vector123 = Main.dust[dust].velocity;
                Main.dust[dust].position = Player.Center;
                vector123.Normalize();
                vector123 *= 30f;
                vector123 = vector123.RotatedBy((double)num1007, default);
                Main.dust[dust].velocity.X = 0f;
                Main.dust[dust].velocity.Y = 0f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position += vector123;
                Main.dust[dust].fadeIn = 0.001f;
                Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(Player.dye[1].dye, Player);
                drawInfo.DustCache.Add(dust);
                fullBright = true;
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.HeldItem.type == ModContent.ItemType<SpaceBlaster>())
                Player.scope = true;

            if (ShinyStone && Player.statLife <= Player.statLifeMax2 * 0.75)
                Player.AddBuff(BuffID.IceBarrier, 2, true);

            if (DivineShield)
            {
                if (Player.statLife <= Player.statLifeMax2 * 0.5)
                    Player.AddBuff(BuffID.IceBarrier, 5, true);

                Player.noKnockback = true;
                if (Player.statLife > Player.statLifeMax2 * 0.25)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherP = Main.player[i];

                        if (i == Player.whoAmI)
                            continue;

                        if (otherP.active && otherP.team == Player.team && Player.DistanceSQ(otherP.Center) <= 800 * 800)
                            otherP.AddBuff(BuffID.PaladinsShield, 2);
                    }
                }
            }
        }

        private static bool StellarNinja(Player player)
        {
            bool have = false;
            if (player.armor[0].type == ModContent.ItemType<StellarNinjaHelmet>() && player.armor[1].type == ModContent.ItemType<StellarNinjaBreastplate>() && player.armor[2].type == ModContent.ItemType<StellarNinjaLeggings>())
            {
                have = true;
            }
            return have;
        }
    }
}