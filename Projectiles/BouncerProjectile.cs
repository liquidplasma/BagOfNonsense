using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class BouncerProjectile : ModProjectile
    {
        public override string Texture => "BagOfNonsense/Items/Weapons/Ranged/Bouncer";
        private VertexStrip _vertexStrip = new();
        Player Player => Main.player[Projectile.owner];
        private bool HitOnce = false;
        private SoundStyle Tick => new("BagOfNonsense/Sounds/Weapons/Bouncer/tick1")
        {
            Volume = 0.33f,
        };

        private int LightTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        private int SoundTimer
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        private int Timer
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        private bool NotExploding
        {
            get => Timer <= 444;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 300;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.width = 7;
            Projectile.height = 7;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 450;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 1;
            Projectile.light = 0f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (NotExploding)
            {
                MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
                miscShaderData.UseSaturation(-2.8f);
                miscShaderData.UseOpacity(4f);
                miscShaderData.Apply();
                _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.UfoShotRedTrail, ShaderStuff.GhostlyArrowStripWidth, -Main.screenPosition + Projectile.Size / 2f);
                _vertexStrip.DrawTrail();
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }
            return base.PreDraw(ref lightColor);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {         
            if(HitOnce)
                return false;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (NotExploding)
            {
                modifiers.FinalDamage *= 0.667f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (NotExploding)
            {
                HitOnce = true;
                Vector2 oldVel = Projectile.velocity;
                Projectile.velocity = target.Center.DirectionTo(Projectile.Center) * oldVel.Length() * 0.5f;
                Projectile.netUpdate = true;
            }
        }

        public override void AI()
        {
            if (LightTimer > 0) 
            {
                LightTimer--;
                Lighting.AddLight(Projectile.Center, Color.Red.ToVector3());
            }
            Projectile.rotation += (Projectile.velocity.X * 0.04f) + (Projectile.velocity.Y * 0.04f);
            Timer++;
            SoundTimer++;
            if (NotExploding)
                Projectile.velocity.Y += 0.097f;
            int blipCooldown = (int)(90 - Timer / 50f * 7);
            if (SoundTimer >= blipCooldown)
            {
                LightTimer = 6;
                SoundTimer = 0;
                SoundEngine.PlaySound(Tick, Projectile.Center);
            }
            if (!NotExploding)
            {
                HitOnce = false;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                Projectile.alpha = 255;
                Projectile.Resize(512, 512);
                Projectile.netUpdate = true;
            }
            else
                Projectile.alpha -= 33;
            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 32; i++)
            {
                var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, default, HelperStats.GrenadeGore);
                gore.velocity = Utils.RandomVector2(Main.rand, -1f, 1f) * Main.rand.NextFloat(8, 14);
            }
            for(int i = 0;i < 32; i++)
            {
                Dust deathEffect = Dust.NewDustDirect(Projectile.Center, 6, 6, HelperStats.SmokeyDust);
                deathEffect.velocity = Utils.RandomVector2(Main.rand, -5, 5);
                deathEffect.scale = 1f + Main.rand.NextFloat();
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (Player.DistanceSQ(Projectile.Center) <= 128 * 128 && Collision.CanHitLine(Projectile.Center, 1, 1, Player.Center, 1, 1))
            {
                Player.HurtInfo greandeSelfDamage = new()
                {
                    Dodgeable = true,
                    HitDirection = Player.direction,
                    Damage = (int)(Projectile.damage * 0.25f),
                    DamageSource = PlayerDeathReason.ByProjectile(Player.whoAmI, Projectile.identity),
                    Knockback = 6f
                };
                Player.Hurt(greandeSelfDamage);

            }
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Bounce(2);
            Projectile.velocity.X *= 0.7f;
            Projectile.velocity.Y *= 0.6f;
            return false;
        }
    }
}