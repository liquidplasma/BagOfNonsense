using BagOfNonsense.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.NPCs
{
    public class VanillaCustomizations : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool virus = false;

        public override void ResetEffects(NPC npc)
        {
            virus = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (virus)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 125;
                if (damage < 25)
                {
                    damage = 25;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (virus)
            {
                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Neon>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].velocity.Y -= 0.5f;
                Lighting.AddLight(npc.position, Main.dust[dust].color.R / 255, Main.dust[dust].color.G / 255, Main.dust[dust].color.B / 255);
            }
        }
    }
}