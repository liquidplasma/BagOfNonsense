using BagOfNonsense.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class VanillaCustomizations : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if ((projectile.type >= 184 && projectile.type <= 188) || projectile.type == 654)
                return;

            if (target.FindBuffIndex(ModContent.BuffType<DVirus>()) != -1)
            {
                modifiers.FinalDamage += 1.2f;
            }
        }
    }
}