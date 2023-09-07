using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class NinjaKnuckles : ModItem
    {
        /*public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Back);
			return true;
		}*/

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ninja Knuckles");
            /* Tooltip.SetDefault("Increases armor penetration by 5\n" +
                "4% increased damage and critical strike chance\n" +
                "'Punch them at the super sonic speed'"); */
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 100000;
            Item.defense = 6;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            player.GetCritChance(DamageClass.Melee) += 4;
            player.GetCritChance(DamageClass.Magic) += 4;
            player.GetCritChance(DamageClass.Ranged) += 4;
            player.GetCritChance(DamageClass.Throwing) += 4;
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetDamage(DamageClass.Throwing) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FleshKnuckles)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}