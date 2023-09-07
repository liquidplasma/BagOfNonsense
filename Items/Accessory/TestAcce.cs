using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class TestAccModPlayer : ModPlayer
    {
        public bool SpawRateTestAcc;

        public override void ResetEffects()
        {
            SpawRateTestAcc = false;
        }
    }

    public class TestAccNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<TestAccModPlayer>().SpawRateTestAcc)
            {
                spawnRate = (int)(spawnRate / 60f);
                maxSpawns *= 50;
            }
        }
    }

    public class TestAcce : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("TestAcce");
            // Tooltip.SetDefault("You're not supposed to have this");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 24;
            Item.height = 24;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TestAccModPlayer>().SpawRateTestAcc = true;
            player.lifeRegen += 5020;
            player.noKnockback = true;
        }
    }
}