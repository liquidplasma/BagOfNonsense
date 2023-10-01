using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public abstract class DoomArrowBase : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private float projSpeed;

        // public override void SetStaticDefaults() => DisplayName.SetDefault("Doom Arrow");

        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.15f;
            Projectile.alpha = 255;
        }

        public override bool PreAI()
        {
            Projectile.FaceForward();
            return base.PreAI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = damageDone / 50;
            float chance = Main.rand.NextFloat(1f);
            if (chance < 0.1f)
                Player.AddBuff(BuffID.RapidHealing, 150);
            if (heal < 1f) heal = 1;
            if (chance < 0.02f)
            {
                Player.statLife += heal;
                Player.HealEffect(heal, true);
            }
        }

        public override void AI()
        {
            Projectile.alpha -= 30;
            int aimNPC = (int)Projectile.ai[0];
            if (aimNPC != -1)
            {
                NPC target = Main.npc[aimNPC];
                if (Player.HeldItem.CountsAsClass(DamageClass.Ranged))
                {
                    projSpeed = Player.HeldItem.shootSpeed;
                    if (projSpeed < 18f)
                    {
                        projSpeed = 18f;
                    }
                }
                else
                    projSpeed = 18f;

                Vector2 aim = Projectile.DirectionTo(target.Center) * projSpeed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, aim, .1f);
                Projectile.CheckAliveNPCProj(target);
            }
        }
    }

    public class DoomArrowEX1 : DoomArrowBase
    {
        private Color Color => new(70, 218, 255, 255);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 3600);
            target.AddBuff(ModContent.BuffType<DHighwattage>(), 3600);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 16f || Projectile.velocity.Y >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y <= -16f)
                Projectile.velocity = Projectile.oldVelocity;
            if (Projectile.timeLeft == 180)
            {
                float num = 16f;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    Vector2 v = Vector2.Add(Vector2.Multiply(Vector2.UnitX, 0.0f), Vector2.Multiply(Vector2.Negate(Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)num), (Vector2.One))), new Vector2(1f, 4f))).RotatedBy((double)Projectile.velocity.ToRotation(), (Vector2.One));
                    int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Marble, 0.0f, 0.0f, 0, Color, 1f);
                    Main.dust[index2].scale = 1f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Vector2.Add(Projectile.Center, v);
                    Main.dust[index2].velocity = Vector2.Add(Vector2.Multiply(Projectile.velocity, 0.0f), Vector2.Multiply(v.SafeNormalize(Vector2.UnitY), 1f));
                }
            }
            if (Main.rand.NextBool(7))
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, 0f, 0f, 100, Color, 0.3f);
                Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[dusty].noGravity = true;
                Main.dust[dusty].fadeIn = 1.05f;
                var dust = Main.dust[dusty];
                dust.velocity *= 0.75f;
            }
            Projectile.FaceForward();
            base.AI();
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int num1 = Main.rand.Next(5, 10);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.Marble, 0.0f, 0.0f, 100, Color, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
        }
    }

    public class DoomArrowEX2 : DoomArrowBase
    {
        private Color Color => new(116, 70, 255, 255);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 3600);
            target.AddBuff(BuffID.Venom, 3600);
            target.AddBuff(BuffID.BetsysCurse, 3600);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 16f || Projectile.velocity.Y >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y <= -16f)
                Projectile.velocity = Projectile.oldVelocity;
            if (Projectile.timeLeft == 180)
            {
                float num = 16f;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    Vector2 v = Vector2.Add(Vector2.Multiply(Vector2.UnitX, 0.0f), Vector2.Multiply(Vector2.Negate(Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)num), (Vector2.One))), new Vector2(1f, 4f))).RotatedBy((double)Projectile.velocity.ToRotation(), (Vector2.One));
                    int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Marble, 0.0f, 0.0f, 0, Color.Purple, 1f);
                    Main.dust[index2].scale = 1f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Vector2.Add(Projectile.Center, v);
                    Main.dust[index2].velocity = Vector2.Add(Vector2.Multiply(Projectile.velocity, 0.0f), Vector2.Multiply(v.SafeNormalize(Vector2.UnitY), 1f));
                }
            }
            if (Main.rand.NextBool(7))
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, 0f, 0f, 100, Color, 0.3f);
                Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[dusty].noGravity = true;
                Main.dust[dusty].fadeIn = 1.05f;
                var dust = Main.dust[dusty];
                dust.velocity *= 0.75f;
            }
            Projectile.FaceForward();
            base.AI();
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int num1 = Main.rand.Next(5, 10);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.Marble, 0.0f, 0.0f, 100, Color, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
        }
    }

    public class DoomArrowEX3 : DoomArrowBase
    {
        private Color Color => new(95, 255, 0, 255);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 3600);
            target.AddBuff(BuffID.CursedInferno, 3600);
            target.AddBuff(BuffID.Frostburn, 3600);
            target.AddBuff(BuffID.ShadowFlame, 3600);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 16f || Projectile.velocity.Y >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y <= -16f)
                Projectile.velocity = Projectile.oldVelocity;
            if (Projectile.timeLeft == 180)
            {
                float num = 16f;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    Vector2 v = Vector2.Add(Vector2.Multiply(Vector2.UnitX, 0.0f), Vector2.Multiply(Vector2.Negate(Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)num), (Vector2.One))), new Vector2(1f, 4f))).RotatedBy((double)Projectile.velocity.ToRotation(), (Vector2.One));
                    int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Marble, 0.0f, 0.0f, 0, Color, 1f);
                    Main.dust[index2].scale = 1f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Vector2.Add(Projectile.Center, v);
                    Main.dust[index2].velocity = Vector2.Add(Vector2.Multiply(Projectile.velocity, 0.0f), Vector2.Multiply(v.SafeNormalize(Vector2.UnitY), 1f));
                }
            }
            if (Main.rand.NextBool(7))
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, 0f, 0f, 100, Color, 0.3f);
                Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[dusty].noGravity = true;
                Main.dust[dusty].fadeIn = 1.05f;
                var dust = Main.dust[dusty];
                dust.velocity *= 0.75f;
            }
            Projectile.FaceForward();
            base.AI();
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int num1 = Main.rand.Next(5, 10);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.Marble, 0.0f, 0.0f, 100, Color, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
        }
    }

    public class DoomArrowEX4 : DoomArrowBase
    {
        private Color Color => new(255, 70, 160, 255);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Oiled, 3600);
            target.AddBuff(BuffID.BetsysCurse, 3600);
            target.AddBuff(BuffID.Ichor, 3600);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 16f || Projectile.velocity.Y >= 16f || Projectile.velocity.X <= -16f || Projectile.velocity.Y <= -16f)
                Projectile.velocity = Projectile.oldVelocity;
            if (Projectile.timeLeft == 180)
            {
                float num = 16f;
                for (int index1 = 0; index1 < num; ++index1)
                {
                    Vector2 v = Vector2.Add(Vector2.Multiply(Vector2.UnitX, 0.0f), Vector2.Multiply(Vector2.Negate(Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)num), (Vector2.One))), new Vector2(1f, 4f))).RotatedBy((double)Projectile.velocity.ToRotation(), (Vector2.One));
                    int index2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Marble, 0.0f, 0.0f, 0, Color, 1f);
                    Main.dust[index2].scale = 1f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = Vector2.Add(Projectile.Center, v);
                    Main.dust[index2].velocity = Vector2.Add(Vector2.Multiply(Projectile.velocity, 0.0f), Vector2.Multiply(v.SafeNormalize(Vector2.UnitY), 1f));
                }
            }
            if (Main.rand.NextBool(7))
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, 0f, 0f, 100, Color, 0.3f);
                Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[dusty].noGravity = true;
                Main.dust[dusty].fadeIn = 1.05f;
                var dust = Main.dust[dusty];
                dust.velocity *= 0.75f;
            }
            Projectile.FaceForward();
            base.AI();
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int num1 = Main.rand.Next(5, 10);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.Marble, 0.0f, 0.0f, 100, Color, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
        }
    }
}