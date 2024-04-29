using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    internal class ManaCoreModPlayer : ModPlayer
    {
        public bool ManaCoreActive;

        public override void ResetEffects()
        {
            ManaCoreActive = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (ManaCoreActive)
            {
                int flatDamageInc = (int)(Player.statManaMax2 / 2f);
                Player.GetDamage(DamageClass.Magic).Flat += flatDamageInc;
                Player.statLifeMax2 += flatDamageInc / 2;
                Player.statDefense += flatDamageInc / 4;
            }
        }
    }

    public class ManaTear : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mana Tear");
            /* Tooltip.SetDefault("Increases maximum mana by 60\n" +
                "Increases mana regeneration rate\n" +
                "10% reduced mana usage\n" +
                "Automatically uses mana potion when needed\n" +
                "15% increased magic damage\n" +
                "Increases pickup range for mana stars\n" +
                "Restores mana when damaged\n" +
                "Permanent magic buffs\n" +
                "'You secretly stole the Dryad's tear'\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !(incomingItem.type == ModContent.ItemType<ManaCore>() || equippedItem.type == ModContent.ItemType<ManaCore>());
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 60;
            player.manaRegenDelayBonus++;
            player.manaRegenBonus += 25;
            player.manaCost -= 0.1f;
            player.manaFlower = true;
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.manaMagnet = true;
            player.magicCuffs = true;
            player.AddBuff(BuffID.ManaRegeneration, 2, true);
            player.AddBuff(BuffID.MagicPower, 2, true);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ManaFlower)
                .AddIngredient(ItemID.CelestialEmblem)
                .AddIngredient(ItemID.MagicCuffs)
                .AddIngredient(ItemID.ManaRegenerationBand)
                .AddIngredient(ItemID.MagicPowerPotion, 30)
                .AddIngredient(ItemID.ManaRegenerationPotion, 30)
                .AddTile(TileID.CrystalBall)
                .Register();
        }

        public class ManaCore : ManaTear
        {
            private float actualsize;
            private Geometry GeometryObject = new();

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Mana Core");
                // Tooltip.SetDefault("Adds flat magic damage, defense and health based on your max mana amount\nAll magic weapons no longer consume mana\nGrants all previous bonuses of: [i:" + ModContent.ItemType<ManaTear>() + "]");
                ItemID.Sets.ItemIconPulse[Item.type] = true;
                ItemID.Sets.ItemNoGravity[Item.type] = true;
            }

            public override void SetDefaults()
            {
                Item.width = 22;
                Item.height = 22;
                Item.rare = ModContent.RarityType<MoonFragmentRarity>();
                Item.accessory = true;
                Item.hasVanityEffects = true;
                Item.value = 600000;
            }

            public override void UpdateAccessory(Player player, bool hideVisual)
            {
                if (!hideVisual)
                {
                    Vector2 pos = player.MountedCenter + Utils.NextVector2CircularEdge(Main.rand, 48, 48);
                    Lighting.AddLight(player.Center, Color.DarkBlue.ToVector3());
                    Dust dusty = Dust.NewDustPerfect(pos, 134);
                    dusty.velocity = pos.DirectionTo(player.Center) * 4f;
                    dusty.shader = GameShaders.Armor.GetSecondaryShader(ContentSamples.ItemsByType[ItemID.BrightBlueDye].dye, Main.LocalPlayer);
                    dusty.noGravity = true;
                    dusty.scale = 0.8f;
                }
                base.UpdateAccessory(player, hideVisual);
            }

            public override void Update(ref float gravity, ref float maxFallSpeed)
            {
                int playerIndex = HelperStats.FindNearestPlayer(Item.Center, 600);
                Lighting.AddLight(Item.Center, Color.DarkBlue.ToVector3());
                if (Main.player.IndexInRange(playerIndex))
                {
                    Player activePlayer = Main.player[playerIndex];
                    if (Main.rand.NextBool(4))
                    {
                        Vector2 pos = Item.position + (Item.Size * Main.rand.NextFloat(0.1f, 1f));
                        Dust dusty = Dust.NewDustPerfect(pos, 156);
                        dusty.velocity = pos.DirectionTo(activePlayer.Center) * 50f * (activePlayer.Distance(Item.Center) / 600f);
                        dusty.shader = GameShaders.Armor.GetSecondaryShader(ContentSamples.ItemsByType[ItemID.BrightBlueDye].dye, Main.LocalPlayer);
                        dusty.noGravity = true;
                        dusty.scale = 1f;
                        dusty.noLight = true;
                    }
                }
                GeometryObject.DrawDustCircle(Item.Center, 20, 600f, 156);
                Lighting.AddLight(Item.Center, Color.DarkBlue.ToVector3());
                base.Update(ref gravity, ref maxFallSpeed);
            }

            public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
            {
                actualsize = GeometryObject.IncreaseDecrease(0.005f);
                scale = Math.Abs(actualsize);
                return true;
            }

            public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
            {
                return !(incomingItem.type == ModContent.ItemType<ManaTear>() || equippedItem.type == ModContent.ItemType<ManaTear>());
            }

            public override void UpdateEquip(Player player)
            {
                player.GetModPlayer<ManaCoreModPlayer>().ManaCoreActive = true;
                player.manaCost *= 0.00001f;
                base.UpdateEquip(player);
            }

            public override void AddRecipes()
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<ManaTear>())
                    .AddIngredient(ModContent.ItemType<MoonFragment>(), 8)
                    .AddTile(TileID.LunarCraftingStation)
                    .Register();
            }
        }
    }
}