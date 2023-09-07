using BagOfNonsense.CoolStuff;
using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ChameleonWarriorBreastplate : ModItem
    {
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            if (Main.gameMenu || !drawPlayer.GetModPlayer<BONPlayer>().chameleonMode)
            {
                color = new Color(43, 163, 80);
            }
            else
            {
                BiomeInformations Biome = new()
                {
                    Player = drawPlayer
                };
                Biome.Update();
                color = Biome.Color;
            }
        }

        public override void ArmorArmGlowMask(Player drawPlayer, float shadow, ref int glowMask, ref Color color)
        {
            if (Main.gameMenu)
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
            // DisplayName.SetDefault("Chameleon Warrior Breastplate");
            /* Tooltip.SetDefault("9% increased damage\n" +
                "11% increased critical strike chance\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.value = 240000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 21;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.09f;
            player.GetCritChance(DamageClass.Generic) += 11;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PaladinBar>(), 12)
                .AddIngredient(ItemID.FrostBreastplate)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}