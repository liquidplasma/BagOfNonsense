using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Others
{
    public class CelestialClock : ModItem
    {
        private bool iii = true;
        private int state;
        private bool day;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Celestial Clock");
            /* Tooltip.SetDefault("Indefinitely switch between day and night\n" +
                "[c/2E86C1:ZoaklenMod Port]"); */
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 32;
            Item.height = 32;
            Item.value = 5000000;
            Item.UseSound = SoundID.Item42;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void HoldItem(Player player)
        {
            Main.StopRain();
            if (state > 0)
                state--;
            if (Main.myPlayer == player.whoAmI)
            {
                if (state > 0 && day)
                {
                    int i = CombatText.NewText(player.getRect(), Color.Blue, "Starting night...");
                    if (Main.combatText.IndexInRange(i))
                    {
                        Main.combatText[i].lifeTime = 180;
                        Main.combatText[i].velocity.Y = Main.rand.NextFloat(-4, -8);
                        Main.combatText[i].velocity.X = Main.rand.NextFloat(-2, 2);
                    }
                }
                else if (state > 0 && !day)
                {
                    int i = CombatText.NewText(player.getRect(), Color.Gold, "Starting day...");
                    if (Main.combatText.IndexInRange(i))
                    {
                        Main.combatText[i].lifeTime = 180;
                        Main.combatText[i].velocity.Y = Main.rand.NextFloat(-4, -8);
                        Main.combatText[i].velocity.X = Main.rand.NextFloat(-2, 2);
                    }
                }
            }
        }

        public override bool? UseItem(Player player)
        {
            if (player.itemTime == 0 && player.itemAnimation > 0 && (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer))
            {
                player.itemTime = Item.useTime;
                if (Main.dayTime)
                {
                    Main.UpdateTime_StartNight(stopEvents: ref iii);
                    state = 2;
                    day = true;
                }
                else
                {
                    Main.UpdateTime_StartDay(stopEvents: ref iii);
                    state = 2;
                    day = false;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ItemID.GoldBar, 10)
                 .AddIngredient(ItemID.Diamond, 5)
                 .AddTile(TileID.WorkBenches)
                 .Register();
        }
    }
}