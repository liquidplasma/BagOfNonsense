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
            Projectile.timeLeft = 390;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.light = 1f;
            Projectile.scale = 0.5f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            if (Player.whoAmI == Main.myPlayer && Main.mouseRight && Player.HeldItem.type == AirControlUnit)
                Projectile.Kill();

            if (Player.channel && Main.myPlayer == Player.whoAmI && Player.HeldItem.type == ModContent.ItemType<AirControlUnit>() && Player.ItemAnimationJustStarted)
            {
                Projectile.Center = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            Projectile.CheckPlayerActiveAndNotDead(Player);
            Projectile.velocity = Vector2.Zero;
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<Spectre>()] < 1)
            {
                var spectre = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Spectre>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
                spectre.originalDamage = Projectile.damage;
            }
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

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        private static Vector2 GetBase(Player player)
        {
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == SpectreCenterIndex && proj.owner == player.whoAmI)
                {
                    pos = proj.Center;
                }
            }
            return pos;
        }

        public override void AI()
        {
            if (Player.whoAmI == Main.myPlayer && Main.mouseRight && Player.HeldItem.type == AirControlUnit)
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.CheckPlayerActiveAndNotDead(Player);
            Vector2 pos = GetBase(Player);
            Projectile.RotateAroundSomething(408, 1f, pos, false);
            Projectile.rotation = Projectile.AngleTo(pos);
            int aimNPC = HelperStats.FindTargetLOSProjectile(Projectile, 1400);
            if (Main.npc.IndexInRange(aimNPC))
            {
                if (Player.statMana <= ContentSamples.ItemsByType[AirControlUnit].mana)
                    playerHasMana = false;
                int mana75Percent = Player.statManaMax2 - (Player.statManaMax2 / 4);
                if (Player.statMana >= mana75Percent)
                    playerHasMana = true;

                NPC target = Main.npc[aimNPC];
                int NPCnext = HelperStats.FindNextNPC(Projectile, target, 1400);
                Projectile.ai[0]++;
                Vector2 aim = Projectile.Center.DirectionTo(target.Center) * 12f;
                if (target.active && Projectile.ai[0] > 4 && playerHasMana)
                {
                    Player.CheckMana(6, true);
                    Projectile.ai[0] = 0;
                    if (Main.myPlayer == Player.whoAmI)
                    {
                        var shooty = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, aim, ModContent.ProjectileType<SpectreBomb>(), Projectile.damage, 0.8f, Player.whoAmI);
                        var dart = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, (aim * 0.9f).RotatedByRandom(MathHelper.ToRadians(4)) * Main.rand.NextFloat(.8f, 1.2f), ModContent.ProjectileType<MonkeyDart>(), (int)(Projectile.damage * 0.5f), 0f, Player.whoAmI);
                        shooty.CritChance = 0;
                        dart.CritChance = 0;
                    }
                    if (Main.npc.IndexInRange(NPCnext))
                    {
                        NPC otherTarget = Main.npc[NPCnext];
                        Vector2 aimNext = Projectile.Center.DirectionTo(otherTarget.Center) * 12f;
                        if (Projectile.DistanceSQ(otherTarget.Center) < 1400 * 1400 && Main.myPlayer == Player.whoAmI)
                        {
                            var extraShoot = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, aimNext, ModContent.ProjectileType<SpectreBomb>(), Projectile.damage, 0.8f, Player.whoAmI);
                            extraShoot.CritChance = 0;
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
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spectre Bomb");
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
            Projectile.light = 1f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Boom(Projectile, damageDone, target, Player);
        }

        private static void Boom(Projectile proj, int damage, NPC target, Player player)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC targetNext = Main.npc[i];
                if (targetNext.whoAmI != target.whoAmI && targetNext.active && !targetNext.friendly && targetNext.CanBeChasedBy() && proj.DistanceSQ(targetNext.Center) <= 64 * 64)
                {
                    NPC.HitInfo hit = new() { Damage = damage, DamageType = DamageClass.Magic };
                    targetNext.StrikeNPC(hit);
                    NetMessage.SendStrikeNPC(target, hit);
                }
            }
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 295)
                Projectile.alpha -= 33;

            Projectile.FaceForward();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, default, HelperStats.GrenadeGore);
                gore.velocity = Utils.RandomVector2(Main.rand, -1f, 1f) * Main.rand.NextFloat(1f, 4f);
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }

    public class MonkeyDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Monkey Dart");
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
            Projectile.light = 1f;
            Projectile.alpha = 255;
            Projectile.scale = 0.5f;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 295)
                Projectile.alpha -= 33;

            Projectile.FaceForward();
        }
    }
}