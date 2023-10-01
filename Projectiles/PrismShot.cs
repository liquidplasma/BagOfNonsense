using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PrismShot : ModProjectile
    {
        private int TypeDust = Utils.SelectRandom(Main.rand, 57, 58);
        public override string Texture => "BagOfNonsense/Projectiles/PrismShot1";

        // public override void SetStaticDefaults() => DisplayName.SetDefault("PrismShot");

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.light = 0.15f;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float spin = 0.33f;
            Projectile.ai[0] += 1f;
            if (Projectile.direction == -1)
                Projectile.rotation -= spin;
            else
                Projectile.rotation += spin;
            if (Main.rand.NextBool(20))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
            int closestNPC = FindTarget();
            if (closestNPC == -1) Projectile.Kill();
        }

        public int FindTarget(float maxRange = 600f)
        {
            float num = maxRange;
            int result = -1;
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                bool flag = nPC.CanBeChasedBy();
                if (Projectile.localNPCImmunity[i] != 0)
                {
                    flag = false;
                }
                if (flag)
                {
                    Player player = Main.player[Main.myPlayer];
                    float num2 = player.Distance(Main.npc[i].Center);
                    if (num2 < num)
                    {
                        num = MathHelper.Min(num2, 800f);
                        result = i;
                    }
                }
            }
            return result;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
            }
        }
    }

    public class PrismShot1 : PrismShot
    {
        public override string Texture => "BagOfNonsense/Projectiles/PrismShot1";

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(4, 8));
        }
    }

    public class PrismShot2 : PrismShot
    {
        public override string Texture => "BagOfNonsense/Projectiles/PrismShot2";

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Oiled, 60 * Main.rand.Next(4, 8));
        }
    }

    public class PrismShot3 : PrismShot
    {
        public override string Texture => "BagOfNonsense/Projectiles/PrismShot3";

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 60 * Main.rand.Next(4, 8));
        }
    }
}