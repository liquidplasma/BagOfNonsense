using BagOfNonsense.Buffs;
using BagOfNonsense.Helpers;
using BagOfNonsense.Items.Accessory;
using BagOfNonsense.Items.Armor;
using BagOfNonsense.NPCs;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    public class BagOfNonsenseModPlayer : ModPlayer
    {
        public bool
            SpawnArrow,
            SpawnArrowQuiver;

        private Item HeldItem => Player.HeldItem;

        public override void ResetEffects()
        {
            SpawnArrow = false;
            SpawnArrowQuiver = false;
        }

        /// <summary>
        /// Golden Trail
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        private static GhostlyArrowTrail Trail(Projectile proj)
        {
            return proj.GetGlobalProjectile<GhostlyArrowTrail>();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            Player projowner = Main.player[proj.owner];
            float GetKnockback = Player.GetTotalKnockback(proj.DamageType).ApplyTo(HeldItem.knockBack) * 0.1f;

            if (GetKnockback < 1f)
                GetKnockback = 1f;

            int spread = 22;
            if ((SpawnArrow || SpawnArrowQuiver)
                && HelperStats.GhostlyArrowCheck(proj)
                && Player.HeldItem.useAmmo == AmmoID.Arrow)
            {
                int randPos = Main.rand.Next(-256, 256);
                Vector2 random = projowner.position + projowner.Size * Main.rand.NextFloat() + new Vector2(randPos, -6400);
                if (Main.rand.NextBool())
                {
                    random = Player.position + Player.Size * Main.rand.NextFloat();
                    spread = 120;
                }
                Vector2 aim = (random.DirectionTo(target.Center) * 8f).RotatedByRandom(MathHelper.ToRadians(spread));

                float mult = 0.6f;
                if (SpawnArrowQuiver)
                    mult = 0.8f;

                int damageinc = (int)(damageDone * mult);
                if (Main.myPlayer == proj.owner)
                {
                    Projectile arrow = Projectile.NewProjectileDirect(proj.GetSource_Misc("GhostlyArrowBON"), random, aim, ModContent.ProjectileType<GhostlyArrowProj>(), damageinc, GetKnockback, Player.whoAmI, target.whoAmI);
                    Trail(arrow).ghostlyArrowTrailFlag = true;
                }
            }
        }
    }

    public class BagOfNonsenseSetBonuses : ModPlayer
    {
        public bool
            equippedAnglerSet,
            equippedNecroSet,
            chameleonMode;

        private int necroSetBonusControl;

        public override void ResetEffects()
        {
            chameleonMode = false;
            equippedNecroSet = false;
            equippedAnglerSet = false;
            if (necroSetBonusControl > 0)
                necroSetBonusControl--;
        }

        public override void ModifyCaughtFish(Item fish)
        {
            if (equippedAnglerSet && (ItemID.Sets.IsFishingCrate[fish.type] || ItemID.Sets.IsFishingCrateHardmode[fish.type]) && !fish.questItem)
                fish.stack += Main.rand.Next(1, 4);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (chameleonMode && !target.friendly)
            {
                target.AddBuff(ModContent.BuffType<DGreenRot>(), 240);
                target.GetGlobalNPC<NPCsDebuffLogic>().ChamaleonPlayerIndex = Player.whoAmI;
            }
        }

        private static BagOfNonsenseGlobalProjectile NecroArrow(Projectile proj)
        {
            return proj.GetGlobalProjectile<BagOfNonsenseGlobalProjectile>();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (equippedNecroSet && !target.friendly && !NecroArrow(proj).NecroArmorBoneArrow)
            {
                if (necroSetBonusControl == 0)
                    necroSetBonusControl = 2;
                Vector2 position = Player.position + Player.Size * Main.rand.NextFloat(0.5f);
                Vector2 velocity = position.DirectionTo(target.Center) * 16f;
                int newDamage = (int)MathHelper.Clamp(damageDone * 0.33f, 1, 50);
                if (Player.whoAmI == Main.myPlayer && !proj.GetGlobalProjectile<BagOfNonsenseGlobalProjectile>().NecroArmorBoneArrow && newDamage != 1 && necroSetBonusControl > 0)
                {
                    Projectile arrow = Projectile.NewProjectileDirect(Player.GetSource_FromThis("BON_NecroSet"), position, velocity, ProjectileID.BoneArrow, newDamage, 2f, Player.whoAmI);
                    arrow.rotation = arrow.AngleTo(target.Center) + MathHelper.PiOver2;
                    NecroArrow(arrow).NecroArmorBoneArrow = true;
                }
            }

            if (chameleonMode && !target.friendly)
            {
                target.AddBuff(ModContent.BuffType<DGreenRot>(), 240);
                target.GetGlobalNPC<NPCsDebuffLogic>().ChamaleonPlayerIndex = Player.whoAmI;
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<HiddenShooterHood>() && (Player.inventory[Player.selectedItem].CountsAsClass(DamageClass.Ranged) || Player.inventory[Player.selectedItem].CountsAsClass(DamageClass.Throwing)))
            {
                Player.scope = true;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (chameleonMode)
            {
                modifiers.DisableSound();
                SoundEngine.PlaySound(SoundID.NPCHit27 with
                {
                    Pitch = Main.rand.NextFloat(-0.25f, 0.25f)
                });
            }
        }
    }
}