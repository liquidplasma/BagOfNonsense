using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Melee
{
    public class IceScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sub Zero Scythe");
            // Tooltip.SetDefault("It's burning but it's still cold, somehow");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item71;
            Item.value = 750000;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 67;
            Item.width = 72;
            Item.height = 64;
            Item.knockBack = 8f;
            Item.useTime = 21;
            Item.scale = 1f;
            Item.useAnimation = 21;
            Item.shoot = ModContent.ProjectileType<ScytheProj>();
            Item.shootSpeed = 18f;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Oiled, 3900);
            target.AddBuff(BuffID.Frostburn, 3900);
            target.AddBuff(BuffID.Chilled, 900);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.IceTorch);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = MathHelper.ToRadians(Main.rand.Next(60));
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (2 - 1))) * .2f;
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 6f, perturbedSpeed.Y * 6f, ModContent.ProjectileType<ScytheProj>(), (int)(damage * 1.2), knockback, player.whoAmI);
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceSickle)
                .AddIngredient(ItemID.DeathSickle)
                .AddIngredient(ItemID.FrostCore, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}