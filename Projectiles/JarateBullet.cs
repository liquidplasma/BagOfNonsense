using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class JarateBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jarate Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CursedBullet);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.extraUpdates = 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DJarate>(), 1800);
            base.OnHitNPC(target, hit, damageDone);
        }

        public void JarateHitPlayerSFX(Player target)
        {
            SoundStyle jarateplayer = new("BagOfNonsense/Sounds/Other/spy_jaratehit", stackalloc (int, float)[] { (2, 1f), (3, 1f) });
            SoundEngine.PlaySound(jarateplayer with
            {
                Volume = Main.rand.NextFloat(0.6f, 0.7f)
            }, target.position);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(250))
            {
                JarateHitPlayerSFX(target);
                int critText = CombatText.NewText(target.getRect(), Color.Yellow, "IS THIS....? NOOOOO", true);
                if (critText < 100)
                {
                    Main.combatText[critText].lifeTime = 120;
                    Main.combatText[critText].velocity.Y -= 16f;
                }
            }
            target.AddBuff(ModContent.BuffType<DJarate>(), 1800);
            Projectile.Kill();
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 16f || Projectile.velocity.Y >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y <= -16f)
                Projectile.velocity = Projectile.oldVelocity;
            if (Main.rand.NextBool(12))
                Dust.NewDustPerfect(Projectile.Center, DustID.YellowTorch, null, 0, default, 1f);
            Projectile.FaceForward();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
            }
            base.OnKill(timeLeft);
        }
    }
}