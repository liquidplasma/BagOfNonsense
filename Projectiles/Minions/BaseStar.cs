using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    public abstract class BaseStar : ModProjectile
    {
        public Player Player => Main.player[Projectile.owner];
        public VertexStrip _vertexStrip = new();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 1000;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 240;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 15;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public void Behavior()
        {
            int closestNPC = (int)Projectile.ai[0];
            Projectile.RotateBasedOnVelocity();
            if (Main.npc.IndexInRange(closestNPC))
            {
                Projectile.netUpdate = true;
                NPC target = Main.npc[closestNPC];
                Projectile.CheckAliveNPCProj(target);
                if (target.dontTakeDamage)
                    Projectile.Kill();
                Projectile.SmoothHoming(target.Center, 1f, 16f);
            }
            if (closestNPC == -1) Projectile.Kill();
        }
    }

    public class PrismStarRed : BaseStar
    {
        private static int TypeDust => Utils.SelectRandom(Main.rand, 60, 114, 120, 266);

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("BagOfNonsense/Projectiles/Minions/PrismStarRed_Glow", AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                texture.Size() * 0.5f,
                1f,
                SpriteEffects.None,
                0
            );

            if (Projectile.ai[1] >= 90)
            {
                MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
                miscShaderData.UseSaturation(-2.8f);
                miscShaderData.UseOpacity(4f);
                miscShaderData.Apply();
                _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.UfoShotRedTrail, ShaderStuff.GhostlyArrowStripWidth, -Main.screenPosition + Projectile.Size / 2f);
                _vertexStrip.DrawTrail();
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] <= 90)
            {
                Projectile.rotation += 0.04f * Player.direction;
            }
            else
            {
                Behavior();
            }
            if (Projectile.timeLeft == 240)
            {
                for (int i = 0; i < 7; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                }
            }

            if (Main.rand.NextBool(48))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
            }
        }
    }

    public class PrismStarBlue : BaseStar
    {
        private static int TypeDust => Utils.SelectRandom(Main.rand, 59, 96, 111, 176, 187);

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("BagOfNonsense/Projectiles/Minions/PrismStarBlue_Glow", AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                texture.Size() * 0.5f,
                1f,
                SpriteEffects.None,
                0
            );

            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.BlueTrail, ShaderStuff.GhostlyArrowStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] <= 120)
            {
                Projectile.rotation += (Projectile.velocity.X * 0.04f) + (Projectile.velocity.Y * 0.04f);
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, Utils.RandomVector2(Main.rand, -6f, 6f), 0.01f);
            }
            else
            {
                Behavior();
            }
            if (Projectile.timeLeft == 240)
            {
                for (int i = 0; i < 7; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                }
            }
            if (Main.rand.NextBool(48))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
            }
        }
    }
}