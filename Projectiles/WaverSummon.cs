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
    public class WaverSummonShot : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.InfluxWaver;

        private float HitCounter
        {
            get
            {
                return Projectile.ai[2];
            }
            set
            {
                Projectile.ai[2] = value;
            }
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.InfluxWaver);
            Projectile.DamageType = DamageClass.Summon;
            AIType = ProjectileID.InfluxWaver;
            Projectile.aiStyle = ProjAIStyleID.InfluxWaver;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Type);
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Frame();
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 1f - (HitCounter / 8f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int random = Utils.SelectRandom(Main.rand, 192, 250);
            HitCounter++;
            Vector2 velocity = Projectile.velocity;
            if (!target.friendly)
            {
                Projectile.Center = target.Center + Utils.NextVector2CircularEdge(Main.rand, random, random);
                Projectile.velocity = Projectile.Center.DirectionTo(target.Center) * velocity.Length() * 0.9f;
                Projectile.netUpdate = true;
                if (HitCounter >= 3)
                    Projectile.Kill();
            }
        }

        public override void AI()
        {
            Projectile.tileCollide = false;
        }
    }

    public class WaverSummon : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private int idleTime;

        private int IdleCirclePosition
        {
            get
            {
                return (int)Projectile.ai[2];
            }
        }

        public override string Texture => "BagOfNonsense/Items/Weapons/Summon/InfluxWepSummon";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Waver Summon");
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            Main.projPet[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Type] = false;
        }

        public override sealed void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 0;
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
            float size = (TextureAssets.Projectile[Type].Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            for (int i = 1; i < 5; i++)
            {
                float X = Projectile.velocity.X * i;
                float Y = Projectile.velocity.Y * i;
                Color color = Projectile.GetAlpha(projectileColor);
                float afterImage = 0f;
                if (i == 1)
                    afterImage = 0.4f;
                if (i == 2)
                    afterImage = 0.3f;
                if (i == 3)
                    afterImage = 0.2f;
                if (i == 4)
                    afterImage = 0.1f;
                color.R = (byte)(color.R * afterImage);
                color.G = (byte)(color.G * afterImage);
                color.B = (byte)(color.B * afterImage);
                color.A = (byte)(color.A * afterImage);
                Main.EntitySpriteDraw(Projectile.MyTexture(),
                    new Vector2(Projectile.position.X - Main.screenPosition.X + size - X, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY - Y),
                    new Rectangle(0, 0, TextureAssets.Projectile[Type].Width(), TextureAssets.Projectile[Type].Height()),
                    color,
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
            return false;
        }

        public override void AI()
        {
            int dustType = Utils.SelectRandom(Main.rand, 226, 229);
            if (!CheckActive(Player))
            {
                return;
            }
            if (Main.rand.NextBool(48))
            {
                for (int i = 0; i < Main.rand.Next(3); i++)
                {
                    Dust dusty = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Electric);
                    dusty.scale = 1.25f;
                    dusty.velocity = Vector2.Zero;
                    dusty.noGravity = true;
                    dusty.position -= Projectile.velocity * 0.5f;
                }
            }
            Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3() * 0.4f);
            Vector2 idlePosition = Player.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Player.active && distanceToIdlePosition > 1800)
            {
                Projectile.TeleportToOrigin(Player, idlePosition, dustType);
            }
            float attackVel = 0.02f;
            float projSpeed = 16f;
            int closestNPC = HelperStats.FindTargetProjectileCenter(Projectile, 1100f);
            if (closestNPC == -1) attackVel = 0.04f;
            if (Main.npc.IndexInRange(closestNPC))
            {
                idleTime = 0;
                Projectile.ai[0] += Utils.SelectRandom(Main.rand, 1, 3);
                NPC target = Main.npc[closestNPC];
                if (Player.HasMinionAttackTargetNPC)
                    target = Main.npc[Player.MinionAttackTargetNPC];
                Vector2 aim = Projectile.DirectionTo(Player.position + Player.Size) * (projSpeed * 1.15f);
                float distanceHead = Projectile.Distance(Player.Top);
                if (distanceHead > 200) attackVel = 0.08f;
                float amount = MathHelper.Lerp(attackVel, attackVel, Utils.GetLerpValue(140f, 30f, Projectile.timeLeft, clamped: true));
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, aim, amount);
                Projectile.rotation = Projectile.AngleTo(target.Center) - 5.5f;
                if (Projectile.ai[0] >= 100)
                {
                    Vector2 shootAim = Projectile.DirectionTo(target.Center) * 16f;
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, shootAim, ModContent.ProjectileType<WaverSummonShot>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
                    }
                }
            }
            else
            {
                idleTime++;
                float amount = MathHelper.Lerp(attackVel, attackVel, Utils.GetLerpValue(1f, 1f, Projectile.timeLeft, clamped: true));
                Vector2 idleStickPos = Player.Top - new Vector2(0, 64);
                Vector2 aim = Projectile.DirectionTo(idleStickPos) * projSpeed;
                Projectile.rotation = Projectile.AngleTo(Player.Center) - MathHelper.PiOver2 * 1.5f;
                if (idleTime > 300)
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.RotateAroundSomething(32f, IdleCirclePosition * 0.25f, Player.Center);
                }
                else
                {
                    Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, aim, amount);
                    FindOtherWavers();
                }
            }
        }

        private void FindOtherWavers()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile waver = Main.projectile[i];
                if (Projectile.identity == i)
                    continue;
                if (waver.active && waver.owner == Player.whoAmI && waver.type == Type)
                {
                    waver.ai[1] = 0;
                }
            }
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<BWaver>());
                return false;
            }
            if (owner.HasBuff(ModContent.BuffType<BWaver>()))
                Projectile.timeLeft = 2;
            return true;
        }
    }
}