using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class SpectreCenter : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private static int AirControlUnit => ModContent.ItemType<AirControlUnit>();

        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.alpha = 0;
            Projectile.friendly = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            if (!Player.mouseInterface && Player.whoAmI == Main.myPlayer && !Player.mouseInterface && Main.mouseRight && Player.HeldItem.type == AirControlUnit)
                Projectile.Kill();

            if (Player.channel && Main.myPlayer == Player.whoAmI && Player.HeldItem.type == ModContent.ItemType<AirControlUnit>() && Player.ItemAnimationJustStarted)
            {
                Projectile.Center = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            Projectile.velocity = Vector2.Zero;
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<Spectre>()] < 1)
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Spectre>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
        }
    }

    public class Spectre : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private bool playerHasMana;

        private static int SpectreCenterIndex => ModContent.ProjectileType<SpectreCenter>();
        private static int AirControlUnit => ModContent.ItemType<AirControlUnit>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spectre");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 208;
            Projectile.height = 148;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 390;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.light = 1f;
            Projectile.scale = 1f;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        private static Projectile GetBase(Player player)
        {
            Projectile pos = null;
            int spectreCenter = HelperStats.FindProjectileIndex(player, SpectreCenterIndex);
            if (Main.projectile.IndexInRange(spectreCenter))
            {
                Projectile Base = Main.projectile[spectreCenter];
                if (Base.active)
                    pos = Base;
            }
            return pos;
        }

        public override bool PreAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            if (Player.whoAmI == Main.myPlayer && !Player.mouseInterface && Main.mouseRight && Player.HeldItem.type == AirControlUnit)
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            Projectile pos = GetBase(Player);
            if (pos != null)
            {
                Projectile.RotateAroundSomething(408, 1f, pos.Center, false);
                Projectile.rotation = Projectile.AngleTo(pos.Center);
                int aimNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1400);
                if (Main.npc.IndexInRange(aimNPC))
                {
                    if (Player.statMana <= ContentSamples.ItemsByType[AirControlUnit].mana)
                        playerHasMana = false;
                    int mana75Percent = Player.statManaMax2 - (Player.statManaMax2 / 4);
                    if (Player.statMana >= mana75Percent)
                        playerHasMana = true;

                    NPC target = Main.npc[aimNPC];
                    Projectile.ai[0]++;
                    Vector2 aim = Projectile.Center.DirectionTo(target.Center) * 12f;
                    if (target.active && Projectile.ai[0] > 6 && Player.whoAmI == Main.myPlayer)
                    {
                        Projectile.ai[0] = 0;
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, aim.RotatedByRandom(MathHelper.ToRadians(4)) * Main.rand.NextFloat(.8f, 1.2f), ModContent.ProjectileType<MonkeyDart>(), (int)(Projectile.damage * 0.5f), 0f, Player.whoAmI);
                        if (playerHasMana)
                        {
                            Player.CheckMana(6, true);
                            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, aim, ModContent.ProjectileType<SpectreBomb>(), Projectile.damage, 0.8f, Player.whoAmI);
                        }
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 60; i++)
            {
                var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, default, HelperStats.GrenadeGore);
                gore.velocity = Utils.RandomVector2(Main.rand, -2f, 2f) * Main.rand.NextFloat(1f, 8f);
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            HelperStats.Fire(Projectile.Center, 100, 2f * Main.rand.NextFloat(0.5f, 1f), 4f);
        }
    }

    public class SpectreBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
            Projectile.CritChance = 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 295)
                Projectile.alpha -= 33;

            Projectile.FaceForward();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 2; i++)
            {
                var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, default, HelperStats.GrenadeGore);
                gore.velocity = Utils.RandomVector2(Main.rand, -1f, 1f) * Main.rand.NextFloat(4f);
            }
        }
    }

    public class MonkeyDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            DrawOriginOffsetY = -6;
            DrawOffsetX = -6;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.scale = 0.5f;
            Projectile.extraUpdates = 3;
            Projectile.CritChance = 0;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 295)
                Projectile.alpha -= 33;

            Projectile.FaceForward();
        }
    }
}