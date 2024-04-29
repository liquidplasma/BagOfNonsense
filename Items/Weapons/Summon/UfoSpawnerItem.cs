using BagOfNonsense.Helpers;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Summon
{
    public class UfoSpawnerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ufo Spawner");
            // Tooltip.SetDefault("Spawns tiny ufos to fight for you\n Ufos will attack nearby enemies and sometimes launch missiles\n Ufos will try to stay near base");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.knockBack = 0f;
            Item.mana = 10;
            Item.width = 28;
            Item.height = 26;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 100000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item44;
            Item.noUseGraphic = false;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.shoot = ModContent.ProjectileType<UfoSpawner>();
        }

        private static int GetBase(Player player)
        {
            int proj = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.type == ModContent.ProjectileType<UfoSpawner>())
                {
                    proj = p.identity;
                }
            }
            return proj;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0.01f;
            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                int projToKill = GetBase(player);
                if (Main.projectile.IndexInRange(projToKill))
                {
                    Projectile ufoBase = Main.projectile[projToKill];
                    for (int i = 0; i < 30; i++)
                    {
                        var gore = Gore.NewGoreDirect(ufoBase.GetSource_FromThis(), ufoBase.Center, default, HelperStats.GrenadeGore);
                        gore.velocity = Utils.RandomVector2(Main.rand, -2f, 2f) * Main.rand.NextFloat(1f, 4f);
                        Dust explosion = Dust.NewDustDirect(ufoBase.Center, 16, 16, HelperStats.SmokeyDust);
                        explosion.velocity = Utils.RandomVector2(Main.rand, -6, 6);
                        explosion.scale = 2f * Main.rand.NextFloat();
                    }
                    SoundEngine.PlaySound(SoundID.Item14, ufoBase.Center);
                    ufoBase.Kill();
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MartianConduitPlating, 50)
                .AddIngredient(ItemID.XenoStaff)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}