using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class ProjectileDeflectorAccessoryModPlayer : ModPlayer
    {
        public bool isEquipped;

        public override void ResetEffects()
        {
            isEquipped = false;
        }
    }

    public class ProjectileDeflectorAccessory : ModItem
    {
        public override string Texture => "BagOfNonsense/Projectiles/ProjectileReflector";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Projectile Deflector");
            // Tooltip.SetDefault("The deflector will protect you");
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(9, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ProjectileDeflectorAccessoryModPlayer>().isEquipped = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ProjectileReflector>()] < 1)
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<ProjectileReflector>(), 0, 0, player.whoAmI);
        }
    }
}