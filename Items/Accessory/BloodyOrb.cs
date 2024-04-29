using BagOfNonsense.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class BloodyOrbEffect : ModPlayer
    {
        public bool BloodyOrbToggle;

        public override void ResetEffects()
        {
            BloodyOrbToggle = false;
        }

        public override void PreUpdate()
        {
            if (BloodyOrbToggle)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && otherPlayer.team == Player.team && otherPlayer.dead)
                        Player.AddBuff(ModContent.BuffType<BBloodyRage>(), 2);
                }
            }
        }
    }

    public class BloodyOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bloody Orb");
            // Tooltip.SetDefault("Deal 90% more damage while a teammate is dead");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<BloodyOrbEffect>().BloodyOrbToggle = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadowOrb)
                .AddIngredient(ItemID.GoldBar, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.CrimsonHeart)
                .AddIngredient(ItemID.GoldBar, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}