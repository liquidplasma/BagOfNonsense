using BagOfNonsense.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class ThunderballBomb : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Thunderball");

        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 400;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 999;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;
            Projectile.extraUpdates = 100;
            Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DHighwattage>(), 300);
            target.immune[Projectile.owner] = 0;
            Projectile.Kill();
        }

        public override void AI()
        {
            for (int i = 0; i < 60; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.Hitbox.X, Projectile.Hitbox.Y), 400, 400, DustID.Vortex, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.25f);
                var dust = Main.dust[dusty];
                dust.noGravity = true;
            }
        }
    }
}

namespace BagOfNonsense.Projectiles
{
    public class ThunderballProj1 : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Thunderball");

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.275f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            target.AddBuff(ModContent.BuffType<DHighwattage>(), 300);
        }

        public override void OnKill(int timeLeft)
        {
            var owner = Main.player[Projectile.owner];
            int damage = owner.inventory[owner.selectedItem].damage;
            if (Main.myPlayer == Projectile.owner)
                Projectile.NewProjectile(owner.GetSource_FromThis(), Projectile.position, Projectile.velocity * 0, ModContent.ProjectileType<ThunderballBomb>(), (int)(damage * 1.75f), 0f, owner.whoAmI);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, Projectile.direction, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                dust.noGravity = true;
            }
        }
    }
}

namespace BagOfNonsense.Projectiles
{
    public class ThunderballProj : ModProjectile
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Thunderball");

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.scale = 1f;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DHighwattage>(), 300);
            target.immune[Projectile.owner] = 0;
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            if (Projectile.alpha > 0) Projectile.alpha -= 15;
            if (Projectile.timeLeft < 3595)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 6, 6, DustID.Vortex, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.25f);
                var dust = Main.dust[dusty];
                dust.velocity *= -0.25f;
                dust = Main.dust[dusty];
                dust.velocity *= -0.25f;
                dust = Main.dust[dusty];
                dust.position -= Projectile.velocity * 0.5f;
                dust.noGravity = true;
                Main.dust[dusty].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            var owner = Main.player[Projectile.owner];
            int damage = owner.inventory[owner.selectedItem].damage;
            float knockback = owner.inventory[owner.selectedItem].knockBack;
            if (owner == Main.player[Main.myPlayer])
            {
                Projectile.NewProjectile(owner.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 50f, 0, -10f, ModContent.ProjectileType<ThunderballProj1>(), (int)(damage * 1.75f), knockback, Main.myPlayer);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dusty = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Vortex, Projectile.direction * 2, 0.0f, 150, default, 0.9f);
                var dust = Main.dust[dusty];
                Vector2 vector2 = Vector2.Multiply(dust.velocity, 1.5f);
                dust.velocity = vector2;
                Main.dust[dusty].noGravity = false;
            }
        }
    }
}