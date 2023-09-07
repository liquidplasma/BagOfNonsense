using BagOfNonsense.Buffs;
using BagOfNonsense.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class CyberCut : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cyber Cut");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.aiStyle = 1;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.noDropItem = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            AIType = 14;
            Projectile.extraUpdates = 100;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.Next(0, 101) < GetWeaponCrit(player))
            {
                modifiers.SetCrit();
            }
            target.AddBuff(ModContent.BuffType<DVirus>(), 600);
            int a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(-1.5f, -1.5f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(-1.5f, 1.5f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(1.5f, -1.5f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(1.5f, 1.5f);
            Main.dust[a].color = new Color(0, 255, 255);

            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(0f, 1.5f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(0f, -1.5f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(1.5f, 0f);
            Main.dust[a].color = new Color(0, 255, 255);
            a = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 0.75f);
            Main.dust[a].velocity = new Vector2(-1.5f, 0f);
            Main.dust[a].color = new Color(0, 255, 255);
        }

        private int GetWeaponCrit(Player player)
        {
            Item item = player.inventory[player.selectedItem];
            int crit = item.crit;
            if (item.CountsAsClass(DamageClass.Melee))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Melee));
            }
            else if (item.CountsAsClass(DamageClass.Magic))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Magic));
            }
            else if (item.CountsAsClass(DamageClass.Ranged))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Ranged));
            }
            else if (item.CountsAsClass(DamageClass.Throwing))
            {
                crit = (int)(crit + player.GetCritChance(DamageClass.Throwing));
            }
            return crit;
        }

        public bool Touching(Player player)
        {
            return (Projectile.Distance(player.Center) < 50);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.alpha = 255;
            int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, ModContent.DustType<Neon>(), 0f, 0f, 0, new Color(0, 255, 255), 1f);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].color = new Color(0, 255, 255);
            if (Touching(player))
            {
                Projectile.Kill();
            }
        }
    }
}