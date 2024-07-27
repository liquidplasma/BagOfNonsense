using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Dusts
{
    public class Audio : ModDust
    {
        private static Texture2D outline = ModContent.Request<Texture2D>("BagOfNonsense/Dusts/EgonLaserOutLine", AssetRequestMode.ImmediateLoad).Value;

        public override void OnSpawn(Dust dust)
        {
            dust.color = new Color(255, 255, 255);
            dust.alpha = 1;
            dust.velocity *= 0.2f;
            dust.noGravity = true;
            dust.noLight = false;
        }

        public override bool PreDraw(Dust dust)
        {
            return base.PreDraw(dust);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.05f;
            int oldAlpha = dust.alpha;
            dust.alpha = (int)(dust.alpha * 1.2);
            if (dust.alpha == oldAlpha)
                dust.alpha++;
            if (dust.alpha >= 255)
            {
                dust.alpha = 255;
                dust.active = false;
            }
            return false;
        }
    }
}