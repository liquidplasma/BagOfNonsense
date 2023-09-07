using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class MeleeJarateFlask : ModPlayer
    {
        public bool MeleeJarate;

        public override void ResetEffects()
        {
            MeleeJarate = false;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (!Main.rand.NextBool(2) && MeleeJarate)
            {
                for (int i = 0; i < 8; i++)
                {
                    int dusty = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Ichor, item.direction * 2, 0.0f, 150, default, 1f);
                    var dust = Main.dust[dusty];
                    Vector2 vector2 = Vector2.Multiply(dust.velocity, 0.2f);
                    dust.velocity = vector2;
                    Main.dust[dusty].noGravity = true;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (MeleeJarate)
            {
                if ((proj.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[proj.type]) && !proj.minion)
                    target.AddBuff(ModContent.BuffType<DJarate>(), (int)(60 * Main.rand.NextFloat(10, 15)));
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (MeleeJarate)
                target.AddBuff(ModContent.BuffType<DJarate>(), (int)(60 * Main.rand.NextFloat(10, 15)));
        }
    }

    public class BWeaponJarate : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Weapon Imbue: Jarate");
            // Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MeleeJarateFlask>().MeleeJarate = true;
        }
    }
}