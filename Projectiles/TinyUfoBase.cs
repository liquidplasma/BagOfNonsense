using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public abstract class TinyUfoBase : ModProjectile
    {
        public Player Player => Main.player[Projectile.owner];
        public int range = 1000 * 1000;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("TinyUfo");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 54;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = false;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.light = 0f;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override bool PreAI()
        {
            Projectile.alpha = 0;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }
            return true;
        }

        private static Vector2 GetBasePosition(Player player)
        {
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<UfoSpawner>() && proj.owner == player.whoAmI)
                {
                    pos = proj.Center + Main.rand.NextVector2CircularEdge(-300, 300);
                }
            }
            return pos;
        }

        public void Dusts(Color color)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust hitEffect = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Scorpion);
                hitEffect.velocity = Projectile.oldVelocity * 0.1f;
                hitEffect.noGravity = true;
                hitEffect.color = color;
                hitEffect.scale = 3f;
            }
        }

        public void Behavior(Color color)
        {
            Lighting.AddLight(Projectile.Center, color.ToVector3());
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1500);
            Vector2 idlePos = GetBasePosition(Player);
            if (idlePos == Vector2.Zero) Projectile.Kill();
            var dust = Dust.NewDustPerfect(idlePos, DustID.Electric, Vector2.Zero, 0, default, 2);
            dust.noGravity = true;
            Vector2 goTo = Projectile.DirectionTo(idlePos) * 12f;
            Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, goTo, 0.03f);
            if (Main.npc.IndexInRange(closeNPC))
            {
                NPC target = Main.npc[closeNPC];
                Projectile.ai[0]++;
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= 400 && Main.rand.NextBool(8))
                {
                    Projectile.ai[1] = 0;
                    SoundEngine.PlaySound(SoundID.Item61, Projectile.Center);
                    Vector2 shootAim = (Projectile.DirectionFrom(target.Center) + new Vector2(Main.rand.NextFloat(-4f, -5f), Main.rand.NextFloat(-8f, -10f))) * 1.5f;
                    Projectile.netUpdate = true;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim, ModContent.ProjectileType<UfoMissile>(), Projectile.damage * 4, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.CritChance = 0;
                    }
                }
            }
            Projectile.rotation = Projectile.velocity.X * 0.04f + Projectile.velocity.Y * 0.04f;
        }

        public override void AI()
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
    }

    public class TinyUfoBlue : TinyUfoBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/TinyUfoBlue";

        public override void AI()
        {
            Behavior(Color.Blue);
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1500);
            if (Projectile.ai[0] >= 30 && closeNPC != -1)
            {
                NPC target = Main.npc[closeNPC];
                Vector2 shootAim = Projectile.DirectionTo(target.Center) * 15f;
                float distance = Projectile.DistanceSQ(target.Center);
                if (distance < range)
                {
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim.RotatedByRandom(MathHelper.ToRadians(4)), ModContent.ProjectileType<UfoShotBlue>(), Projectile.damage, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.CritChance = 0;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dusts(Color.Blue);
        }
    }

    public class TinyUfoRed : TinyUfoBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/TinyUfoRed";

        public override void AI()
        {
            Behavior(Color.Red);
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1500);
            if (Projectile.ai[0] >= 30 && closeNPC != -1)
            {
                NPC target = Main.npc[closeNPC];
                Vector2 shootAim = Projectile.DirectionTo(target.Center) * 15f;
                float distance = Projectile.DistanceSQ(target.Center);
                if (distance < range)
                {
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim.RotatedByRandom(MathHelper.ToRadians(4)), ModContent.ProjectileType<UfoShotRed>(), Projectile.damage, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.CritChance = 0;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dusts(Color.Red);
        }
    }

    public class TinyUfoGreen : TinyUfoBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/TinyUfoGreen";

        public override void AI()
        {
            Behavior(Color.Green);
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1500);
            if (Projectile.ai[0] >= 30 && closeNPC != -1)
            {
                NPC target = Main.npc[closeNPC];
                Vector2 shootAim = Projectile.DirectionTo(target.Center) * 15f;
                float distance = Projectile.DistanceSQ(target.Center);
                if (distance < range)
                {
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim.RotatedByRandom(MathHelper.ToRadians(4)), ModContent.ProjectileType<UfoShotGreen>(), Projectile.damage, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.CritChance = 0;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dusts(Color.Green);
        }
    }

    public class TinyUfoYellow : TinyUfoBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/TinyUfoYellow";

        public override void AI()
        {
            Behavior(Color.Yellow);
            int closeNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1500);
            if (Projectile.ai[0] >= 30 && closeNPC != -1)
            {
                NPC target = Main.npc[closeNPC];
                Vector2 shootAim = Projectile.DirectionTo(target.Center) * 15f;
                float distance = Projectile.DistanceSQ(target.Center);
                if (distance < range)
                {
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim.RotatedByRandom(MathHelper.ToRadians(4)), ModContent.ProjectileType<UfoShotYellow>(), Projectile.damage, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.CritChance = 0;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dusts(Color.Yellow);
        }
    }

    public class UfoMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ufo Missile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = false;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.alpha -= 15;
            if (Projectile.alpha <= 200)
            {
                var dust = Dust.NewDustPerfect(Projectile.Center, 137, Vector2.Zero, 0, default, 1);
                dust.noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - 5.5f;
            if (Projectile.ai[0] != -1)
            {
                NPC target = Main.npc[(int)Projectile.ai[0]];
                Vector2 aim = Projectile.DirectionTo(target.Center) * 10f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, aim, 0.06f);
                if (!target.active || target.friendly)
                {
                    Projectile.ai[1] = 1;
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 launchVelocity;
            if (Projectile.ai[1] != 1)
                launchVelocity = new(4, 4);
            else
                launchVelocity = new(8, 8);

            SoundEngine.PlaySound(SoundID.Item14);
            for (int i = 0; i < 14; i++)
            {
                launchVelocity = launchVelocity.RotatedByRandom(MathHelper.ToRadians(360));
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<UfoMissileBits>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            }
        }
    }

    public class UfoMissileBits : ModProjectile
    {
        private int Behavior => (int)Projectile.ai[0];
        private int rippleCount = 3;
        private int rippleSize = 5;
        private int rippleSpeed = 5;
        private float distortStrength = 100f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ufo Missile Bits");
        }

        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = false;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            switch (Behavior)
            {
                case 2:
                    //Pulse ball shockwave
                    return false;

                default:
                    break;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnSpawn(IEntitySource source)
        {
            switch (Behavior)
            {
                case 1:
                    //Fireburn
                    Projectile.ArmorPenetration = 999;
                    Projectile.alpha = 255;
                    Projectile.CritChance = 0;
                    Projectile.netUpdate = true;
                    break;

                case 2:
                    //Pulse ball shockwave
                    Projectile.timeLeft = 90;
                    Projectile.alpha = 255;
                    Projectile.netUpdate = true;
                    break;
            }
        }

        public override void AI()
        {
            switch (Behavior)
            {
                case 0:
                    Projectile.tileCollide = true;
                    Projectile.DamageType = DamageClass.Summon;
                    var dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, Vector2.Zero, 0, default, 1);
                    dust.noGravity = true;
                    if (Projectile.timeLeft < 140)
                        Projectile.velocity.Y += 0.1875f;
                    Projectile.rotation = Projectile.velocity.X * 0.5f + Projectile.velocity.Y * 0.5f;
                    break;

                case 1:
                    //Fireburn
                    Projectile.tileCollide = false;
                    Projectile.timeLeft = 2;
                    Projectile.DamageType = DamageClass.Ranged;
                    break;

                case 2:
                    //Pulseball shockwave
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                    {
                        Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);
                    }

                    if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                    {
                        float progress = (90f - Projectile.timeLeft) / 60f; // Will range from -3 to 3, 0 being the point where the bomb explodes.
                        Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
                    }
                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Bounce(0);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            switch (Behavior)
            {
                case 0:
                    for (int i = 0; i < 12; i++)
                    {
                        Dust hitEffect = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                        hitEffect.velocity = Utils.RandomVector2(Main.rand, -2, 2);
                        hitEffect.noGravity = true;
                    }
                    break;

                case 2:
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                        Filters.Scene["Shockwave"].Deactivate();
                    break;
            }
        }
    }
}