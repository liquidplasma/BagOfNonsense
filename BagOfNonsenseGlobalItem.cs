using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace BagOfNonsense
{
    internal class BagOfNonsenseGlobalItem : GlobalItem
    {
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.NecroHelmet && body.type == ItemID.NecroBreastplate && legs.type == ItemID.NecroGreaves)
                return "BON_NecroSet";

            if (head.type == ItemID.AnglerHat && body.type == ItemID.AnglerVest && legs.type == ItemID.AnglerPants)
                return "BON_AnglerSet";

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "BON_NecroSet")
            {
                player.GetModPlayer<BagOfNonsenseSetBonuses>().equippedNecroSet = true;
                player.setBonus += "\n" + Language.GetTextValue("Mods.BagOfNonsense.Sets.NecroSetExtra.Description");
            }
            if (set == "BON_AnglerSet")
            {
                player.GetModPlayer<BagOfNonsenseSetBonuses>().equippedAnglerSet = true;
                player.setBonus = "Increases fishing skill by 25\nGrants fishing buffs\nShoot extra bobbers\nChance to get extra crates";
                player.fishingSkill += 25;
                player.accFishingLine = true;
                player.accFishFinder = true;
                player.AddBuff(BuffID.Crate, 2);
                player.AddBuff(BuffID.Calm, 2);
                player.AddBuff(BuffID.PeaceCandle, 2);
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<BagOfNonsenseSetBonuses>().equippedAnglerSet && item.fishingPole > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectileDirect(source, position, velocity * Main.rand.NextFloat(.95f, 1.05f), type, damage, knockback, player.whoAmI);
                }
                return true;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode && item.CountsAsClass(DamageClass.Melee))
            {
                if (!Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        BiomeInformations biome = new()
                        {
                            Player = player
                        };
                        biome.Update();
                        int dusty = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Scorpion, item.direction * 2, 0.0f, 150, biome.Color, 1f);
                        var dust = Main.dust[dusty];
                        Vector2 vector2 = Vector2.Multiply(dust.velocity, 0.2f);
                        dust.velocity = vector2;
                        Main.dust[dusty].noGravity = true;
                    }
                }
            }
        }
    }
}