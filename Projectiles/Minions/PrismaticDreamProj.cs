using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    public class PrismaticControl : ModPlayer
    {
        public bool PrismAllow;
        private int attackTarget;
        private int canShoot;

        public override void ResetEffects()
        {
            PrismAllow = false;
            if (canShoot > 0)
            {
                canShoot--;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (target.active
                && !target.friendly
                && damageDone > 0)
            {
                attackTarget = target.whoAmI;
                canShoot = 2;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI == proj.owner
                && !HelperStats.IsAStarSummon(proj)
                && damageDone > 0
                && target.active
                && !target.friendly)
            {
                attackTarget = target.whoAmI;
                canShoot = 2;
            }
        }

        public int GetTarget()
        {
            if (attackTarget != -1)
            {
                return attackTarget;
            }
            else
            {
                return attackTarget = -1;
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
    }

    public class PrismaticDreamProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private static int TypeDust => Utils.SelectRandom(Main.rand, 57, 58);

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("PrismaticDreamProj");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 76;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.minion = false;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            PrismaticControl prismControl = Player.GetModPlayer<PrismaticControl>();
            prismControl.PrismAllow = true;
            if (!CheckActive(Player))
                return;

            if (Main.rand.NextBool(2))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, TypeDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
            Projectile.rotation += 0.1f;
            Projectile.RotateAroundSomething(64, 3f, Player.RotatedRelativePoint(Player.MountedCenter));
            int autoAttack = HelperStats.FindTargetLOSPlayer(Projectile, 1600, Player);
            if (autoAttack != -1)
            {
                NPC targetAA = Main.npc[autoAttack];
                if (Player.HasMinionAttackTargetNPC)
                    targetAA = Main.npc[Player.MinionAttackTargetNPC];
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 20 && targetAA.active && targetAA.Distance(Player.Center) <= 16f * 50f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position + Projectile.Size * Main.rand.NextFloat(), Vector2.Zero, ModContent.ProjectileType<PrismStarRed>(), (int)(Projectile.damage * 0.1f), 1f, Player.whoAmI, targetAA.whoAmI);
                    }
                    Projectile.ai[0] = 0;
                }
            }
            int manualAim = prismControl.GetTarget();
            if (manualAim != -1)
            {
                NPC target = Main.npc[manualAim];
                if (prismControl.GetAllowed() && target.active && HelperStats.GetBlueStarCount(Player) <= 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position + Projectile.Size * Main.rand.NextFloat(), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -3f)) * 1.5f, ModContent.ProjectileType<PrismStarBlue>(), (int)(Projectile.damage * Main.rand.NextFloat(0.4f, 0.55f)), 1f, Player.whoAmI, target.whoAmI);
                        }
                    }
                }
            }
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<BPrismDream>());
                return false;
            }
            if (owner.HasBuff(ModContent.BuffType<BPrismDream>()))
                Projectile.timeLeft = 2;
            return true;
        }
    }
}