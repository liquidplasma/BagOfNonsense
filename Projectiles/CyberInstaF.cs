using BagOfNonsense.Buffs;
using BagOfNonsense.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class CyberInstaF : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cyber Neon");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 9999;
            Projectile.tileCollide = false;
            Projectile.noDropItem = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 100;
            AIType = ProjectileID.Bullet;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.Next(0, 101) < GetWeaponCrit(player))
            {
                modifiers.SetCrit();
            }
            target.AddBuff(ModContent.BuffType<DVirus>(), 600);
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

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.alpha = 255;
            Projectile.frameCounter++;
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X + Main.rand.Next(-4, 5), Projectile.Center.Y + Main.rand.Next(-4, 5)), Projectile.width, Projectile.height, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].color = new Color(0, 255, 255);
            }
            if (player.whoAmI == Main.myPlayer)
            {
                if (Projectile.Center.Y >= (float)Main.mouseY + Main.screenPosition.Y)
                {
                    Projectile.tileCollide = true;
                }
            }
        }

        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[dust].velocity.X = (int)(Main.rand.Next(-8, 9));
                Main.dust[dust].velocity.Y = (int)(Main.rand.Next(-8, 9));
                Main.dust[dust].color = new Color(0, 255, 255);
            }
            Projectile.type = 0;
            return true;
        }
    }
}