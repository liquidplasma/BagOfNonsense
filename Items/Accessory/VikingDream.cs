using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class VikingCounter : ModPlayer
    {
        public bool
            VikingDreamToggle,
            SpinalShield;

        private int timer;

        public int Defense { get; private set; }

        public int KillCounter { get; private set; }
        public int SecondsLeft { get; private set; }

        public override void ResetEffects()
        {
            VikingDreamToggle = false;
        }

        public override void PostUpdate()
        {
            if (VikingDreamToggle && KillCounter >= 1 && timer > 0)
                timer--;

            SecondsLeft = timer / 60;

            if (timer == 0)
            {
                Defense = 0;
                KillCounter = 0;
            }

            if (KillCounter >= 20) KillCounter = 20;
            if (Defense >= 10) Defense = 10;

            if (VikingDreamToggle)
            {
                Player.statDefense += SpinalShield ? 2 * KillCounter : 1 * KillCounter;
            }
        }

        private void UpdateDamage()
        {
            KillCounter++;
            Defense++;
            timer = 900;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (VikingDreamToggle && target.life <= 0)
                UpdateDamage();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (VikingDreamToggle && target.life <= 0)
                UpdateDamage();
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            if (VikingDreamToggle && target.active && !target.friendly && KillCounter >= 1)
            {
                int mult = SpinalShield ? 2 : 1;
                modifiers.FinalDamage *= 1 + 0.01f * KillCounter * mult;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            if (VikingDreamToggle && target.active && !target.friendly && KillCounter >= 1)
            {
                int mult = SpinalShield ? 2 : 1;
                modifiers.FinalDamage *= 1 + 0.01f * KillCounter * mult;
            }
        }
    }

    public class VikingDream : ModItem
    {
        private Player Player { get; set; }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vikings Dream");
            /* Tooltip.SetDefault("Increases damage and defense by 1% and 1 respectivelly for each enemy killed\n" +
                "Max of 20% and 10 defense"); */
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ModContent.ItemType<SpinalShield>())
                return false;
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Player = player;
            player.GetModPlayer<VikingCounter>().VikingDreamToggle = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Player != null)
            {
                VikingCounter vkPlayer = Player.GetModPlayer<VikingCounter>();
                int t = vkPlayer.SecondsLeft;
                if (vkPlayer.VikingDreamToggle)
                {
                    TooltipLine damage = new(Mod, "CurrentBuff", "Damage: +" + vkPlayer.KillCounter + "%")
                    {
                        OverrideColor = Color.Yellow
                    };
                    TooltipLine defense = new(Mod, "CurrentDef", "Defense: +" + vkPlayer.Defense)
                    {
                        OverrideColor = Color.Yellow
                    };
                    TooltipLine timer = new(Mod, "Timeleft", "Time left: " + t + "s")
                    {
                        OverrideColor = Color.Yellow
                    };
                    tooltips.Add(damage);
                    tooltips.Add(defense);
                    tooltips.Add(timer);
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.hasVanityEffects = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.VikingHelmet)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}