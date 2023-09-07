using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.NPCs
{
    public class NPCsDebuffLogic : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool
            highwattage,
            corrupttouch,
            coldtouch,
            greenrotDebuff;

        public int ChamaleonPlayerIndex;

        public override void ResetEffects(NPC npc)
        {
            highwattage = false;
            corrupttouch = false;
            coldtouch = false;
            greenrotDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (greenrotDebuff)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= 160;
                if (damage < 20) damage = 20;
            }
            if (coldtouch)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= 64;
                if (damage < 8) damage = 8;
            }
            if (corrupttouch)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                int corrupttouch = 0;
                for (int i = 0; i < 1000; i++)
                {
                    var p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<CorruptSpearProj>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI)
                        corrupttouch++;
                }
                npc.lifeRegen -= corrupttouch * 20 * 20;
                if (damage < corrupttouch * 3)
                    damage = corrupttouch * 100;
            }
            if (highwattage)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= 300;
                if (damage < 20) damage = 20;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.player.IndexInRange(ChamaleonPlayerIndex))
            {
                Player Player = Main.player[ChamaleonPlayerIndex];
                if (greenrotDebuff && Player.active)
                {
                    BiomeInformations Biome = new()
                    {
                        Player = Player
                    };
                    Biome.Update();
                    drawColor = Biome.Color;
                    Lighting.AddLight(npc.Center, Biome.Color.ToVector3());
                    if (Main.rand.NextBool(6))
                    {
                        int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Scorpion, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Biome.Color, 2f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= Main.rand.NextFloat(2.1f, 4.8f);
                        Main.dust[dust].velocity.Y -= Main.rand.NextFloat(-0.8f, 0.8f);
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dust].noGravity = false;
                            Main.dust[dust].scale *= Main.rand.NextFloat(0.1f, 0.7f);
                        }
                    }
                }
            }
            if (coldtouch)
            {
                if (Main.rand.NextBool(6))
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.IceTorch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.1f;
                    Main.dust[dust].velocity.Y -= 0.4f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }
            if (corrupttouch)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.ScourgeOfTheCorruptor, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 3.8f;
                    Main.dust[dust].velocity.Y -= 0.87f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            if (highwattage)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Vortex, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, Color.Cyan.ToVector3());
            }
        }
    }
}