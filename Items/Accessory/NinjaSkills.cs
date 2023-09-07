using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class NinjaSkills : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Ninja Soul");
            /* Tooltip.SetDefault("15% increased throwing damage, velocity and critical strike chance\n" +
                "Increases armor penetration by 50\n" +
                "Allows player ability to dash\n" +
                "Double tap into a direction\n" +
                "May confuse nearby enemies after being struck\n" +
                "Increases length of invincibility after taking damage"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 19));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.defense = 12;
            Item.accessory = true;
            Item.expert = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Throwing) += 15;
            player.GetDamage(DamageClass.Throwing) += 0.15f;
            player.ThrownVelocity += 0.15f;
            player.GetArmorPenetration(DamageClass.Generic) += 50;
            player.dash = 1;
            player.blackBelt = true;
            player.spikedBoots = 2;
            player.longInvince = true;
            player.GetModPlayer<NinjaDodgeAmulet>().NinjaDodgeAmuletbool = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NinjaKnuckles>())
                .AddIngredient(ModContent.ItemType<NinjaAmulet>())
                .AddIngredient(ItemID.MasterNinjaGear)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}