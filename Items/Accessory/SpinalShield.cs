using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class SpinalShield : ModItem
    {
        private Player Player { get; set; }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spinal Shield");
            /* Tooltip.SetDefault("Allows the player to dash into the enemy\nDouble tap a direction\nReduces damage taken by 17%\n" +
                "Increases damage and defense by 2% and 2 respectivelly for each enemy killed\nMax of 40% and 20 defense\nIncreases max health by 50 and life regen by 5"); */
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Expert;
            Item.knockBack = 9f;
            Item.defense = 4;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.accessory = true;
            Item.expert = true;
            Item.hasVanityEffects = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ModContent.ItemType<VikingDream>())
                return false;
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Player = player;
            player.dashType = 2;
            player.endurance += 0.17f;
            player.GetModPlayer<VikingCounter>().VikingDreamToggle = true;
            player.GetModPlayer<VikingCounter>().SpinalShield = true;
            player.statLifeMax2 += 50;
            player.lifeRegen += 5;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Player != null)
            {
                VikingCounter vkPlayer = Player.GetModPlayer<VikingCounter>();
                int t = vkPlayer.SecondsLeft;
                if (vkPlayer.VikingDreamToggle)
                {
                    TooltipLine damage = new(Mod, "CurrentBuff", "Damage: +" + vkPlayer.KillCounter * 2 + "%")
                    {
                        OverrideColor = Color.Yellow
                    };
                    TooltipLine defense = new(Mod, "CurrentDef", "Defense: +" + vkPlayer.Defense * 2)
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WormyShield>())
                .AddIngredient(ModContent.ItemType<VikingDream>())
                .AddIngredient(ItemID.LifeCrystal, 5)
                .AddIngredient(ItemID.Bone, 25)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}