using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class DoomArrowProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private VertexStrip _vertexStrip = new();
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Doom Arrow");
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 1000;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 480;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.15f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
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
            if (Projectile.velocity.Length() >= 16f)
                Projectile.velocity = Projectile.oldVelocity;

            Projectile.FaceForward();
            if (Main.rand.NextBool(3))
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, 0f, 0f, 100, default, 0.3f);
                Main.dust[dusty].scale = Main.rand.Next(1, 10) * 0.1f;
                Main.dust[dusty].noGravity = true;
                Main.dust[dusty].fadeIn = 1.1f;
                var dust = Main.dust[dusty];
                dust.velocity *= 0.75f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = damageDone / 30;
            float chance = Main.rand.NextFloat(1f);
            if (heal <= 1f) heal = 1;
            if (chance <= 0.02f)
                Player.AddBuff(BuffID.RapidHealing, 150);
            if (chance <= 0.01f)
            {
                Player.statLife += heal;
                Player.HealEffect(heal, true);
            }
            int nerfdamage = (int)(Player.GetWeaponDamage(Player.HeldItem) * 0.5f);
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 4; i++)
                {
                    int chooserandomDoom = Utils.SelectRandom(Main.rand, ModContent.ProjectileType<DoomArrowEX1>(), ModContent.ProjectileType<DoomArrowEX2>(), ModContent.ProjectileType<DoomArrowEX3>(), ModContent.ProjectileType<DoomArrowEX4>());
                    Vector2 source = Player.position + Player.Size * Utils.NextVector2Circular(Main.rand, -8, 2f);
                    Vector2 direction = target.DirectionFrom(source) * 18f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), source, direction, chooserandomDoom, nerfdamage, 0.0f, Player.whoAmI, target.whoAmI, 0.0f);
                }
            }
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = velocityChange.X / .975f;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = velocityChange.Y / .975f;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Color color = new(255, 249, 70, 255);
            int num1 = Main.rand.Next(15, 20);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.position, 0, 0, DustID.TintableDustLighted, 0.0f, 0.0f, 100, color, 1f);
                var dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 1.6f);
                var dust2 = Main.dust[index2];
                dust2.position = Vector2.Subtract(dust2.position, Vector2.Multiply(Vector2.One, 4f));
                Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, Projectile.Center, 0.5f);
                Main.dust[index2].noGravity = true;
            }
        }
    }
}