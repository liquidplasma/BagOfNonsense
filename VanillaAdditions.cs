using BagOfNonsense.CoolStuff;
using BagOfNonsense.Helpers;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    public class VanillaModSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup evilmaterail = new(() => "Any evil material", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup("Any evil material", evilmaterail);
        }
    }

    public class VanillaAdditions : GlobalItem
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
            if (set == "BON_NecroSet") player.GetModPlayer<BONPlayer>().equippedNecroSet = true;
            if (set == "BON_AnglerSet")
            {
                player.GetModPlayer<BONPlayer>().equippedAnglerSet = true;
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
            if (player.GetModPlayer<BONPlayer>().equippedAnglerSet && item.fishingPole > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectileDirect(source, position, velocity * Main.rand.NextFloat(.95f, 1.05f), type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.GetModPlayer<BONPlayer>().chameleonMode && item.CountsAsClass(DamageClass.Melee))
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

    public class VanillaAdditionsProjs : GlobalProjectile
    {
        public bool
            SashaProjBool,
            NecroArmorBoneArrow;

        private int critTimerEffectTimer;
        public override bool InstancePerEntity => true;
        private int ChamaleonPlayerIndex;

        public Player ChameleonPlayer
        {
            get
            {
                if (Main.player.IndexInRange(ChamaleonPlayerIndex))
                    return Main.player[ChamaleonPlayerIndex];
                else
                    return null;
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player.IndexInRange(projectile.owner))
            {
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<BONPlayer>().chameleonMode)
                {
                    ChamaleonPlayerIndex = player.whoAmI;
                }
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            if (SashaProjBool)
            {
                critTimerEffectTimer++;
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<SashaCritTime>().critTimer > 0 && critTimerEffectTimer > 3)
                {
                    Dust critEffect = Dust.NewDustPerfect(projectile.Center, DustID.Firework_Red);
                    critEffect.noGravity = true;
                    critEffect.velocity = projectile.velocity * 0.1f * Main.rand.NextFloat();
                    critEffect.scale *= 0.4f;
                    critEffect.color = player.TeamColor();
                }
            }
            if (ChameleonPlayer != null)
            {
                if (ChameleonPlayer.active
                    && ChameleonPlayer.GetModPlayer<BONPlayer>().chameleonMode
                    && HelperStats.ChameleonProjCheck(projectile, ChameleonPlayer)
                    && !HelperStats.IsAStarSummon(projectile)
                    && projectile.alpha < 33
                    && projectile.velocity != Vector2.Zero
                    )
                {
                    BiomeInformations Biome = new()
                    {
                        Player = ChameleonPlayer
                    };
                    Biome.Update();
                    if (Main.rand.NextBool(2))
                    {
                        Dust dusty = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Scorpion, 0f, 0f, 100, Biome.Color, 0.3f);
                        dusty.scale = Main.rand.Next(1, 10) * 0.1f;
                        dusty.noGravity = true;
                        dusty.fadeIn = 1.1f;
                        dusty.velocity *= 0.75f * Main.rand.NextFloat(0.7f, 1f); ;
                    }
                }
            }
            return base.PreAI(projectile);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SashaProjBool)
            {
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<SashaCritTime>().critTimer > 0)
                    hit.HideCombatText = true;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (NecroArmorBoneArrow) modifiers.DisableCrit();
            if (SashaProjBool)
            {
                Player player = Main.player[projectile.owner];
                modifiers.DisableCrit();
                modifiers.DamageVariationScale *= 0f;
                float distance = Math.Clamp(1.85f - (target.Center.Distance(player.Center) / 1250f), 0.5f, 1.5f);
                if (player.GetModPlayer<SashaCritTime>().critTimer > 0)
                    modifiers.FinalDamage *= 3f;
                else
                    modifiers.FinalDamage *= distance;
            }
        }
    }
}