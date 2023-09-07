using BagOfNonsense.Buffs;
using BagOfNonsense.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons
{
    //imported from my tAPI mod because I'm lazy
    public class CyberStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Arcade Staff");
            // Tooltip.SetDefault("'Press left click to insert a coin'" + "\n[c/2E86C1:ZoaklenMod Port]");
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Invader1>();
            Item.buffType = ModContent.BuffType<BInvader>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<Pacman>();
                Item.buffType = ModContent.BuffType<BPacman>();
            }
            else
            {
                Item.shoot = Utils.SelectRandom(Main.rand,
                    ModContent.ProjectileType<Invader1>(),
                    ModContent.ProjectileType<Invader2>(),
                    ModContent.ProjectileType<Invader3>());
                Item.buffType = ModContent.BuffType<BInvader>();
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
            return false;
        }
    }
}