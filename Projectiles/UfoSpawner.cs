using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class UfoSpawner : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("UfoSpawner");
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

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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

        private int GetUfoCount(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoBlue>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoRed>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoYellow>()] +
                player.ownedProjectileCounts[ModContent.ProjectileType<TinyUfoGreen>()];
        }

        public override void AI()
        {
            Projectile.CheckPlayerActiveAndNotDead(Player);
            Projectile.velocity = Vector2.Zero;
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 2000);
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= Main.rand.Next(120, 150) && Main.myPlayer == Projectile.owner && closeNPC != -1 && GetUfoCount(Player) <= Player.maxMinions)
            {
                SoundEngine.PlaySound(SoundID.Item44, Projectile.Center);
                NPC target = Main.npc[closeNPC];
                Vector2 aim = Projectile.DirectionTo(target.Center) * 2.7f;
                int thingtoShoot = Utils.SelectRandom(
                    Main.rand,
                    ModContent.ProjectileType<TinyUfoBlue>(),
                    ModContent.ProjectileType<TinyUfoRed>(),
                    ModContent.ProjectileType<TinyUfoGreen>(),
                    ModContent.ProjectileType<TinyUfoYellow>());
                int shooty = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, aim.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(0, 360))), thingtoShoot, Projectile.damage, 0, Player.whoAmI);
                Main.projectile[shooty].DamageType = DamageClass.Summon;
                Main.projectile[shooty].CritChance = 0;
                Projectile.ai[0] = 0;
            }
        }
    }
}