using BagOfNonsense.Buffs;
using BagOfNonsense.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class SuspiciousLookingJoystick : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Suspicious Looking Joystick");
            // Tooltip.SetDefault("'Reward for defeating the techno doom'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CompanionCube);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<PracticalCube>();
            Item.buffType = ModContent.BuffType<BPracticalCube>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
            base.UseStyle(player, heldItemFrame);
        }
    }
}