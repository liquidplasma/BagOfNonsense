using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PlasmaBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunray");
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 56;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 390;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.extraUpdates = 10;
            Projectile.light = 1f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color projectileColor = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            float size = (TextureAssets.Projectile[Projectile.type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            for (int i = 1; i < 5; i++)
            {
                float X = Projectile.velocity.X * i;
                float Y = Projectile.velocity.Y * i;
                Color getAlpha = Projectile.GetAlpha(projectileColor);
                float afterImage = 0f;
                if (i == 1)
                    afterImage = 0.4f;
                if (i == 2)
                    afterImage = 0.3f;
                if (i == 3)
                    afterImage = 0.2f;
                if (i == 4)
                    afterImage = 0.1f;
                getAlpha.R = (byte)(getAlpha.R * afterImage);
                getAlpha.G = (byte)(getAlpha.G * afterImage);
                getAlpha.B = (byte)(getAlpha.B * afterImage);
                getAlpha.A = (byte)(getAlpha.A * afterImage);
                Main.EntitySpriteDraw(Projectile.MyTexture(),
                    new Vector2(Projectile.position.X - Main.screenPosition.X + size - X, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY - Y),
                    new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()),
                    getAlpha,
                    Projectile.rotation,
                    new Vector2(size, Projectile.height / 2),
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 7; i++)
            {
                int dusty = Dust.NewDust(target.position, target.width, target.height, DustID.PurpleTorch, 0, 0, 100, default, Main.rand.NextFloat(3.5f));
                var dust = Main.dust[dusty];
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.FaceForward();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0, 0, 100, default, Main.rand.NextFloat(3.5f));
                var dust = Main.dust[dusty];
                dust.noGravity = true;
            }
        }
    }
}