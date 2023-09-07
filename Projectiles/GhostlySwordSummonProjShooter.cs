using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class GhostlySwordSummonProjShooter : ModProjectile
    {
        public override string Texture => "BagOfNonsense/Projectiles/GhostlySwordSummonProj";
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly SwordS");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
        }

        public override sealed void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color projectileColor = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            float size = (TextureAssets.Projectile[Projectile.type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            for (int i = 1; i < 5; i++)
            {
                float X = Projectile.velocity.X * i;
                float Y = Projectile.velocity.Y * i;
                Color getAlpha = Projectile.GetAlpha(projectileColor);
                float afterImage = 0f;
                if (i == 1)
                    afterImage = 0.4f;
                if (i == 2)
                    afterImage = 0.3f;
                if (i == 3)
                    afterImage = 0.2f;
                if (i == 4)
                    afterImage = 0.1f;
                getAlpha.R = (byte)(getAlpha.R * afterImage);
                getAlpha.G = (byte)(getAlpha.G * afterImage);
                getAlpha.B = (byte)(getAlpha.B * afterImage);
                getAlpha.A = (byte)(getAlpha.A * afterImage);
                Main.EntitySpriteDraw(Projectile.MyTexture(),
                    new Vector2(Projectile.position.X - Main.screenPosition.X + size - X, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY - Y),
                    new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()),
                    getAlpha,
                    Projectile.rotation,
                    new Vector2(size, Projectile.height / 2),
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            if (!CheckActive(Player))
            {
                return;
            }
            int dustType = Utils.SelectRandom(Main.rand, 15, 57, 58);
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, dustType, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
                int dusty = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, dustType, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
                Dust dust = Main.dust[dusty];
                dust.noGravity = true;
                dust = Main.dust[dusty];
                dust.position -= Projectile.velocity * 0.5f;
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
            Vector2 idlePosition = Player.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Player.active && distanceToIdlePosition > 1200f)
            {
                Projectile.TeleportToOrigin(Player, idlePosition, dustType);
            }
            float projSpeed = 16f;
            int closestNPC = HelperStats.FindTargetNoLOS(Projectile, 1100f);
            if (closestNPC != -1)
            {
                Projectile.ai[0] += Utils.SelectRandom(Main.rand, 1, 3, 5, 7);
                NPC target = Main.npc[closestNPC];
                if (Player.HasMinionAttackTargetNPC)
                    target = Main.npc[Player.MinionAttackTargetNPC];
                float attackVel = 0.05f;
                Vector2 aim = Projectile.DirectionTo(Player.Top - new Vector2(Main.rand.Next(-36, 36), 36)) * (projSpeed * 1.15f);
                float distanceHead = Projectile.Distance(Player.Top);
                if (distanceHead > 200) attackVel = 0.08f;
                float amount = MathHelper.Lerp(attackVel, attackVel, Utils.GetLerpValue(140f, 30f, Projectile.timeLeft, clamped: true));
                Projectile.alpha = 0;
                Projectile.friendly = true;
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, aim, amount);
                Projectile.rotation = Projectile.AngleTo(target.Center) - 5.5f;
                if (Projectile.ai[0] >= 84)
                {
                    Vector2 shootAim = Projectile.DirectionTo(target.Center) * 15f;
                    int typeBeam = Utils.SelectRandom(Main.rand, ProjectileID.EnchantedBeam, ProjectileID.SwordBeam);
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim, typeBeam, Projectile.damage, Projectile.knockBack, Player.whoAmI);
                        shooty.DamageType = DamageClass.Summon;
                        shooty.penetrate = 1;
                        shooty.tileCollide = false;
                        shooty.extraUpdates = 3;
                        shooty.CritChance = 0;
                        shooty.timeLeft = 480;
                    }
                }
            }
            else
            {
                float amount = MathHelper.Lerp(0.03f, 0.03f, Utils.GetLerpValue(1f, 1f, Projectile.timeLeft, clamped: true));
                Vector2 aim = Projectile.DirectionTo(Player.Top - new Vector2(Main.rand.Next(-12, 12), 64)) * projSpeed;
                Projectile.rotation = Projectile.AngleTo(Player.Top) - MathHelper.PiOver2 * 1.5f;
                Projectile.alpha = 160;
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, aim, amount);
            }
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<BGhostlySword>());
                return false;
            }
            if (owner.HasBuff(ModContent.BuffType<BGhostlySword>()))
                Projectile.timeLeft = 2;
            return true;
        }
    }
}