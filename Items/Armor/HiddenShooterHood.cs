using BagOfNonsense.Items.Ingredients;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Armor
{
    public class SuperCrit : ModPlayer
    {
        public bool SuperCritBool = false;
        public bool printCrit = false;
        private int hitCounter;

        public override void ResetEffects()
        {
            SuperCritBool = false;
        }

        public void ShowCounter(Player player)
        {
            if (hitCounter == 5 || hitCounter == 10 || hitCounter == 15)
            {
                string crit;
                if (hitCounter == 15)
                    crit = "CRIT!!!";
                else
                    crit = hitCounter.ToString();
                int counterText = CombatText.NewText(player.getRect(), Color.Lime, crit, true);
                if (counterText < 100)
                {
                    Main.combatText[counterText].velocity.Y -= 8f;
                    Main.combatText[counterText].lifeTime = 24;
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            Player shooterOwner = Main.player[proj.owner];
            if (SuperCritBool &&
                !target.friendly &&
                (proj.CountsAsClass(DamageClass.Ranged) ||
                proj.CountsAsClass(DamageClass.Throwing)))
            {
                hitCounter++;
                ShowCounter(shooterOwner);
            }
            if (hitCounter == 15 ||
                SuperCritBool &&
                Main.rand.NextBool(50) &&
                !target.friendly &&
                (proj.CountsAsClass(DamageClass.Ranged) ||
                proj.CountsAsClass(DamageClass.Throwing))
                )
            {
                modifiers.FinalDamage *= 3;
                modifiers.Knockback *= 1.5f;
                printCrit = true;
                hitCounter = 0;
                Lighting.AddLight((int)(target.position.X + target.width / 2) / 16, (int)(target.position.Y + target.height / 2) / 16, 0.8f, 0.95f, 1f);
            }
            else
                printCrit = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (SuperCritBool && printCrit)
            {
                hit.HideCombatText = true;
                TF2Crit.CritSFXandText(target, damageDone);
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (SuperCritBool && Player.active && !Player.dead)
            {
                a = 55;
                fullBright = true;
            }
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class HiddenShooterHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hidden Shooter Hood");
            /* Tooltip.SetDefault("15% increased throwing and ranged critical strike chance\n" +
                "Enables zoom out (right click) with throwing and ranged weapons\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Throwing) += 15;
            player.GetCritChance(DamageClass.Ranged) += 15;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<HiddenShooterHood>() && body.type == ModContent.ItemType<HiddenShooterCoat>() && legs.type == ModContent.ItemType<HiddenShooterPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<SuperCrit>().SuperCritBool = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.AddBuff(BuffID.Warmth, 2, true);
            player.setBonus = "Permanent Warmth buff, immunity to Chilled and Frozen debuffs\n" +
                "2% chance to deal a super-crit dealing 3x damage\n" +
                "Enables auto reuse on all ranged weapons\n" +
                "Super-crit is guaranteed at 15 enemy hits\n" +
                "Only [c/70FF79:Ranged] and [c/FFBB4F:Throwing] damage can trigger this effect";

            if (player.HeldItem.CountsAsClass(DamageClass.Ranged))
                player.HeldItem.autoReuse = true;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<SuperCrit>().SuperCritBool)
            {
                TooltipLine damage = new(Mod, "DamageSoFar", "Damage done with Super Crits: " + TF2Crit.UpdateNumber()) { OverrideColor = Color.LightPink };
                tooltips.Add(damage);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.TitaniumBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IceEssence>(), 5)
                .AddIngredient(ItemID.AdamantiteBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}