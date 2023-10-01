using BagOfNonsense.Items.Ingredients;
using BagOfNonsense.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class BulleyeEmblemEffect : ModPlayer
    {
        public bool
            Bullseye16,
            Bullseye33,
            Bullseye50;

        public override void ResetEffects()
        {
            Bullseye16 = false;
            Bullseye33 = false;
            Bullseye50 = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Bullseye50)
                modifiers.CritDamage += 0.5f;
            if (Bullseye33)
                modifiers.CritDamage += 0.33f;
            if (Bullseye16)
                modifiers.CritDamage += 0.16f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Bullseye50)
                modifiers.CritDamage += 0.5f;
            if (Bullseye33)
                modifiers.CritDamage += 0.33f;
            if (Bullseye16)
                modifiers.CritDamage += 0.16f;
        }
    }

    public class BullseyeEmblem50 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terminator Emblem");
            // Tooltip.SetDefault("Increases crit damage by 50%");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AvengerEmblem);
            Item.rare = ModContent.RarityType<MoonFragmentRarity>();
            Item.hasVanityEffects = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ModContent.ItemType<BullseyeEmblem33>() || incomingItem.type == ModContent.ItemType<BullseyeEmblem16>())
                return false;
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BulleyeEmblemEffect>().Bullseye50 = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BullseyeEmblem33>())
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<MoonFragment>(), 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class BullseyeEmblem33 : ModItem

    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eradicator Emblem");
            // Tooltip.SetDefault("Increases crit damage by 33%");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AvengerEmblem);
            Item.rare = ItemRarityID.Lime;
            Item.hasVanityEffects = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ModContent.ItemType<BullseyeEmblem16>() || incomingItem.type == ModContent.ItemType<BullseyeEmblem50>())
                return false;
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BulleyeEmblemEffect>().Bullseye33 = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BullseyeEmblem16>())
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class BullseyeEmblem16 : ModItem

    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullseye Emblem");
            // Tooltip.SetDefault("Increases crit damage by 16%");
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ModContent.ItemType<BullseyeEmblem33>() || incomingItem.type == ModContent.ItemType<BullseyeEmblem50>())
                return false;
            return true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AvengerEmblem);
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BulleyeEmblemEffect>().Bullseye16 = true;
        }
    }
}