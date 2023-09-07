using BagOfNonsense.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class NinjaDodgeAmulet : ModPlayer
    {
        public bool NinjaDodgeAmuletbool;

        public override void ResetEffects()
        {
            NinjaDodgeAmuletbool = false;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        {
            if (NinjaDodgeAmuletbool && Main.rand.NextBool(4))
            {
                for (int num9 = 0; num9 < 200; num9++)
                {
                    double num2 = Player.CalculateDamagePlayersTake((int)modifiers.FinalDamage.Flat, Player.statDefense);
                    if (!Main.npc[num9].active || Main.npc[num9].friendly)
                    {
                        continue;
                    }
                    int num10 = 300;
                    num10 += (int)num2 * 2;
                    if (Main.rand.Next(500) < num10)
                    {
                        float num11 = (Main.npc[num9].Center - Player.Center).Length();
                        float num12 = Main.rand.Next(200 + (int)num2 / 2, 301 + (int)num2 * 2);
                        if (num12 > 500f)
                            num12 = 500f + (num12 - 500f) * 0.75f;
                        if (num12 > 700f)
                            num12 = 700f + (num12 - 700f) * 0.5f;
                        if (num12 > 900f)
                            num12 = 900f + (num12 - 900f) * 0.25f;

                        if (num11 < num12)
                        {
                            float num13 = Main.rand.Next(90 + (int)num2 / 3, 300 + (int)num2 / 2);
                            Main.npc[num9].AddBuff(31, (int)num13);
                        }
                    }
                }
            }
        }
    }

    public class NinjaAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ninja Amulet");
            /* Tooltip.SetDefault("Allows player to dash into an enemy\n" +
                "Double tap into a direction\n" +
                "May confuse nearby enemies after being struck\n" +
                "Increases length of invincibility after taking damage\n" +
                "'You see your future in the jewel'\n" +
                 "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightPurple;
            Item.knockBack = 9f;
            Item.defense = 3;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.accessory = true;
            Item.expert = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.dashType = 2;
            player.longInvince = true;
            player.GetModPlayer<NinjaDodgeAmulet>().NinjaDodgeAmuletbool = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EoCShield)
                .AddIngredient(ItemID.BrainOfConfusion)
                .AddIngredient(ItemID.CrossNecklace)
                .AddTile(TileID.AdamantiteForge)
                .Register();
            Recipe.Create(ItemID.BrainOfConfusion)
                .AddIngredient(ItemID.WormScarf)
                .AddTile(TileID.DemonAltar)
                .Register();
            Recipe.Create(ItemID.WormScarf)
                .AddIngredient(ItemID.BrainOfConfusion)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}