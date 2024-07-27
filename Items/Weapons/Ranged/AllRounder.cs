using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Ranged
{
    public class AllRounder : ModItem
    {
        private List<int> projectileTypes = new();

        private List<int> itemsToConsume = new();

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 22;
            Item.knockBack = 1f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 27;
            Item.shootSpeed = 12.5f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int j = 0; j < player.inventory.Length; j++)
            {
                Item ammoItem = player.inventory[j];
                if (ammoItem.IsAir)
                    continue;

                if (ammoItem.ammo == Item.useAmmo)
                {
                    projectileTypes.Add(ammoItem.shoot);
                    itemsToConsume.Add(ammoItem.type);
                }
            }

            for (int i = 0; i < projectileTypes.Count; i++)
            {
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
                Projectile.NewProjectile(source, position, velocity, projectileTypes[i], damage, knockback, player.whoAmI);
                if (ContentSamples.ItemsByType[itemsToConsume[i]].consumable)
                    player.ConsumeItem(itemsToConsume[i]);
            }
            projectileTypes.Clear();
            itemsToConsume.Clear();
            return false;
        }
    }
}