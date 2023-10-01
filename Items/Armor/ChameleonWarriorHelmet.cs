using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChameleonWarriorHelmet : ModItem
    {
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            if (Main.gameMenu || !drawPlayer.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode)
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

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chameleon Warrior Helmet");
            /* Tooltip.SetDefault("16% increased damage\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.value = 300000;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.yoraiz0rDarkness = true;
            player.GetDamage(DamageClass.Generic) += 0.16f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ChameleonWarriorBreastplate>() && legs.type == ModContent.ItemType<ChameleonWarriorLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            BiomeInformations Biome = new()
            {
                Player = player
            };
            Biome.Update();
            player.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode = true;
            player.yoraiz0rDarkness = true;
            player.setBonus = "Grants buffs depending on biome\n" + "Inflicts [c/7A4E1E:Rot] on hit enemies\n" + "[c/7A4E1E:Rot] deals massive damage";
            if (player.ZoneForest)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += 0.15f;
            }
            else if (player.Center.Y / 16 <= Main.worldSurface * 0.35 || player.position.Y < 2500)
            {
                if (player.wingTimeMax > 0)
                    player.wingTimeMax = (int)(player.wingTimeMax * 1.5f);
            }
            else if (player.ZoneUnderworldHeight)
            {
                player.waterWalk = true;
                player.accFlipper = true;
                player.fireWalk = true;
                player.lavaMax += 1800;
            }
            else if (player.ZoneDungeon)
            {
                player.lifeRegen += 8;
                player.statDefense += 8;
                player.statLifeMax2 += 50;
            }
            else if (player.ZoneNormalCaverns || player.ZoneNormalUnderground)
            {
                player.pickSpeed -= 0.15f;
                player.AddBuff(BuffID.Spelunker, 2);
            }
            else if (player.ZoneCrimson || player.ZoneCorrupt)
            {
                player.statDefense += 5;
                player.lifeRegen += 5;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 10;
            }
            else if (player.ZoneHallow)
            {
                player.AddBuff(BuffID.Lovestruck, 2);
                player.AddBuff(BuffID.Sunflower, 2);
                player.accRunSpeed += 0.15f;
                player.maxRunSpeed += 0.15f;
                player.GetDamage(DamageClass.Generic) += 0.1f;
            }
            else if (player.ZoneGlowshroom)
            {
                player.shroomiteStealth = true;
                player.GetDamage(DamageClass.Generic) += ((1f - player.stealth) * 0.3f);
                player.GetCritChance(DamageClass.Generic) += (int)(((1f - player.stealth) * 0.1f) * 100f);
            }
            else if (player.ZoneDesert || player.ZoneUndergroundDesert)
            {
                player.detectCreature = true;
                player.dangerSense = true;
                player.noFallDmg = true;
            }
            else if (player.ZoneSnow)
            {
                player.buffImmune[BuffID.Frostburn] = true;
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Chilled] = true;
            }
            else if (player.ZoneBeach || player.position.X < 4000 || player.position.X > (Main.maxTilesX - 250) * 16)
            {
                player.AddBuff(BuffID.Sonar, 2);
                player.AddBuff(BuffID.Fishing, 2);
                player.AddBuff(BuffID.Calm, 2);
                player.AddBuff(BuffID.Crate, 2);
                player.accDivingHelm = true;
            }
            else if (player.ZoneJungle)
            {
                player.endurance += 0.1f;
                player.wingTimeMax = (int)(player.wingTimeMax * 1.2f);
                player.dangerSense = true;
                player.AddBuff(BuffID.Honey, 2);
            }
            Lighting.AddLight(player.Center, Biome.Color.ToVector3());
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode)
            {
                BiomeInformations Biome = new()
                {
                    Player = Main.LocalPlayer
                };
                Biome.Update();
                TooltipLine tp = new(Mod, "CurrentBiome", "Current Biome: " + Biome.name)
                {
                    OverrideColor = Biome.Color
                };
                TooltipLine buff = new(Mod, "Currentbuff", "Current buff: " + Biome.currentbuff)
                {
                    OverrideColor = Biome.Color
                };
                tooltips.Add(tp);
                tooltips.Add(buff);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PaladinBar>(), 8)
                .AddIngredient(ItemID.FrostHelmet)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}