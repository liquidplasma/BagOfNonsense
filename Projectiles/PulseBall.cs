using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PulseBall : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private List<int> IgnoredNPCS = new();
        private ActiveSound aBallLoop;
        private VertexStrip _vertexStrip = new();

        private int BallHitTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private bool PlayHitSound
        {
            get => Projectile.ai[0] == 2;
        }

        private SoundStyle BallLoop => new("BagOfNonsense/Sounds/Weapons/AR2/PulseBall/ball_loop")
        {
            Volume = 0.2f,
            IsLooped = true,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
        };

        private SoundStyle BallHit => new("BagOfNonsense/Sounds/Weapons/AR2/PulseBall/ball_hit", 2)
        {
            Volume = 0.5f,
            MaxInstances = 0,
            Pitch = Main.rand.NextFloat(-0.25f, 0.25f),
        };

        private SoundStyle BallBounce => new("BagOfNonsense/Sounds/Weapons/AR2/PulseBall/ball_bounce", 2)
        {
            Volume = 0.25f,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        private SoundStyle BallDeath => new("BagOfNonsense/Sounds/Weapons/AR2/PulseBall/ball_death")
        {
            Volume = 0.7f
        };

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;           //Act exactly like default Bullet
            Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.alpha = 12;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            Projectile.DamageType = DamageClass.Ranged;           //Is the projectile shoot by a ranged weapon?
            Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.height = 14;              //The height of projectile hitbox
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.localNPCHitCooldown = 4;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;          //Can the projectile collide with tiles?
            Projectile.timeLeft = 240;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.usesLocalNPCImmunity = true;
            Projectile.width = 14;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Type);
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.PulseBallTrail, ShaderStuff.MagicMissileStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            int shaderID = ContentSamples.ItemsByType[Utils.SelectRandom(Main.rand, ItemID.CyanDye, ItemID.OrangeDye, ItemID.BrightCyanDye)].dye;
            Main.instance.PrepareDrawnEntityDrawing(Projectile, shaderID, null);
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), Color.White, Projectile.rotation, texture.Size() / 2, 1.5f, SpriteEffects.None);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 10f;
            modifiers.DamageVariationScale *= 0;
            modifiers.DisableCrit();
            modifiers.ArmorPenetration += target.defense * 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 oldVel = Projectile.velocity;
            BallHitTimer = 2;
            if (target.life >= 0)
                IgnoredNPCS.Add(target.whoAmI);

            int npcIndex = HelperStats.FindTargetLOSProjectile(Projectile, 4000f);
            if (Main.npc.IndexInRange(npcIndex) && !IgnoredNPCS.Contains(target.whoAmI) && !Main.rand.NextBool(8))
            {
                NPC otherNPC = Main.npc[npcIndex];
                Projectile.velocity = Projectile.Center.DirectionTo(otherNPC.Center) * oldVel.Length();
                Projectile.netUpdate = true;
            }
            else
            {
                Vector2 bounce = target.DirectionTo(Projectile.Center) * oldVel.Length();
                Projectile.velocity = bounce.RotatedByRandom(MathHelper.ToRadians(27));
                Projectile.netUpdate = true;
            }

            if (target.life <= 0)
            {
                IgnoredNPCS.Clear();
                for (int k = 0; k < Main.combatText.Length; k++)
                {
                    CombatText combatText = Main.combatText[k];
                    if (combatText != null && combatText.text == damageDone.ToString() && (CombatText.DamagedHostile == combatText.color || CombatText.DamagedHostileCrit == combatText.color))
                        combatText.active = false;
                }
                for (int i = 0; i < 60; i++)
                {
                    Vector2 velocity = Utils.NextVector2Circular(Main.rand, 5, 5);
                    Dust altFire = Dust.NewDustDirect(target.position, target.width, target.height, HelperStats.RandomCyanDust);
                    altFire.noGravity = true;
                    altFire.velocity = velocity;
                }
            }
        }

        public override bool PreAI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.1f + Projectile.velocity.Y * 0.1f;
            return base.PreAI();
        }

        public override void AI()
        {
            if (PlayHitSound)
                SoundEngine.PlaySound(BallHit, Projectile.Center);
            if (BallHitTimer > 0) BallHitTimer--;

            aBallLoop = SoundEngine.FindActiveSound(BallLoop);
            if (aBallLoop == null)
                SoundEngine.PlaySound(BallLoop, Projectile.Center);
            if (aBallLoop != null)
                aBallLoop.Position = Projectile.Center;

            Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3() * 0.33f);

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 7; i++)
            {
                Vector2 velocity = Utils.NextVector2Circular(Main.rand, 5, 5);
                Dust altFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, HelperStats.RandomCyanDust);
                altFire.velocity = velocity;
            }
            SoundEngine.PlaySound(BallBounce, Projectile.Center);
            Projectile.Bounce(8);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            IgnoredNPCS.Clear();
            aBallLoop?.Stop();
            if (Main.netMode != NetmodeID.Server)
            {
                PunchCameraModifier deathShake = new(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 8f, 18f, 40, 1000f);
                Main.instance.CameraModifiers.Add(deathShake);
            }
            for (int i = 0; i < 300; i++)
            {
                Vector2 velocity = Utils.NextVector2CircularEdge(Main.rand, 45, 30);
                Dust altFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, HelperStats.RandomCyanDust, Scale: 1.5f);
                altFire.noGravity = true;
                altFire.velocity = velocity;
                altFire.scale = 2f;
            }
            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Utils.NextVector2Circular(Main.rand, 5, 5);
                Dust altFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, HelperStats.RandomCyanDust);
                altFire.velocity = velocity;
                altFire.scale = 1.5f;
            }
            if (Player.whoAmI == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UfoMissileBits>(), 0, 0, Player.whoAmI, ai0: 2);

            SoundEngine.PlaySound(BallDeath, Projectile.Center);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
        }
    }
}