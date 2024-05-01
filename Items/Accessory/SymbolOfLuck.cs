using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class SymbolOfLuckModPlayer : ModPlayer
    {
        public bool active;

        public override void ResetEffects()
        {
            active = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (active)
            {
                float mult = Main.rand.NextFloat(0.01f, 1.99f);
                modifiers.FinalDamage *= mult;
            }
        }
    }

    public class SymbolOfLuck : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 31;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            base.SetDefaults();
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.5f;
            ExtensionMethods.BetterEntityDraw(Item.MyTexture(), Item.Center, Item.MyTexture().Bounds, lightColor, rotation, Item.MyTexture().Size() / 2, scale, 0); ;
            return false;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SymbolOfLuckModPlayer>().active = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RodofDiscord)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}