using BagOfNonsense.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Accessory
{
    public class SwordSpiritModPlayer : ModPlayer
    {
        private bool SwordThrowerLimit => Player.ownedProjectileCounts[ModContent.ProjectileType<SwordThrower>()] < 3;
        public bool isActive;

        public override void ResetEffects()
        {
            isActive = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (SwordThrowerLimit && (item.CountsAsClass(DamageClass.Melee) || item.CountsAsClass(DamageClass.MeleeNoSpeed)) && isActive)
            {
                Projectile projy = Projectile.NewProjectileDirect(Projectile.InheritSource(item), Player.Top + new Vector2(0, Main.rand.Next(-128, -64)), Vector2.Zero, ModContent.ProjectileType<SwordThrower>(), damageDone, 1f, Player.whoAmI, target.whoAmI);
                projy.originalDamage = damageDone;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (SwordThrowerLimit && (proj.CountsAsClass(DamageClass.Melee) || proj.CountsAsClass(DamageClass.MeleeNoSpeed)) && isActive && Main.myPlayer == proj.owner)
            {
                Projectile projy = Projectile.NewProjectileDirect(Projectile.InheritSource(proj), Player.Top + new Vector2(0, Main.rand.Next(-128, -64)), Vector2.Zero, ModContent.ProjectileType<SwordThrower>(), damageDone, 1f, Player.whoAmI, target.whoAmI);
                projy.originalDamage = damageDone;
            }
        }
    }

    public class SwordSpirit : ModItem
    {
        public override string Texture => "BagOfNonsense/Projectiles/Pets/SwordThrower";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sword Spirit");
            // Tooltip.SetDefault("Hitting enemies with melee damage will spawn\na sword spirit that will attack enemies");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.accessory = true;
            Item.hasVanityEffects = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SwordSpiritModPlayer>().isActive = true;
        }
    }
}