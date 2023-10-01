using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class SpookyQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spooky Quiver");
            /* Tooltip.SetDefault("20% increased arrow damage and speed,\n" +
                "using arrows will spawn an additional arrow that deals 80% attack damage\n" +
                "Turns the holder into a werewolf at night and a merfolk when entering water\n" +
                "Mild increase to damage, melee speed, critical strike chance,\n" +
                "life, life regeneration, defense, mining speed, and minion knockback"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(9, 3));
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Purple;
            Item.width = 30;
            Item.height = 30;
            Item.value = 300000;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (hideVisual)
            {
                player.hideMerman = true;
                player.hideWolf = true;
            }
            player.magicQuiver = true;
            player.accMerman = true;
            player.wolfAcc = true;
            player.lifeRegen += 4;
            player.statDefense += 8;
            player.arrowDamage += 0.2f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
            player.GetDamage(DamageClass.Melee) += 0.2f;
            player.GetCritChance(DamageClass.Generic) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.2f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.GetDamage(DamageClass.Magic) += 0.2f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.pickSpeed -= 0.25f;
            player.GetDamage(DamageClass.Summon) += 0.2f;
            player.GetKnockback(DamageClass.Summon) += 1.5f;
            player.GetDamage(DamageClass.Throwing) += 0.2f;
            player.GetCritChance(DamageClass.Throwing) += 5;
            player.statLifeMax2 += 25;
            player.GetModPlayer<BagOfNonsenseModPlayer>().SpawnArrowQuiver = true;
        }

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CelestialShell);
            recipe.AddIngredient(ItemID.MagicQuiver);
            recipe.AddIngredient(ModContent.ItemType<GhostlyArrow>());
            recipe.AddIngredient(ItemID.FragmentVortex, 15);
            recipe.Register();
        }
    }
}