using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class ColdRevolver : ModItem
    {
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Chilled Magnum");

        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 48;
            Item.height = 28;
            Item.shoot = ProjectileID.Bullet;
            Item.knockBack = 3f;
            Item.useAmmo = AmmoID.Bullet;
            Item.damage = 34;
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.scale = 1f;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 18;
        }

        public override void UpdateInventory(Player player)
        {
            SoundStyle shoot = new("BagOfNonsense/Sounds/Custom/357shot", 2);
            Item.UseSound = shoot with
            {
                Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
            };
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 48f;
            if (Collision.CanHit(position, 1, 1, position + muzzleOffset, 1, 1))
                position += muzzleOffset;
            int projupdates = 1;
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.Blizzard;
                projupdates = 2;
            }
            Vector2 dustpos = new(position.X, position.Y - 8f);

            Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(1));
            Projectile proj = Projectile.NewProjectileDirect(source, dustpos, spread * 1.66f, type, damage, 8, player.whoAmI);
            proj.DamageType = DamageClass.Ranged;
            proj.extraUpdates += projupdates;
            proj.GetGlobalProjectile<ColdRevDebuff>().coldActivate = true;
            proj.netUpdate = true;
            for (int i = 0; i < 15; i++)
            {
                Dust effect = Dust.NewDustDirect(proj.Center, 4, 4, Utils.SelectRandom(Main.rand, 259, 31), velocity.X * 0.67f, velocity.Y * 0.67f);
                effect.noGravity = true;
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<ColdRevolver>())
                 .AddIngredient(ItemID.Revolver)
                 .AddIngredient(ItemID.IceBlock, 30)
                 .AddIngredient(ItemID.IllegalGunParts)
                 .AddTile(TileID.Anvils)
                 .Register();

            Recipe.Create(ItemID.Revolver)
                 .AddIngredient(ItemID.IronBar, 10)
                 .AddIngredient(ItemID.IllegalGunParts, 5)
                 .AddTile(TileID.Anvils)
                 .Register();
        }
    }
}