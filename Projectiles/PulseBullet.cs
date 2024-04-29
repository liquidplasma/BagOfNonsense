using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PulseBullet : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private VertexStrip _vertexStrip = new();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4800;
        }

        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;           //Act exactly like default Bullet
            Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.alpha = 12;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            Projectile.DamageType = DamageClass.Ranged;           //Is the projectile shoot by a ranged weapon?
            Projectile.extraUpdates = 4;            //Set to above 0 if you want the projectile to update multiple time in a frame
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.height = 6;              //The height of projectile hitbox
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.tileCollide = true;          //Can the projectile collide with tiles?
            Projectile.timeLeft = 900;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.width = 10;               //The width of projectile hitbox
        }

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.PulseBulletTrail, ShaderStuff.PulseBulletStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        private void Dusts()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust hitEffect = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, HelperStats.RandomCyanDust);
                hitEffect.velocity = (-Projectile.velocity * 0.1f).RotatedByRandom(MathHelper.PiOver4);
                hitEffect.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Dusts();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3() * 0.33f);
            Projectile.FaceForward();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center, oldVelocity, Projectile.width, Projectile.height);
            Dusts();
            return base.OnTileCollide(oldVelocity);
        }
    }
}