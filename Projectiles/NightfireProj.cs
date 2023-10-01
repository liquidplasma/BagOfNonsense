using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class NightfireProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nightfire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 31;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.extraUpdates = 2;
            Projectile.scale = 0.75f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color projectileColor = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            float num138 = (TextureAssets.Projectile[Projectile.type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            for (int num432 = 1; num432 < 5; num432++)
            {
                float num433 = Projectile.velocity.X * num432;
                float num434 = Projectile.velocity.Y * num432;
                Color getAlpha = Projectile.GetAlpha(projectileColor);
                float num435 = 0f;
                if (num432 == 1)
                    num435 = 0.4f;
                if (num432 == 2)
                    num435 = 0.3f;
                if (num432 == 3)
                    num435 = 0.2f;
                if (num432 == 4)
                    num435 = 0.1f;
                getAlpha.R = (byte)(getAlpha.R * num435);
                getAlpha.G = (byte)(getAlpha.G * num435);
                getAlpha.B = (byte)(getAlpha.B * num435);
                getAlpha.A = (byte)(getAlpha.A * num435);
                Main.EntitySpriteDraw(Projectile.MyTexture(),
                    new Vector2(Projectile.position.X - Main.screenPosition.X + num138 - num433, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY - num434),
                    new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()),
                    getAlpha,
                    Projectile.rotation,
                    new Vector2(num138, Projectile.height / 2),
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Main.myPlayer];
            Vector2 source = Main.player[player.whoAmI].position + Main.player[player.whoAmI].Size * Utils.RandomVector2(Main.rand, 0f, 1f);
            Vector2 goToNPC = target.DirectionFrom(source) * Main.rand.NextFloat(26f, 40f);
            if (Main.myPlayer == Projectile.owner)
            {
                int actualDamage = (int)target.CalculateDamageNPCsTake(damageDone, target.defense);
                int rnProj = Utils.SelectRandom(Main.rand, 495, 497);
                var projy = Projectile.NewProjectileDirect(player.GetSource_FromThis(), source, goToNPC, rnProj, (int)(actualDamage * 0.3f), hit.Knockback * 0.66f, player.whoAmI);
                projy.tileCollide = false;
                projy.friendly = true;
                projy.DamageType = DamageClass.Throwing;
                projy.extraUpdates = 1;
            }
            for (int j = 0; j < 6; j++)
            {
                int rndust = Utils.SelectRandom(Main.rand, 17, 24, 171);
                int dusty = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, rndust, 0f, 0f, 100);
                Main.dust[dusty].alpha = 200;
                Dust dust2 = Main.dust[dusty];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[dusty];
                dust2.scale += Main.rand.NextFloat(0.1f, 0.3f);
            }
            target.AddBuff(BuffID.ShadowFlame, 900);
        }

        public override void AI()
        {
            int rndust = Utils.SelectRandom(Main.rand, 17, 24, 171);
            if (Main.rand.NextBool(5))
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, rndust, 0f, 0f, 100);
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
            for (int num636 = 0; num636 < 6; num636++)
            {
                int rndust = Utils.SelectRandom(Main.rand, 17, 24, 171);
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