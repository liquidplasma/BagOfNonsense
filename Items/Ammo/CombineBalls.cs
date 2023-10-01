using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Ammo
{
    internal class CombineBalls : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PulseBall>();
            Item.damage = 1000;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = false;
            Item.ammo = Type;
            Item.knockBack = 7f;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.DamageType = DamageClass.Default;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Rectangle rect = texture.Bounds;
            scale = 0.2f;
            Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, rect, lightColor, rotation, rect.Size() / 2, scale, SpriteEffects.None);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ItemID.Cog)
                .AddIngredient(ModContent.ItemType<PulseAmmo>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}