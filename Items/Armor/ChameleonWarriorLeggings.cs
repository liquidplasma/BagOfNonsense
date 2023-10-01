using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ChameleonWarriorLeggings : ModItem
    {
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            if (Main.gameMenu || !drawPlayer.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode)
            {
                color = new Color(43, 163, 80);
            }
            else
            {
                BiomeInformations Biome = new();
                Biome.Player = drawPlayer;
                Biome.Update();
                color = Biome.Color;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chameleon Warrior Leggings");
            /* Tooltip.SetDefault("9% increased critical strike chance\n" +
                "15% increased movement speed\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 180000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 9;
            player.moveSpeed += 0.15f;
            player.accRunSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PaladinBar>(), 10)
                .AddIngredient(ItemID.FrostLeggings)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}