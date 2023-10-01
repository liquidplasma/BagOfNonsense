using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class SunRay : ModProjectile

    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunray");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 450;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.extraUpdates = 10;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
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
                    Color.Yellow * 0.5f,
                    Projectile.rotation,
                    new Vector2(size, Projectile.height / 2),
                    Projectile.scale - (0.05f * i),
                    SpriteEffects.None,
                    0);
            }
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = new(0, 0, texture.Width, texture.Height);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.Yellow, Projectile.rotation, HelperStats.GetDrawOrigin(texture), Projectile.scale * 1.15f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, HelperStats.GetDrawOrigin(texture), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DamageVariationScale *= 2f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.5f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowStarDust, 0, 0, 100, default, Main.rand.NextFloat(1.5f));
                var dust = Main.dust[dusty];
                dust.noGravity = true;
            }
        }
    }
}