using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    public class ActualBoneArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bone Marrow Arrow");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BoneArrow);
            Item.shoot = ProjectileID.BoneArrow;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 7f;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 1000;
            Item.DamageType = DamageClass.Ranged;
        }
    }
}