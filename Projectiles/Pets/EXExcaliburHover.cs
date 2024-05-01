using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Pets
{
    public class EXExcaliburControl : ModPlayer
    {
        public bool EXEAllow;

        private int canShoot;

        private int aimTarget;

        public override void ResetEffects()
        {
            EXEAllow = false;
            if (canShoot > 0)
            {
                canShoot--;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI == proj.owner
               && !HelperStats.IsAStarSummon(proj)
               && damageDone > 0
               && target.active
               && !target.friendly
               && EXEAllow == true
               && proj.type == ModContent.ProjectileType<LightBeamModded>())
            {
                canShoot = 6;
                aimTarget = target.whoAmI;
            }
        }

        public bool GetAllowed()
        {
            if (canShoot > 0)
            {
                return true;
            }
            else return false;
        }

        public int GetTarget()
        {
            if (aimTarget != -1)
            {
                return aimTarget;
            }
            return -1;
        }
    }

    public class EXExcaliburHover : ModProjectile
    {
        public override string Texture => "BagOfNonsense/Items/Weapons/Magic/EXExcalibur";
        private Player Player => Main.player[Projectile.owner];
        private Projectile syncProj;

        private Vector2 idlePos;

        private float mov;

        private bool increase;

        private NPC target;

        private static int DustType => Utils.SelectRandom(Main.rand, 27, 6, 15);
        private Color color = Color.AliceBlue;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("EXExcalibur");
        }

        public override void SetDefaults()
        {
            Projectile.width = 91;
            Projectile.height = 91;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int shaderID = ContentSamples.ItemsByType[ItemID.MirageDye].dye;
            Main.instance.PrepareDrawnEntityDrawing(Projectile, shaderID, null);
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = new(0, 0, Projectile.width, Projectile.height);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 60)
            {
                Projectile.ai[0] = 0;
                switch (Main.rand.Next(0, 3))
                {
                    case 0:
                        color = Color.Purple;
                        break;

                    case 1:
                        color = Color.Orange;
                        break;

                    case 2:
                        color = Color.AliceBlue;
                        break;
                }
            }
            Vector2 randomVector = Utils.RandomVector2(Main.rand, -4f, 4f);
            var dust = Dust.NewDustDirect(Projectile.position - new Vector2(2f, 2f), (int)Projectile.Size.X, (int)Projectile.Size.Y, DustType, randomVector.X, randomVector.Y, 100, default, 1f);
            dust.velocity *= 1.2f + Main.rand.NextFloat();
            dust.noLight = true;
            Lighting.AddLight(Projectile.Center, color.ToVector3());
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            if (Player.HeldItem.type != ModContent.ItemType<EXExcalibur>()) Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 * 1.5f;
            if (Player.direction == 1)
                idlePos = new Vector2(-64, -32);
            else
                idlePos = new Vector2(-24, -32);

            if (increase)
            {
                if (mov < 16)
                    mov += 0.333333333333333333333333333f;
                else
                    increase = false;
            }
            else
            {
                if (mov > -32)
                    mov -= 0.333333333333333333333333333f;
                else
                    increase = true;
            }
            Projectile.position = Player.Center + idlePos + new Vector2(0, mov);

            Projectile.ai[1]++;
            EXExcaliburControl AA = Player.GetModPlayer<EXExcaliburControl>();
            AA.EXEAllow = true;
            if (Main.npc.IndexInRange(AA.GetTarget()))
            {
                target = Main.npc[AA.GetTarget()];
            }
            int attackTime = Player.HeldItem.useTime + Main.rand.Next(1, 5);
            if (AA.GetAllowed() && target.active && !target.friendly && Projectile.ai[1] > attackTime)
            {
                SoundStyle fire = new("BagOfNonsense/Sounds/Weapons/exexcalibur", 3);
                SoundEngine.PlaySound(fire with
                {
                    Pitch = Main.rand.NextFloat(0, .55f),
                    Volume = .2f
                }, Projectile.Center);
                Projectile.ai[1] = 0;
                float actualDamage = (int)(Player.HeldItem.damage * Player.GetPlayerDamageMultiplier(DamageClass.Magic) * 0.9f);
                Vector2 pos = Projectile.Center + new Vector2(0, Main.rand.Next(-64, 64));
                Vector2 aim = (pos.DirectionTo(randomVector) * Player.HeldItem.shootSpeed).RotatedByRandom(MathHelper.ToRadians(360));
                if (Player.whoAmI == Main.myPlayer)
                {
                    var proj = Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), pos, aim, ModContent.ProjectileType<LightBeamModded>(), (int)actualDamage, 1f, Player.whoAmI, AA.GetTarget(), 2);
                }
            }
        }
    }

    public class LightBeamModded : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private int timer;

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.LightBeam;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("LightBeam");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LightBeam);
            DrawOffsetX = -6;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Player.HeldItem.type != ModContent.ItemType<EXExcalibur>()) Projectile.Kill();

            if (Projectile.ai[1] == 1) // for Excalibur magic weapon
            {
                Projectile.alpha -= 15;
                Projectile.DamageType = DamageClass.Magic;
                Projectile.extraUpdates = 1;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
                if (Projectile.timeLeft == 300)
                {
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 randomVector = Utils.RandomVector2(Main.rand, -4f, 4f);
                        var dust = Dust.NewDustDirect(Projectile.position - new Vector2(2f, 2f), 1, 1, DustID.PinkFairy, randomVector.X, randomVector.Y, 255, default, 1f);
                        dust.velocity *= 1.6f;
                        dust.noGravity = true;
                        var otherDust = Dust.NewDustDirect(Projectile.position - new Vector2(2f, 2f), 1, 1, DustID.PinkFairy, randomVector.X, randomVector.Y, 255, default, 1f);
                        otherDust.velocity *= 0.8f;
                        otherDust.noGravity = true;
                    }
                }
                var dusty = Dust.NewDustDirect(Projectile.position + new Vector2(2f, 2f), (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.PinkFairy, 0, 0, 255, default, (Projectile.timeLeft / 300f));
                dusty.velocity = Projectile.oldVelocity * 0.1f;
            }
            if (Projectile.ai[1] == 2) // for Excalibur magic weapon
            {
                Projectile.alpha -= 15;
                Projectile.DamageType = DamageClass.Magic;
                Projectile.extraUpdates = 1;
                Projectile.penetrate = 1;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
                int npc = (int)Projectile.ai[0];
                timer++;
                if (Main.npc.IndexInRange(npc) && timer > 18)
                {
                    NPC target = Main.npc[npc];
                    if (!target.active || target.friendly)
                    {
                        Projectile.Kill();
                    }
                    float speed = timer / 240f;
                    Vector2 aim = Projectile.DirectionTo(target.Center) * Player.HeldItem.shootSpeed * Main.rand.NextFloat(0.95f, 1.05f);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, aim, speed);
                }
                for (int i = 0; i < 7; i++)
                {
                    var dusty = Dust.NewDustDirect(Projectile.position + new Vector2(2f, 2f), (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.PinkFairy, 0, 0, 255, default, Main.rand.NextFloat(0.2f, 1.2f));
                    dusty.velocity = Projectile.oldVelocity * 0.1f;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
        }
    }
}