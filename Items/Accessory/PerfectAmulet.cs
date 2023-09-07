using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class PerfectAmulet : ModItem
    {
        public int timer = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Perfect Amulet");
            // Tooltip.SetDefault("It's split in half");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = 400000;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public int FindTarget(float maxRange = 800f)
        {
            float num = maxRange;
            int result = -1;
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                bool flag = nPC.CanBeChasedBy();
                if (flag)
                {
                    Player player = Main.player[Main.myPlayer];
                    float num2 = player.Distance(nPC.Center);
                    if (num2 < num)
                    {
                        num = MathHelper.Min(num2, 800f);
                        result = i;
                    }
                }
            }
            return result;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            timer--;
            static int hardmode() => Main.hardMode ? 24 : 30;
            int nPC = FindTarget();
            if (timer <= 0
                && player == Main.player[Main.myPlayer]
                && player.ownedProjectileCounts[ModContent.ProjectileType<SummonedSword>()] <= 1
                && nPC != -1)
            {
                timer = hardmode();
                NPC target = Main.npc[nPC];
                Vector2 pos = new(player.Top.X - Main.rand.NextFloat(-30f, 30f), player.Top.Y - Main.rand.NextFloat(10f, 20f));
                Vector2 aim = pos.DirectionTo(target.Center);
                float distance = pos.DistanceSQ(target.Center);
                if (distance < 720 * 720
                    && !target.friendly
                    && !target.immortal
                    && target.CanBeChasedBy()
                    && Collision.CanHitLine(player.Center, 1, 1, target.Center, 1, 1))
                {
                    int getdamage = 20 + (int)(player.HeldItem.damage * 1.25f);
                    var proj = Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), pos, aim, ModContent.ProjectileType<SummonedSword>(), getdamage, 5f, Main.myPlayer, target.whoAmI);
                    proj.netUpdate = true;
                    proj.DamageType = DamageClass.Magic;
                    proj.ignoreWater = true;
                    proj.ArmorPenetration = 200;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ruby)
                .AddIngredient(ItemID.PanicNecklace)
                .AddIngredient(ItemID.GoldBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}