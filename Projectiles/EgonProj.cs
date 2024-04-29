using BagOfNonsense.Dusts;
using BagOfNonsense.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    // This file showcases a projectile moving in a sine wave
    // Can be tested with ExampleMagicRod
    public class EgonProj : ModProjectile
    {
        // This field is used in this projectie's custom AI
        public Vector2 initialCenter;

        private Player Player => Main.player[Projectile.owner];

        // This field is used as a counter for the wave motion
        public int sineTimer;

        public int waveDirection = 1;
        private float waveAmplitude = 0;
        private Color MainBeam => new(47, 193, 203);

        private static SoundStyle HitNoise => new("BagOfNonsense/Sounds/Weapons/egon_hit", 5)
        {
            MaxInstances = 1,
        };

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 2000;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        // Setting the default parameters of the projectile
        // You can check most of Fields and Properties here https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
        public override void SetDefaults()
        {
            Projectile.width = 9; // The width of projectile hitbox
            Projectile.height = 9; // The height of projectile hitbox
            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Magic; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 120; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.extraUpdates = 30;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[0] != 1)
            {
                modifiers.FinalDamage *= 0.57f;
                modifiers.Knockback *= 0;
            }
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dusty = Dust.NewDustDirect(target.position, target.width, target.height, ModContent.DustType<EgonLaser>());
                dusty.velocity = Vector2.Zero;
                dusty.color = Utils.SelectRandom(Main.rand, Color.Cyan, MainBeam);
                dusty.scale = Main.rand.NextFloat(1f, 2f);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        // This projectile updates its position manually
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            initialCenter = Projectile.Center;
        }

        public override void AI()
        {
            // Ensure that the "waveDirection = 1" projectile starts above the "waveDirection = -1" projectile not matter which direction they were fired from
            int trueDirection = Projectile.direction * waveDirection;

            // How many oscillations happen per second
            // Higher value = more oscillations
            float wavesPerSecond = 2;

            // Math.Sin expects a radians angle instead of degrees
            float radians = MathHelper.ToRadians(sineTimer * 6f * wavesPerSecond) - (Player.GetModPlayer<EgonModPLayer>().radiansControl * (Projectile.ai[0] == 2 ? -1.5f : 1));
            float sine = (float)Math.Sin(radians) * trueDirection;

            // Using the calculated sine value, generate an offset used to position the projectile on the wave
            // The offset should be perpendicular to the velocity direction, hence the RotatedBy call
            Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2 * -1);

            // How wide the wave should be, times two
            // An amplitude of 24 pixels is 1.5 tiles, meaning the total wave width is 48 pixels, or 3 tiles
            waveAmplitude++;
            if (waveAmplitude >= 18) waveAmplitude = 18;

            if (Projectile.ai[0] == 1)
                waveAmplitude = 0;
            // Get the offset used to adjust the projectile's position
            offset *= sine * waveAmplitude;
            // Update the position manually since ShouldUpdatePosition returns false
            initialCenter += Projectile.velocity;
            Projectile.Center = initialCenter + offset;

            // Update the rotation used to draw the projectile
            // This projectile should act as if it were moving along the sine wave
            float cosine = (float)Math.Cos(radians) * trueDirection;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 * cosine * -1;

            // Spawn dusts
            if (Projectile.ai[0] == 1)
            {
                Dust dusty = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<EgonLaser>());
                dusty.velocity = Vector2.Zero;
                dusty.scale = 1f;
                dusty.color = MainBeam;
            }
            else
            {
                Dust dusty2 = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<EgonLaser>());
                dusty2.velocity = Vector2.Zero;
                dusty2.scale = 1f;
                dusty2.color = Color.Cyan;
            }

            Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();

            sineTimer++;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                Dust deathDust = Dust.NewDustDirect(Projectile.position + new Vector2(0, -4), 0, 0, ModContent.DustType<EgonLaser>());
                deathDust.scale = 5f;
                deathDust.color = MainBeam;
            }
            else
            {
                Dust deathDust = Dust.NewDustDirect(Projectile.position + new Vector2(0, -4), 0, 0, ModContent.DustType<EgonLaser>());
                deathDust.scale = 5f;
                deathDust.color = Color.Cyan;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(initialCenter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            initialCenter = reader.ReadVector2();
        }
    }
}