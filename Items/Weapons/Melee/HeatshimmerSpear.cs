using BagOfNonsense.Buffs;
using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Projectiles;
using BagOfNonsense.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Melee
{
    public class HeatshimmerSpear : ModItem
    {
        private static int timer;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heatshimmer Spear");
            // Tooltip.SetDefault("Consumes HP to power itself up");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 24f;
            Item.knockBack = 14f;
            Item.width = 69;
            Item.height = 69;
            Item.shoot = ModContent.ProjectileType<HeatshimmerProj>();
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 80;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                timer++;
                if (timer % 15 == 0 && timer <= 120)
                {
                    int damageplayer = (int)(player.statLife * 0.1f) + player.statDefense;
                    player.Hurt(PlayerDeathReason.ByPlayerItem(player.whoAmI, Item), damageplayer, player.direction, knockback: 0f, dodgeable: false);
                }
                if (timer >= 120)
                {
                    Item.shoot = ModContent.ProjectileType<HeatshimmerProjAwoken>();
                    Item.damage = 90;
                }
                if (timer >= 600) timer = 600;
            }
            else
            {
                Item.damage = 80;
                Item.shoot = ModContent.ProjectileType<HeatshimmerProj>();
                timer--;
                if (timer <= 0) timer = 0;
            }
            if (timer >= 120) 
                player.AddBuff(ModContent.BuffType<BShimmerSpear>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Spear)
                .AddIngredient(ItemID.FragmentSolar, 6)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}