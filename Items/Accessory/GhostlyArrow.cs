using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    internal class GhostlyArrowTrail : GlobalProjectile
    {
        public bool ghostlyArrowTrailFlag;

        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (ghostlyArrowTrailFlag && Main.rand.NextBool(8))
            {
                Dust dusty = Dust.NewDustPerfect(projectile.Center, DustID.IchorTorch, null, 0, default, 1.1f);
                dusty.velocity += Vector2.Zero + projectile.oldVelocity * 0.1f;
                dusty.scale += 1f / Main.rand.NextFloat(50f, 100f);
                dusty.noGravity = !Main.rand.NextBool(20);
                dusty.fadeIn = 1.1f;
            }
            base.AI(projectile);
        }
    }
}

namespace BagOfNonsense.Items.Accessory
{
    public class GhostlyArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghostly Arrow");
            /* Tooltip.SetDefault("15% increased arrow damage and\n" +
                "using arrows spawns an additional arrow dealing 60% attack damage"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(60, 4));
        }

        public override void SetDefaults()
        {
            Item.scale = 1f;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 24;
            Item.height = 24;
            Item.value = 30000;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.arrowDamage += 0.15f;
            player.GetModPlayer<BagOfNonsenseModPlayer>().SpawnArrow = true;
        }
    }
}