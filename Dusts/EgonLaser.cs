using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Dusts
{
    public class EgonLaser : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 12, 12);
            dust.alpha = 0;
            dust.velocity = Vector2.Zero;
            dust.noGravity = true;
            dust.noLight = false;
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, Color.Cyan.ToVector3());
            dust.alpha += 30;
            dust.velocity = Vector2.Zero;
            if (dust.alpha >= 210)
                dust.active = false;
            return false;
        }
    }
}