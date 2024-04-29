using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class RoboMonkey : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 106;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 390;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.light = 1f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void AI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            Projectile.velocity = Vector2.Zero;
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<RoboMonkey>()] > 1)
            {
                Projectile.Kill();
            }
            int aimTarget = HelperStats.FindTargetLOSProjectile(Projectile, 1000);
            if (aimTarget != -1)
            {
                NPC target = Main.npc[aimTarget];
                Projectile.rotation = Projectile.AngleTo(target.Center) + MathHelper.PiOver2;
                Projectile.ai[0] += 1f;
                if (target.active && Main.myPlayer == Projectile.owner && Projectile.ai[0] >= 6)
                {
                    Projectile.ai[0] = 0;
                    Projectile aimLeft = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, 32), Vector2.Zero, ModContent.ProjectileType<PlasmaBall>(), (int)(Projectile.damage * 1.2f), 1f, Player.whoAmI);
                    aimLeft.velocity = aimLeft.DirectionTo(target.Center) * 4f;
                    Projectile aimRight = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 32), Vector2.Zero, ModContent.ProjectileType<PlasmaBall>(), (int)(Projectile.damage * 1.2f), 1f, Player.whoAmI);
                    aimRight.velocity = aimRight.DirectionTo(target.Center) * 4f;
                }
            }
        }
    }
}