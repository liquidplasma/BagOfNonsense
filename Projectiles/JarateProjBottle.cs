using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class JarateProjBottle : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private float fadeColor = 1f;

        private static SoundStyle Jarateplayer => new("BagOfNonsense/Sounds/Other/spy_jaratehit", 3)
        {
            Volume = Main.rand.NextFloat(0.6f, 0.7f)
        };

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Piss Jar");
        }

        public override void SetDefaults()
        {
            Projectile.light = 0.1f;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.extraUpdates += 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, rect.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DJarate>(), 1800);
            for (int f = 0; f < 15; f++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 0, default);
            }
        }

        public static void JarateHitPlayerSFX(Player target)
        {
            SoundEngine.PlaySound(Jarateplayer, target.position);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= 0.1f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(250))
            {
                JarateHitPlayerSFX(target);
                int text = CombatText.NewText(target.getRect(), Color.Yellow, "IS THIS....? NOOOOO", true);
                if (text < 100)
                {
                    Main.combatText[text].lifeTime = 120;
                    Main.combatText[text].velocity.Y -= 16f;
                }
            }
            target.AddBuff(ModContent.BuffType<DJarate>(), 1800);
            Projectile.Kill();
        }

        public override void AI()
        {
            fadeColor *= 0.9875f;
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * fadeColor);
            float spin = Main.rand.NextFloat(0.2f, 0.5f);
            Projectile.ai[0] += 1f;
            if (Projectile.direction == -1)
                Projectile.rotation -= spin;
            else
                Projectile.rotation += spin;
            if (Projectile.ai[0] >= 40f)
            {
                Projectile.hostile = true;
                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y += 0.15f;
            }
        }

        public override bool PreKill(int timeLeft)
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (Projectile.Distance(npc.Center) <= 6 * 16 && !npc.friendly && npc.CanBeChasedBy() && Collision.CanHit(npc, Projectile))
                {
                    npc.AddBuff(ModContent.BuffType<DJarate>(), 1800);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                    }
                }
            }
            return base.PreKill(timeLeft);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
            }
        }
    }
}