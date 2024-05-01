using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class FireBirdBase : ModProjectile
    {
        private float distance;

        private int fixDamage;

        private Vector2
            mousePosFireBird,
            placeToGo;

        private int ShotDelay
        {
            get
            {
                return (int)Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }

        private int TypeDust => Utils.SelectRandom(Main.rand, 259, 64, 158);

        private Player Player => Main.player[Projectile.owner];
        public override string Texture => "BagOfNonsense/Items/Weapons/Ranged/FireBird";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Bird Base");
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
            Projectile.extraUpdates = 1;
            Projectile.ContinuouslyUpdateDamageStats = true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreAI()
        {
            Projectile.alpha = 0;
            if (Player.HasAmmo(Player.HeldItem))
            {
                Lighting.AddLight(Projectile.Center, Color.Wheat.ToVector3());
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = (Projectile.frame + 1) % 5;
                }
            }
            if (Player.whoAmI == Main.myPlayer && !Player.channel && Player.HasAmmo(Player.HeldItem))
            {
                Vector2 aim = Projectile.Center.DirectionTo(Main.MouseWorld) * 160f;
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.RedTorch, aim, 0, default, 1);
                dust.noGravity = true;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            Projectile.KeepAliveIfOwnerIsAlive(Player);
            if (Player.whoAmI == Main.myPlayer)
            {
                ShotDelay++;
                mousePosFireBird = Main.MouseWorld;
                Projectile.netUpdate = true;
                int useTime = (int)(Player.HeldItem.useTime * (Player.HasFireBirdUpgrade() ? 1.5f : 2));
                if (Player.channel && ShotDelay >= useTime && Player.HasAmmo(Player.HeldItem))
                {
                    Item ammo = Player.ChooseAmmo(Player.HeldItem);
                    fixDamage = (int)(Player.GetDamage(DamageClass.Ranged).ApplyTo(ammo.damage) + Projectile.damage);
                    ShotDelay = 0;
                    Vector2 aim = Projectile.Center.DirectionTo(mousePosFireBird) * 14f * Main.rand.NextFloat(0.9f, 1.1f);
                    Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), Projectile.Center, aim, ModContent.ProjectileType<FireBirdProj>(), fixDamage, Projectile.knockBack, Player.whoAmI);
                }
            }
            Vector2 centerFixed = Player.Center;
            Projectile.rotation = Projectile.AngleTo(mousePosFireBird) + MathHelper.PiOver4;
            distance = Player.Distance(mousePosFireBird);
            float projPos = distance / 6;
            switch (Projectile.ai[1])
            {
                case 1:
                    placeToGo = centerFixed + new Vector2(0, -projPos);
                    break;

                case 2:
                    placeToGo = centerFixed + new Vector2(0, projPos);
                    break;

                case 3:
                    placeToGo = centerFixed + new Vector2(projPos, 0);
                    break;

                case 4:
                    placeToGo = centerFixed + new Vector2(-projPos, 0);
                    break;

                case 5:
                    placeToGo = centerFixed + new Vector2(-projPos * 0.5f, projPos * 0.5f);
                    break;

                case 6:
                    placeToGo = centerFixed + new Vector2(-projPos * 0.5f, -projPos * 0.5f);
                    break;

                case 7:
                    placeToGo = centerFixed + new Vector2(projPos * 0.5f, -projPos * 0.5f);
                    break;

                case 8:
                    placeToGo = centerFixed + new Vector2(projPos * 0.5f, projPos * 0.5f);
                    break;
            }
            Projectile.Center = placeToGo;

            if (Projectile.Center.DistanceSQ(Player.Center) >= 1400 * 1400)
                Projectile.TeleportToOrigin(Player, placeToGo, TypeDust);

            if (Player.HeldItem.type != ModContent.ItemType<FireBird>())
                Projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(mousePosFireBird);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mousePosFireBird = reader.ReadVector2();
        }
    }

    public class FireBirdProj : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private static int TypeDust => Utils.SelectRandom(Main.rand, 259, 64, 158);

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
            Projectile.extraUpdates = 3;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Player.HasFireBirdUpgrade())
            {
                Projectile.extraUpdates = 5;
                Projectile.netUpdate = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Player.HasFireBirdUpgrade())
                return Color.BlueViolet;

            return base.GetAlpha(lightColor);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.coldDamage)
                modifiers.FinalDamage *= 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            FireBurnGlobal fireBurnGlobal = target.GetGlobalNPC<FireBurnGlobal>();
            int time = (int)(300 * Main.rand.NextFloat(2f, 3f));
            fireBurnGlobal.Player = Player;
            fireBurnGlobal.burningTime = time;
            fireBurnGlobal.burning = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 300)
            {
                SoundStyle shotSound = SoundID.DD2_PhantomPhoenixShot;
                SoundEngine.PlaySound(shotSound with
                {
                    Pitch = Main.rand.NextFloat(.8f, 1.2f)
                }, Projectile.Center);
            }
            var dusty = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, TypeDust, Projectile.direction * 2, 0.0f, 150, default, 1f);
            dusty.velocity *= 0.2f;
            dusty.noGravity = true;
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());
            Projectile.alpha -= 15;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        }
    }
}