using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class PlateBeltDef : ModPlayer
    {
        public bool DefToggle;

        public override void ResetEffects()
        {
            DefToggle = false;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (DefToggle)
                modifiers.FinalDamage *= 0.7f;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            if (DefToggle)
                modifiers.FinalDamage *= 0.7f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            if (DefToggle)
                modifiers.FinalDamage *= 0.7f;
        }
    }

    public class PlateBelt : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Plate Belt");
            // Tooltip.SetDefault("Deal 30% less damage but take 30% less damage");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.hasVanityEffects = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PlateBeltDef>().DefToggle = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Hay, 15)
                .AddIngredient(ItemID.Cobweb, 15)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}