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
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 160;
                if (damage < 20)
                    damage = 20;
            }
            if (coldtouch)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 64;
                if (damage < 8)
                    damage = 8;
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

        private void DustEffect(Entity target, float velocity, float velocityY, float scale, int dustType, Color color = default)
        {
            Dust dusty = Dust.NewDustDirect(target.position - new Vector2(2f, 2f), target.width + 4, target.height + 4, dustType, target.velocity.X * 0.4f, target.velocity.Y * 0.4f, 100, color, 2f);
            dusty.noGravity = true;
            dusty.velocity *= velocity;
            dusty.velocity.Y -= velocityY;
            if (Main.rand.NextBool(4))
            {
                dusty.noGravity = false;
                dusty.scale *= scale;
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
                        DustEffect(npc, Main.rand.NextFloat(2.1f, 4.8f), Main.rand.NextFloat(-0.8f, 0.8f), Main.rand.NextFloat(0.1f, 0.7f), DustID.Scorpion, Biome.Color);
                }
            }

            if (coldtouch && Main.rand.NextBool(6))
                DustEffect(npc, 2.1f, 0.4f, 0.5f, DustID.IceTorch);

            if (corrupttouch && Main.rand.NextBool(2))
                DustEffect(npc, 3.8f, 0.87f, 0.5f, DustID.ScourgeOfTheCorruptor);

            if (highwattage)
            {
                if (Main.rand.NextBool(2))
                    DustEffect(npc, 1.8f, 0.5f, 0.5f, DustID.Vortex);
                Lighting.AddLight(npc.position, Color.Cyan.ToVector3());
            }
        }
    }
}