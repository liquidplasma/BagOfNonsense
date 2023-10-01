using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class GhostlyArrowProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private int timer;
        private VertexStrip _vertexStrip = new();
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Ghostly Arrow");

        private int R => Main.rand.Next(220, 255);
        private int G => Main.rand.Next(90, 125);
        private int B => Main.rand.Next(0, 25);
        private Color RandomColor => new(R, G, B);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 1000;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 480;
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.6f;
            Projectile.extraUpdates = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.GoldenTrail, ShaderStuff.GhostlyArrowStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            Projectile.alpha -= 15;
            Projectile.CritChance = 0;
            if (Projectile.timeLeft < 479)
            {
                Projectile.FaceForward();
                if (Main.rand.NextBool(7))
                {
                    int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, RandomColor, Main.rand.NextFloat(.3f, .67f));
                    Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                    Main.dust[dusty].noGravity = true;
                    Main.dust[dusty].fadeIn = 1.05f;
                    var dust = Main.dust[dusty];
                    dust.velocity *= 0.75f;
                }
            }
            if (Projectile.timeLeft < 450 && Main.npc.IndexInRange((int)Projectile.ai[0]))
            {
                timer++;
                if (timer % 60 == 0)
                {
                    Projectile.extraUpdates++;
                    Projectile.netUpdate = true;
                }
                NPC target = Main.npc[(int)Projectile.ai[0]];
                Projectile.CheckAliveNPCProj(target);
                if (!target.active || target.friendly)
                    Projectile.Kill();
                Projectile.SmoothHoming(target.Center, 0.667f, 16f); ;
            }
        }

        public override bool PreKill(int timeLeft)
        {
            int num1 = Main.rand.Next(15, 20);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.TintableDustLighted, 0.0f, 0.0f, 100, RandomColor, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
            Projectile.type = 0;
            return true;
        }
    }
}