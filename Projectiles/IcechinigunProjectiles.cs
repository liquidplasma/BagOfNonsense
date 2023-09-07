using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class IcegunProj1 : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Ice bolt");

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.15f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < Main.rand.Next(3, 7); i++)
            {
                int dusty = Dust.NewDust(target.position, target.width, target.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 1.45f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.75f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            target.AddBuff(ModContent.BuffType<DColdtouch>(), 240);
            target.AddBuff(BuffID.Chilled, 240);
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Main.rand.NextFloat(1f) < 0.25f)
                Dust.NewDust(Projectile.position, 5, 5, DustID.IceTorch, 0f, 0f, 100, default, 1f); ;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}

namespace BagOfNonsense.Projectiles
{
    public class IcegunProj2 : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Ice bolt");

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.15f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < Main.rand.Next(3, 7); i++)
            {
                int dusty = Dust.NewDust(target.position, target.width, target.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 1.45f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.75f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            target.AddBuff(ModContent.BuffType<DColdtouch>(), 240);
            target.AddBuff(BuffID.Chilled, 240);
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Main.rand.NextFloat(1f) < 0.25f)
                Dust.NewDust(Projectile.position, 5, 5, DustID.IceTorch, 0f, 0f, 100, default, 1f);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}

namespace BagOfNonsense.Projectiles
{
    public class IcegunProj3 : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Ice bolt");

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.15f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < Main.rand.Next(3, 7); i++)
            {
                int dusty = Dust.NewDust(target.position, target.width, target.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 1.45f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.75f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            target.AddBuff(ModContent.BuffType<DColdtouch>(), 240);
            target.AddBuff(BuffID.Chilled, 240);
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Main.rand.NextFloat(1f) < 0.25f)
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 5, 5, DustID.IceTorch, 0f, 0f, 100, default, 1f);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Projectile.direction * 2, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}