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
    public class GhostlySwordSummonProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly Sword");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
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
            Projectile.CritChance = 0;
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
                NPC target = Main.npc[closestNPC];
                if (Player.HasMinionAttackTargetNPC)
                    target = Main.npc[Player.MinionAttackTargetNPC];
                float ProjDistanceNPC = Projectile.Distance(target.Center);
                float speed() => ProjDistanceNPC > 150 ? 0.15f : 0.02f;
                float attackVel = speed();
                float amount = MathHelper.Lerp(attackVel, attackVel + 0.08f, Utils.GetLerpValue(140f, 30f, Projectile.timeLeft, clamped: true));
                Vector2 aim = Projectile.DirectionTo(target.Center) * (projSpeed * 1.15f);
                Projectile.alpha = 0;
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, aim, amount);
                float spin = Main.rand.NextFloat(0.2f, 0.5f);
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation -= spin;
                else
                    Projectile.rotation += spin;
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