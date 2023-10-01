using BagOfNonsense.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class ScytheProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        // public override void SetStaticDefaults() => DisplayName.SetDefault("Cold Touch");

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.alpha = 100;
            Projectile.light = 0.5f;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 360;
            Projectile.coldDamage = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 900);
            target.AddBuff(BuffID.Frostburn2, 900);
            target.AddBuff(ModContent.BuffType<DColdtouch>(), 900);
            target.immune[Player.whoAmI] = 6;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(4))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 255, default, 1f);
                    dust.noGravity = false;
                    Dust dusty = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 255, default, 1f);
                    dusty.velocity = Projectile.oldVelocity * 0.1f;
                }
            }
            Projectile.spriteDirection = -1;
            Projectile.rotation = Projectile.rotation + Projectile.direction * 0.05f;
            Projectile.rotation = Projectile.rotation + (float)(Projectile.direction * 0.5 * (Projectile.timeLeft / 180.0));
            Projectile.velocity = Projectile.velocity * 0.96f;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 31; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 1.2f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}