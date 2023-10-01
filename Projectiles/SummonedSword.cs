using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class SummonedSword : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Summoned Sword");

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ViciousPowder, Projectile.direction * Main.rand.NextFloat(1f, 3f), 0.0f, 150, Color.DarkCyan, 1f);
                var dust = Main.dust[dusty];
                Vector2 dustspeed = Vector2.Multiply(dust.velocity, 0.2f);
                dust.velocity = dustspeed;
                Main.dust[dusty].noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.alpha -= 15;
            Projectile.FaceForward();
            Projectile.velocity.X *= 1.2f;
            Projectile.velocity.Y *= 1.2f;
            if (Projectile.timeLeft == 120)
                SoundEngine.PlaySound(new SoundStyle("BagOfNonsense/Sounds/Custom/summonedsword"), Projectile.Center);
            if (Projectile.velocity.X >= 20f || Projectile.velocity.Y >= 20f || Projectile.velocity.X <= -20f || Projectile.velocity.Y <= -20f)
                Projectile.velocity = Projectile.oldVelocity;

            Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3());
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("BagOfNonsense/Sounds/Custom/summonedbreak"), Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.ViciousPowder, Projectile.direction * Main.rand.NextFloat(1f, 3f), 0.0f, 150, Color.DarkCyan, 1f);
                var dust = Main.dust[dusty];
                Vector2 dustspeed = Vector2.Multiply(dust.velocity, 0.2f);
                dust.velocity = dustspeed;
                Main.dust[dusty].noGravity = true;
            }
        }
    }
}