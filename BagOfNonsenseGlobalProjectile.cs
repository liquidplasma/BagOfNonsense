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
    public class BagOfNonsenseGlobalProjectile : GlobalProjectile
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
                if (player.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode)
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
                    && ChameleonPlayer.GetModPlayer<BagOfNonsenseSetBonuses>().chameleonMode
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
            if (NecroArmorBoneArrow)
                modifiers.DisableCrit();

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