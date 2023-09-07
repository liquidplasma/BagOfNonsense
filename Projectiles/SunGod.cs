using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class SunGod : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private bool playerHasMana;
        private static int MonkeyGlasses => ModContent.ItemType<MonkeyGlasses>();
        private static int SuperMonkeyGlasses => ModContent.ItemType<SuperMonkeyGlasses>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SunGod");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 88;
            Projectile.height = 71;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 390;
            Projectile.alpha = 0;
            Projectile.friendly = true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3());
            if (Player.whoAmI == Main.myPlayer && Main.mouseRight && !Player.controlInv && (Player.HeldItem.type == SuperMonkeyGlasses || Player.HeldItem.type == MonkeyGlasses))
                Projectile.Kill();
            Projectile.CheckPlayerActiveAndNotDead(Player);
            Projectile.velocity = Vector2.Zero;
            if (Player.channel && Main.myPlayer == Player.whoAmI && (Player.HeldItem.type == ModContent.ItemType<MonkeyGlasses>() || Player.HeldItem.type == ModContent.ItemType<SuperMonkeyGlasses>()))
            {
                Projectile.Center = Main.MouseWorld;
                Projectile.netUpdate = true;
            }

            int aimTarget = HelperStats.FindTargetLOSProjectile(Projectile, 1000);
            if (Main.npc.IndexInRange(aimTarget))
            {
                if (Player.statMana <= ContentSamples.ItemsByType[ModContent.ItemType<SuperMonkeyGlasses>()].mana)
                    playerHasMana = false;
                int mana75Percent = Player.statManaMax2 - (Player.statManaMax2 / 4);
                if (Player.statMana >= mana75Percent)
                    playerHasMana = true;

                NPC target = Main.npc[aimTarget];
                Projectile.rotation = Projectile.AngleTo(target.Center) + MathHelper.PiOver2;
                Projectile.ai[0] += 1f;
                if (target.active && Projectile.ai[0] >= 4 && playerHasMana)
                {
                    Player.CheckMana(6, pay: true);
                    Projectile.ai[0] = 0;
                    Vector2 aim = Projectile.Center.DirectionTo(target.Center) * 4f;
                    float numberProjectiles = 3;
                    float rotation = MathHelper.ToRadians(11);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = aim.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            var sun = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + (aim * 4f), perturbedSpeed, ModContent.ProjectileType<SunRay>(), Projectile.damage, 0f, Player.whoAmI);
                            sun.CritChance = 0;
                            sun.ArmorPenetration = 8;
                        }
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(180, 250); i++)
            {
                Dust death = Dust.NewDustDirect(Projectile.Center, 4, 4, DustID.IchorTorch, Scale: 1.4f);
                death.noGravity = true;
                death.scale *= 1.1f * Main.rand.NextFloat(3f);
                if (Main.rand.NextBool())
                    death.velocity = Utils.NextVector2Circular(Main.rand, 5f, 5f);
                else
                {
                    death.position = Projectile.Center + Utils.NextVector2CircularEdge(Main.rand, 96, 96);
                    death.velocity = death.position.DirectionTo(Projectile.Center) * 10f;
                }
            }
            SoundEngine.PlaySound(SoundID.DD2_BetsyDeath with
            {
                PitchRange = (0.4f, 0.7f),
                MaxInstances = 0,
            }, Projectile.Center);
        }
    }
}