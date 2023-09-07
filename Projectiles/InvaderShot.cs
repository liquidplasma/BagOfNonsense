using BagOfNonsense.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class InvaderShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Invader Shot");
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            Projectile.width = 4;
            Projectile.height = 8;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 45;
            Projectile.tileCollide = false;
            Projectile.noDropItem = true;
            Projectile.minion = true;
            AIType = 14;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<DVirus>(), 600);
        }
    }
}