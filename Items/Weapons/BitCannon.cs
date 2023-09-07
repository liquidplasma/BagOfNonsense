using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons
{
    public class BitCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bit Cannon");
            // Tooltip.SetDefault("'Let me hurt you a bit'" + "\n[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.crit = 25;
            Item.width = 38;
            Item.height = 20;
            Item.shoot = 10;
            Item.useTime = 2;
            Item.useAnimation = 8;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item42;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 100;
            Item.shootSpeed = 30f;
            Item.noMelee = true;
            Item.value = 200000;
            Item.knockBack = 8f;
            Item.rare = ItemRarityID.Red;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Bullet;
            Item.autoReuse = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<CyberBit>();
            damage = (int)(damage * 0.5f);
            velocity.X += Main.rand.Next(-4, 5);
            velocity.Y += Main.rand.Next(-4, 5);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -8);
        }
    }
}