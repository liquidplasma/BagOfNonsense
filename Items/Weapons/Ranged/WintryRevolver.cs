using BagOfNonsense.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class ColdRevDebuff : GlobalProjectile
    {
        public bool coldActivate;

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (coldActivate)
                target.AddBuff(ModContent.BuffType<DColdtouch>(), Main.rand.Next(300, 600));
        }
    }

    public class WintryRevolver : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wintry Handcannon");
            // Tooltip.SetDefault("5% chance to shoot a rocket!!");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.width = 72;
            Item.height = 44;
            Item.shoot = ProjectileID.Bullet;
            Item.knockBack = 6f;
            Item.useAmmo = AmmoID.Bullet;
            Item.damage = 68;
            Item.shootSpeed = 20f;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.scale = 0.7f;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.crit = 9;
        }

        public override bool? UseItem(Player player)
        {
            SoundStyle shoot = new("BagOfNonsense/Sounds/Custom/357shot", 2)
            {
                Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
            };
            SoundEngine.PlaySound(shoot, player.Center);
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 56f;
            if (Collision.CanHit(position, 1, 1, position + muzzleOffset, 1, 1))
                position += muzzleOffset;
            Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            Vector2 dustpos = new(position.X, position.Y - 8f);
            int projupdates;
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.Blizzard;
                projupdates = 2;
            }
            else projupdates = 1;

            if (Main.rand.NextFloat(1f) <= 0.05f)
                Projectile.NewProjectile(source, position.X, position.Y - 8f, velocity.X, velocity.Y, ProjectileID.ClusterRocketI, (int)(damage * 2f), knockback, player.whoAmI);
            else
            {
                Projectile blizzy = Projectile.NewProjectileDirect(source, dustpos, spread, type, (int)(damage * 1.25f), (int)(knockback * 1.25f), player.whoAmI);
                blizzy.DamageType = DamageClass.Ranged;
                blizzy.extraUpdates += projupdates;
                blizzy.netUpdate = true;
                blizzy.GetGlobalProjectile<ColdRevDebuff>().coldActivate = true;
                for (int i = 0; i < 36; i++)
                {
                    Dust effect = Dust.NewDustDirect(blizzy.Center, 4, 4, Utils.SelectRandom(Main.rand, 259, 31), velocity.X * 0.4f, velocity.Y * 0.4f);
                    effect.noGravity = true;
                }
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(0, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FrostCore, 2)
            .AddIngredient(ModContent.ItemType<ColdRevolver>())
            .AddIngredient(ItemID.IllegalGunParts, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}