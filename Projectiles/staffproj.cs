using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class staffproj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Staff of Extreme Prejudice bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 13;
            Projectile.height = 13;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
            Projectile.damage = 0;
            Projectile.light = 0.1f;
            Projectile.noDropItem = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var owner = Main.player[Projectile.owner];
            float chance = Main.rand.NextFloat(1f);
            if (chance < 0.3f)
            {
                owner.statLife += 20;
                owner.HealEffect(10, true);
            }
            else if (chance < 0.2f)
            {
                owner.statLife += 30;
                owner.HealEffect(20, true);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Color color = new(255, 0, 0, 255);
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 182, Projectile.direction * 2, 0.0f, 150, color, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 0.8f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = true;
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        }

        public override void AI()
        {
            Projectile.FaceForward();
        }
    }
}