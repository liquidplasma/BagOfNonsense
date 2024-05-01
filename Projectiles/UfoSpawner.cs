using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class UfoSpawner : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private int extraRadius;

        private static Geometry GeometryObject = new();

        private int SpawnTimer
        {
            get
            {
                return (int)Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 80;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.light = 1f;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreAI()
        {
            Projectile.alpha = 0;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }
            return true;
        }

        public static int GetUfoCount(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoBlue>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoRed>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoYellow>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoGreen>()];
        }

        public override void AI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            Projectile.velocity = Vector2.Zero;
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 2000);
            SpawnTimer++;
            if (SpawnTimer >= Main.rand.Next(120, 150) && Main.npc.IndexInRange(closeNPC) && GetUfoCount(Player) <= Player.maxMinions)
            {
                SpawnTimer = 0;
                extraRadius = 120;
                int UfoToShoot = Utils.SelectRandom(
                    Main.rand,
                    ModContent.ProjectileType<TinyUfoBlue>(),
                    ModContent.ProjectileType<TinyUfoRed>(),
                    ModContent.ProjectileType<TinyUfoGreen>(),
                    ModContent.ProjectileType<TinyUfoYellow>());
                SoundEngine.PlaySound(SoundID.Item44, Projectile.Center);
                NPC target = Main.npc[closeNPC];
                Vector2 aim = Projectile.DirectionTo(target.Center) * 2.7f;
                Projectile shooty = ExtensionMethods.BetterNewProjectile(Player, Projectile.GetSource_FromThis(), Projectile.Center, aim.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(0, 360))), UfoToShoot, Projectile.damage, 0, Player.whoAmI);
                shooty.CritChance = 0;
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex).noGravity = true;
                }
            }

            if (Main.npc.IndexInRange(closeNPC))
            {
                List<Dust> dusty = GeometryObject.DrawDustCircle(Projectile, 120, 300f, DustID.Electric);
                foreach (Dust dust in dusty)
                    dust.noGravity = true;
            }
        }
    }
}