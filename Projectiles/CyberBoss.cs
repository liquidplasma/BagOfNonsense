using BagOfNonsense.Buffs;
using BagOfNonsense.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class CyberBoss : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cyber Bits");
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.noDropItem = true;
            Projectile.alpha = 255;
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
            Projectile.alpha = 255;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 2 == 0)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, Utils.SelectRandom(Main.rand, ModContent.DustType<Binary0>(), ModContent.DustType<Binary1>()), 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }

        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Utils.SelectRandom(Main.rand, ModContent.DustType<Binary0>(), ModContent.DustType<Binary1>()), 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].velocity.X = (int)(Main.rand.Next(-8, 9));
                Main.dust[dust].velocity.Y = (int)(Main.rand.Next(-8, 9));
            }
            Projectile.type = 0;
            return true;
        }
    }
}