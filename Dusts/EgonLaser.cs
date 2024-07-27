using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Dusts
{
    public class EgonLaser : ModDust
    {
        private static Texture2D outLine = ModContent.Request<Texture2D>("BagOfNonsense/Dusts/EgonLaserOutLine", AssetRequestMode.ImmediateLoad).Value;

        private static Texture2D inLine = ModContent.Request<Texture2D>("BagOfNonsense/Dusts/EgonLaser", AssetRequestMode.ImmediateLoad).Value;

        private static List<Vector2> pos = new();

        private Color MainBeam => new(47, 193, 203);

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 12, 12);
            dust.alpha = 0;
            dust.velocity = Vector2.Zero;
            dust.noGravity = true;
            dust.noLight = false;
        }

        public override bool PreDraw(Dust dust)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                ExtensionMethods.BetterEntityDraw(outLine, pos[i], outLine.Bounds, MainBeam, dust.rotation, outLine.Size() / 2, dust.scale, 0);
            }
            for (int i = 0; i < pos.Count; i++)
            {
                ExtensionMethods.BetterEntityDraw(inLine, pos[i], inLine.Bounds, Color.DarkCyan, dust.rotation, inLine.Size() / 2, dust.scale, 0);
            }
            pos.Clear();
            return false;
        }

        public override bool Update(Dust dust)
        {
            pos.Add(dust.position);
            dust.alpha += 35;
            dust.velocity = Vector2.Zero;
            if (dust.alpha >= 210)
                dust.active = false;
            return false;
        }
    }
}