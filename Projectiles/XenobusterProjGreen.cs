using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class XenobusterProjBlue : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Xenobuster");

        public override void SetDefaults()
        {
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color projectileColor = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            float num138 = (TextureAssets.Projectile[Projectile.type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            for (int num432 = 1; num432 < 5; num432++)
            {
                float num433 = Projectile.velocity.X * num432;
                float num434 = Projectile.velocity.Y * num432;
                Color getAlpha = Projectile.GetAlpha(projectileColor);
                float num435 = 0f;
                if (num432 == 1)
                    num435 = 0.4f;
                if (num432 == 2)
                    num435 = 0.3f;
                if (num432 == 3)
                    num435 = 0.2f;
                if (num432 == 4)
                    num435 = 0.1f;
                getAlpha.R = (byte)(getAlpha.R * num435);
                getAlpha.G = (byte)(getAlpha.G * num435);
                getAlpha.B = (byte)(getAlpha.B * num435);
                getAlpha.A = (byte)(getAlpha.A * num435);
                Main.EntitySpriteDraw(Projectile.MyTexture(),
                    new Vector2(Projectile.position.X - Main.screenPosition.X + num138 - num433, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY - num434),
                    new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()),
                    getAlpha,
                    Projectile.rotation,
                    new Vector2(num138, Projectile.height / 2),
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (Projectile.Distance(npc.Center) <= 25 * 16 && !npc.friendly && npc.CanBeChasedBy() && Collision.CanHit(npc, Projectile))
                    Projectile.velocity = Projectile.DirectionTo(npc.Center) * 16f;
            }
            Projectile.FaceForward();
            if (Projectile.timeLeft < 98)
            {
                Projectile.alpha = 0;
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Projectile.direction * 2, 0.0f, 150, default, 1f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 0.2f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = true;
                float light = Projectile.alpha / 255f;
                Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.3f * light, 0.4f * light, 1f * light);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 7; i++)
            {
                int dusty = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Vortex, Projectile.direction * 2, 0.0f, 150, default, 1f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.1f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            for (int i = 0; i < 15; i++)
            {
                int dusty = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Electric, Projectile.direction * 2, 0.0f, 150, default, 0.66f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
            target.AddBuff(ModContent.BuffType<DHighwattage>(), 1800);
            Vector2 direction = target.DirectionTo(player.position) * 6f;
            if (Main.rand.NextFloat(1f) <= 0.01f && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), Projectile.position, direction, ProjectileID.SpiritHeal, damageDone, hit.Knockback, player.whoAmI, ai0: player.whoAmI, ai1: player.statLifeMax2 * 0.025f);
            }
        }
    }
}

namespace BagOfNonsense.Projectiles
{
    public class XenobusterProjGreen : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Xenobuster");

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 4;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 5;
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 NPCpos = Vector2.Add(Main.npc[target.whoAmI].position, Vector2.Multiply(Main.npc[target.whoAmI].Size, Utils.RandomVector2(Main.rand, -8f, 8f))); // spawn "zone"
                    Vector2 projspawn = Vector2.Multiply(Main.npc[target.whoAmI].DirectionFrom(NPCpos), 6f); // projectile spawn velocity
                    float velocity = Main.rand.NextFloat(1.4f, 1.66f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), NPCpos.X, NPCpos.Y, projspawn.X * velocity, projspawn.Y * velocity, ModContent.ProjectileType<XenobusterProjBlue>(), (int)(damageDone * 0.35f), 0, player.whoAmI, target.whoAmI, 0.0f);
                }
                for (int i = 0; i < 15; i++)
                {
                    int dusty = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.TerraBlade, Projectile.direction * Main.rand.NextFloat(0.1f, 0.8f), 0.0f, 150, default, 0.66f);
                    var dust = Main.dust[dusty];
                    Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                    dust.velocity = vector2;
                    Main.dust[dusty].noGravity = false;
                }
                Vector2 direction = target.DirectionTo(player.position) * 6f;
                if (Main.rand.NextFloat(1f) <= 0.05f && Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, direction, ProjectileID.SpiritHeal, damageDone, hit.Knockback, player.whoAmI, ai0: player.whoAmI, ai1: player.statLifeMax2 * 0.05f);
                }
            }
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Projectile.timeLeft == 3600)
            {
                SoundEngine.PlaySound(SoundID.Item60 with
                {
                    Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
                }, Projectile.position);
            }

            if (Projectile.timeLeft < 3595)
            {
                Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
                int dusty = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
                Dust dust = Main.dust[dusty];
                dust.noGravity = true;
                dust = Main.dust[dusty];
                dust.position -= Projectile.velocity * 0.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 31; i++)
            {
                int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.direction * -Main.rand.NextFloat(1f, 1.8f), 0.0f, 150, default, 0.66f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}