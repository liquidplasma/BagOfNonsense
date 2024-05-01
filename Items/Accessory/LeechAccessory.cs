using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class LeechAccessoryModPlayer : ModPlayer
    {
        public bool
            isActive,
            regen;

        public int ticks;

        private const int DefaultMaxHeal = 100;

        public int
            HealCapMax,
            HealCapMax2;

        private static int DustType => Utils.SelectRandom(Main.rand, DustID.Blood, DustID.RedTorch);

        public override void ResetEffects()
        {
            isActive = false;

            if (HealCapMax2 <= 14)
                regen = true;
            else if (HealCapMax2 >= 90)
                regen = false;
        }

        public override void PostUpdateEquips()
        {
            if (regen && isActive)
            {
                ticks++;
                if (ticks >= 300)
                {
                    HealCapMax2 += 15;
                    ticks = 0;
                    for (int i = 0; i < 40; i++)
                    {
                        Dust recoverEffect = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustType);
                        recoverEffect.velocity = Utils.RandomVector2(Main.rand, 2f, 2f).RotatedByRandom(MathHelper.TwoPi);
                        recoverEffect.scale = 1.2f;
                        recoverEffect.noGravity = true;
                    }
                    ExtensionMethods.CreateCombatText(Player, Color.Red, "15").lifeTime = 27;
                }
            }
            HealCapMax2 = Utils.Clamp(HealCapMax2, 0, HealCapMax);
        }

        public override void Initialize()
        {
            HealCapMax = DefaultMaxHeal;
        }

        public override void UpdateDead()
        {
            HealCapMax2 = HealCapMax;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HealSelf(damageDone);
        }

        private void HealSelf(int damage)
        {
            if (isActive && Player.statLife < Player.statLifeMax2)
            {
                int healAmount = 1 + damage / 10;
                if (healAmount >= 10)
                    healAmount /= 2;
                if (healAmount <= DefaultMaxHeal / 2 && HealCapMax2 >= 15 && Main.rand.NextBool(6))
                {
                    HealCapMax2 -= healAmount;
                    Player.Heal(healAmount);
                }
            }
        }
    }

    public class LeechAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leech Emblem"); Tooltip.SetDefault("Attacks heal life");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AvengerEmblem);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            LeechAccessoryModPlayer leechEmblem = player.GetModPlayer<LeechAccessoryModPlayer>();
            leechEmblem.isActive = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            LeechAccessoryModPlayer leechModPlayer = Main.LocalPlayer.GetModPlayer<LeechAccessoryModPlayer>();
            if (leechModPlayer.isActive)
            {
                TooltipLine healing = new(Mod, "MaxHeal", "Healing left: " + leechModPlayer.HealCapMax2) { OverrideColor = Color.Red };
                TooltipLine healingTimeleft = new(Mod, "HealingTimeleft", "Time until healing recharge: " + (5 - leechModPlayer.ticks / 60)) { OverrideColor = Color.Yellow };
                tooltips.Add(healing);
                tooltips.Add(healingTimeleft);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BullseyeEmblem16>())
                .AddIngredient(ModContent.ItemType<PaladinBar>())
                .AddRecipeGroup("Any evil material", 15)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}