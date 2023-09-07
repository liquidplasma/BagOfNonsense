using BagOfNonsense.Items.Accessory;
using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class StellarNinjaHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Ninja Helmet");
            /* Tooltip.SetDefault("50% chance to not consume thrown item\n" +
                "You are now smart as a ninja."); */
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Purple;
            Item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            player.ThrownCost50 = true;
            player.GetArmorPenetration(DamageClass.Generic) += 100;
            player.noFallDmg = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StellarNinjaBreastplate>() && legs.type == ModContent.ItemType<StellarNinjaLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            Lighting.AddLight(player.Center, Color.Gold.ToVector3());
            string bonus = "Major life regeneration";
            bool StellarGear = false;
            for (int l = 3; l < 8 + player.extraAccessorySlots; l++)
            {
                if (player.armor[l].type == ModContent.ItemType<NinjaSkills>())
                {
                    StellarGear = true;
                    break;
                }
            }
            if (StellarGear)
            {
                bonus += " and press [Z] to quick teleport to your cursor position";
            }
            if (player.name == "Zoaklen")
            {
                bonus += "\n'Strange things are happening with you...'";
                player.lifeRegen += 30;
                player.GetDamage(DamageClass.Throwing) += 0.5f;
                player.AddBuff(BuffID.Shine, 2);
                player.iceBarrier = true;
                player.iceBarrierFrameCounter += 1;
                if (player.iceBarrierFrameCounter > 2)
                {
                    player.iceBarrierFrameCounter = 0;
                    player.iceBarrierFrame += 1;
                    if (player.iceBarrierFrame >= 12)
                    {
                        player.iceBarrierFrame = 0;
                    }
                }
            }
            player.lifeRegen += 9;
            player.setBonus = bonus;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 8)
            .AddIngredient(ModContent.ItemType<StellarFragment>(), 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}