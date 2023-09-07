using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class ElementalDaggerProj : ModProjectile
    {
        private int DustType
        {
            get
            {
                return (int)Projectile.ai[2];
            }
            set
            {
                Projectile.ai[2] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Dagger");
            Main.projFrames[Projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 6;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.light = 0.1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            DustType = Main.rand.Next(3);
            base.OnSpawn(source);
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 600);
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.Frostburn, 600);
            target.AddBuff(BuffID.CursedInferno, 600);
            target.immune[Projectile.owner] = 4;
            //vanilla code for shadowflame knife
            float num28 = Projectile.velocity.Length();
            Vector2 vector5 = target.Center - Projectile.Center;
            vector5.Normalize();
            vector5 *= num28;
            Projectile.velocity = -vector5 * 0.9f;
            Projectile.netUpdate = true;
        }

        public override bool PreAI()
        {
            Projectile.alpha = 0;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 9;
            }
            return true;
        }

        public override void AI()
        {
            HelperStats.KnifeDust(Projectile.position, Projectile, DustType);
            if (Projectile.velocity.X >= 20f || Projectile.velocity.Y >= 20f || Projectile.velocity.X <= -20f || Projectile.velocity.Y <= -20f)
                Projectile.velocity = Projectile.oldVelocity;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 30f)
            {
                if (Projectile.direction == -1)
                    Projectile.rotation -= 0.33f;
                else
                    Projectile.rotation += 0.33f;
                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y += 0.5f;
            }
            else
                Projectile.FaceForward();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X * 0.85f;
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(10, 25); i++)
                HelperStats.KnifeDust(Projectile.position, Projectile, DustType);
        }
    }
}