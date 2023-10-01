using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class AposthosProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aposthos");
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 31;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 3;
            AIType = ProjectileID.PossessedHatchet;
            Projectile.scale = 0.75f;
            Projectile.extraUpdates = 1;
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
            if (Main.myPlayer == Player.whoAmI)
            {
                int actualDamage = Player.HeldItem.damage;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 source2 = Player.position + Player.Size * Main.rand.NextFloat();
                    var secondproj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), source2, Vector2.Zero, ProjectileID.GreenLaser, (int)(actualDamage * 0.33f), 0f, Player.whoAmI);
                    secondproj.tileCollide = false;
                    secondproj.friendly = true;
                    secondproj.DamageType = DamageClass.Generic;
                    secondproj.timeLeft = 300;
                    secondproj.penetrate = 1;
                    secondproj.velocity = source2.DirectionTo(target.Center) * Main.rand.NextFloat(12, 24);
                }
            }
            for (int j = 0; j < 6; j++)
            {
                int rndust = Utils.SelectRandom(Main.rand, 74, 75);
                int dusty = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, rndust, 0f, 0f, 100);
                Main.dust[dusty].alpha = 200;
                Dust dust2 = Main.dust[dusty];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[dusty];
                dust2.scale += Main.rand.NextFloat(0.1f, 0.3f);
            }
        }

        public override void AI()
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (Projectile.DistanceSQ(npc.Center) <= 5000 && !npc.friendly && npc.CanBeChasedBy())
                    npc.AddBuff(BuffID.CursedInferno, 900);
            }
            int rndust = Utils.SelectRandom(Main.rand, 74, 75);
            if (Main.rand.NextBool(20))
            {
                int dusty = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, rndust, 0f, 0f, 100);
                Main.dust[dusty].alpha = 200;
                Dust dust2 = Main.dust[dusty];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[dusty];
                dust2.scale += Main.rand.NextFloat(0.1f, 0.3f);
            }
            Lighting.AddLight(Projectile.Center, 0.8f, 0.7f, 0.4f);
            float spin = Main.rand.NextFloat(0.2f, 0.5f);
            if (Projectile.direction == -1)
                Projectile.rotation -= spin;
            else
                Projectile.rotation += spin;
            Projectile.spriteDirection = Projectile.direction;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                int rndust = Utils.SelectRandom(Main.rand, 74, 75);
                int num637 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, rndust, 0f, 0f, 100);
                if (Main.rand.NextBool(2))
                {
                    Dust dust2 = Main.dust[num637];
                    dust2.scale *= 2.5f;
                    Main.dust[num637].noGravity = true;
                    dust2 = Main.dust[num637];
                    dust2.velocity *= 5f;
                }
            }
        }
    }
}