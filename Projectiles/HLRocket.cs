using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class HLRocketGunProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private Vector2 mousePosLauncher;
        private int soundEffect;

        private static SoundStyle Fire => new("BagOfNonsense/Sounds/Weapons/hl_rocketfire")
        {
            Volume = 0.33f,
        };

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rocket Launcher");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.width = 38;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                mousePosLauncher = Main.MouseWorld;
                Projectile.netUpdate = true;
            }

            Projectile.KeepAliveIfOwnerIsAlive(Player);

            int mouseDirection = (Player.DirectionTo(mousePosLauncher).X > 0f) ? 1 : -1;
            Player.ChangeDir(mouseDirection);

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<HLRocketLaser>()] < 1 && Main.myPlayer == Projectile.owner)
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Player.Center, Vector2.Zero, ModContent.ProjectileType<HLRocketLaser>(), 0, 0, Player.whoAmI);

            if (soundEffect > 0) soundEffect--;
            if (soundEffect == 30)
                SoundEngine.PlaySound(Fire, Player.Center);

            Projectile.ai[0]++;
            if (Player.HasAmmo(Player.HeldItem) && Player.channel && Player.ownedProjectileCounts[ModContent.ProjectileType<HLRocket>()] < 1 && Projectile.ai[0] > Player.HeldItem.useTime)
            {
                Projectile.ai[0] = 0;
                soundEffect = 31;
                Item ammo = Player.ChooseAmmo(Player.HeldItem);
                int fixDamage = (int)((ammo.damage + Projectile.damage) * Player.GetPlayerDamageMultiplier(Player.HeldItem.DamageType));
                Vector2 aim = Projectile.Bottom.DirectionTo(Projectile.Top) * 9;
                Vector2 offset = Projectile.Center.DirectionTo(mousePosLauncher) * 32f;
                ExtensionMethods.BetterNewProjectile(Player, Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), Projectile.Center + offset, aim, ModContent.ProjectileType<HLRocket>(), fixDamage, 20f, Player.whoAmI);
            }

            int followIndex = HelperStats.FindProjectileIndex(Player, ModContent.ProjectileType<HLRocketLaser>());
            if (Main.projectile.IndexInRange(followIndex))
            {
                Projectile toFollow = Main.projectile[followIndex];
                Projectile.rotation = Projectile.AngleTo(toFollow.Center) + MathHelper.PiOver2;
            }

            Projectile.spriteDirection = Player.direction;

            Player.HoldOutArm(Projectile, mousePosLauncher);
            Projectile.Center = Player.MountedCenter + new Vector2(0, -4);

            Player.heldProj = Projectile.whoAmI;
            if (Player.HeldItem.type != ModContent.ItemType<HLRocketGun>())
                Projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(mousePosLauncher);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mousePosLauncher = reader.ReadVector2();
        }
    }

    public class HLRocketLaser : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rocket Laser");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.netImportant = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            Projectile.rotation++;
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.Center = Main.MouseWorld;
                Projectile.netUpdate = true;
            }

            if (Player.HeldItem.type != ModContent.ItemType<HLRocketGun>())
                Projectile.Kill();
        }
    }

    public class HLRocket : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private int boomSound;

        // public override void SetStaticDefaults() => DisplayName.SetDefault("Rocket");

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            AIType = 0;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 720;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (boomSound != 1)
                Boom();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            boomSound = 1;
        }

        private void Boom()
        {
            Projectile.netUpdate = true;
            Projectile.timeLeft = 6;
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= 6)
            {
                Projectile.Resize(360, 360);
                Projectile.alpha = 255;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                Projectile.netUpdate = true;
                HelperStats.SmokeGore(Projectile.GetSource_Death(), Projectile.Center, 35, 4);
            }
            Projectile.alpha -= 15;
            Projectile.ai[0]++;
            int followIndex = HelperStats.FindProjectileIndex(Player, ModContent.ProjectileType<HLRocketLaser>());
            if (Projectile.ai[0] >= 24)
            {
                for (int i = 0; i < Main.rand.Next(4, 18); i++)
                {
                    int type = Main.rand.NextBool(12) ? DustID.Torch : DustID.Smoke;
                    Dust dusty = Dust.NewDustPerfect(Projectile.Center, type);
                    dusty.velocity = Vector2.Zero + Utils.NextVector2Circular(Main.rand, -4, 4) * (Main.rand.NextBool() ? 0.1f : 0.33f);
                    if (type == DustID.Torch)
                    {
                        Dust extraDusty = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                        extraDusty.velocity = (Projectile.oldVelocity * 0.1f) + Vector2.Zero + Utils.NextVector2Circular(Main.rand, -4, 4) * (Main.rand.NextBool() ? 0.2f : 0.53f);
                    }
                    dusty.fadeIn = 1f;
                }

                if (Main.projectile.IndexInRange(followIndex))
                {
                    Projectile followThis = Main.projectile[followIndex];
                    Vector2 aim = Projectile.Center.DirectionTo(followThis.Center) * 24f;
                    float distance = 0.1f - Projectile.Distance(followThis.Center) * 0.000872f;
                    Main.NewText(distance);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, aim, 0.08f - distance);
                    Projectile.FaceForward();
                }
            }
            else
            {
                Projectile.velocity.Y += 0.5f;
            }
            if (Main.projectile.IndexInRange(followIndex))
            {
                Projectile followThis = Main.projectile[followIndex];
                if (Projectile.ai[0] == 1)
                    Projectile.rotation = Projectile.AngleTo(followThis.Center) + MathHelper.PiOver2;
                if (Projectile.ai[0] <= 24)
                    Projectile.velocity += Projectile.Center.DirectionFrom(followThis.Center) * 0.06f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Boom();
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 60; i++)
            {
                var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center + Utils.RandomVector2(Main.rand, -8, 8), default, HelperStats.GrenadeGore);
                gore.velocity = Utils.RandomVector2(Main.rand, -2f, 2f) * Main.rand.NextFloat(2f, 4f);
            }
            if (boomSound != 1)
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }
}