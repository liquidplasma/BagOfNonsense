using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public abstract class UfoShotBase : ModProjectile
    {
        public Player Player => Main.player[Projectile.owner];
        public VertexStrip _vertexStrip = new();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 500;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = false;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }

        public void DeathDusts(Color color)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust hitEffect = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
                hitEffect.velocity = Projectile.oldVelocity * 0.1f + Utils.RandomVector2(Main.rand, -6, 6);
                hitEffect.noGravity = true;
                hitEffect.color = color;
            }
        }

        public void ReflectTowardsNearbyNPC(NPC target)
        {
            int foundNPC = HelperStats.FindNextNPC(Projectile, target, 1000);
            if (Main.npc.IndexInRange(foundNPC) && Main.rand.NextBool())
            {
                NPC targetNext = Main.npc[foundNPC];
                Vector2 aim = Projectile.DirectionTo(targetNext.Center) * 15f;
                var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, aim, Type, Projectile.damage, Projectile.knockBack, Player.whoAmI, target.whoAmI);
                shooty.CritChance = 0;
            }
            else Projectile.Kill();
        }

        public void Behavior(Color color)
        {
            Lighting.AddLight(Projectile.Center, color.ToVector3());
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Main.rand.NextBool(2))
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC target = Main.npc[i];
                    if (target.active && !target.friendly && target.CanBeChasedBy() && !target.CountsAsACritter && Collision.CanHit(Projectile.Center, 1, 1, target.position, 1, 1))
                    {
                        Projectile.velocity = Projectile.DirectionTo(target.Center) * 15f;
                    }
                    else Projectile.Kill();
                }
            }
            else Projectile.Kill();
            return false;
        }
    }

    public class UfoShotBlue : UfoShotBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/UfoShotBase";

        public override void SetDefaults()
        {
            Projectile.ArmorPenetration = 9999;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.BlueTrail, ShaderStuff.UfoShotStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ReflectTowardsNearbyNPC(target);
        }

        public override void AI()
        {
            Behavior(Color.Blue);
        }

        public override void OnKill(int timeLeft)
        {
            DeathDusts(Color.Blue);
        }
    }

    public class UfoShotYellow : UfoShotBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/UfoShotBase";

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.YellowTrail, ShaderStuff.UfoShotStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ReflectTowardsNearbyNPC(target);
        }

        public override void AI()
        {
            Behavior(Color.Yellow);
        }

        public override void OnKill(int timeLeft)
        {
            DeathDusts(Color.Yellow);
        }
    }

    public class UfoShotGreen : UfoShotBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/UfoShotBase";

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.GreenTrail, ShaderStuff.UfoShotStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ReflectTowardsNearbyNPC(target);
        }

        public override void AI()
        {
            Behavior(Color.Green);
        }

        public override void OnKill(int timeLeft)
        {
            DeathDusts(Color.Green);
        }
    }

    public class UfoShotRed : UfoShotBase
    {
        public override string Texture => "BagOfNonsense/Projectiles/UfoShotBase";

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.UfoShotRedTrail, ShaderStuff.UfoShotStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ReflectTowardsNearbyNPC(target);
        }

        public override void AI()
        {
            Behavior(Color.Red);
        }

        public override void OnKill(int timeLeft)
        {
            DeathDusts(Color.Red);
        }
    }
}