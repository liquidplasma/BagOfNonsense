using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Accessory;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class ProjectileReflector : ModProjectile
    {
        public override string Texture => "BagOfNonsense/Projectiles/ProjectileReflector";
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Projectile Deflector");
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
            Projectile.light = 1f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Dust dusty = Dust.NewDustPerfect(Projectile.Center + Utils.RandomVector2(Main.rand, -(Projectile.width / 2f), Projectile.width / 2f), DustID.PurpleTorch, Vector2.Zero);
            dusty.noGravity = true;

            Dust dusty2 = Dust.NewDustDirect(Projectile.Center + Utils.RandomVector2(Main.rand, -(Projectile.width / 2f), Projectile.width / 2f), 0, 0, DustID.PurpleTorch, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
            dusty2.noGravity = true;

            if (Projectile.Center.DistanceSQ(Player.Center) >= 1400 * 1400)
                Projectile.TeleportToOrigin(Player, Player.Center, DustID.PurpleTorch);

            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3());
            Projectile.CheckPlayerActiveAndNotDead(Player);
            int hostileProjIndex = HelperStats.FindNearestHostileProj(112, Player);
            if (Main.projectile.IndexInRange(hostileProjIndex))
            {
                Projectile hostileProj = Main.projectile[hostileProjIndex];
                if (hostileProj.type != ProjectileID.SaucerDeathray
                && hostileProj.type != ProjectileID.PhantasmalDeathray)
                {
                    Vector2 offset = Projectile.Center.DirectionTo(hostileProj.Center) * 16f;
                    HelperStats.DustLine(Projectile.Center + offset, hostileProj.Center, 8, Color.White, DustID.GoldFlame);
                    SoundEngine.PlaySound(SoundID.Item35 with
                    {
                        PitchVariance = 1,
                    }, hostileProj.Center);
                    Projectile.frameCounter++;
                    Projectile.frame = (Projectile.frameCounter + 1) % 3;
                    hostileProj.velocity = hostileProj.Center.DirectionFrom(Player.Center) * 14f;
                    hostileProj.friendly = true;
                    hostileProj.hostile = false;
                    hostileProj.netUpdate = true;
                    Projectile.rotation = Projectile.AngleTo(hostileProj.Center) + MathHelper.Pi;
                    Projectile.ai[1] = 0;
                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 dustVel = hostileProj.velocity.RotatedByRandom(MathHelper.ToRadians(180)) * 0.1f;
                        Dust dust = Dust.NewDustDirect(hostileProj.Center, (int)hostileProj.Size.X, (int)hostileProj.Size.Y, DustID.GoldFlame, dustVel.X, dustVel.Y, 0, Color.White, 1.5f);
                        dust.noGravity = true;
                        Dust dust2 = Dust.NewDustDirect(Projectile.Center + offset, 8, 8, DustID.GoldFlame, dustVel.X, dustVel.Y, 0, Color.Gold, 1.5f);
                        dust2.noGravity = true;
                    }
                }
            }
            else
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= 180)
                    Projectile.rotation = Projectile.AngleTo(Player.Center) + MathHelper.Pi;
            }
            Vector2 placeToGo = Player.Center;
            if (Player.active)
            {
                Vector2 pos = placeToGo + new Vector2(0, -64);
                Projectile.Center = pos;
            }
            if (!Player.GetModPlayer<ProjectileDeflectorAccessoryModPlayer>().isEquipped) Projectile.Kill();
        }
    }
}