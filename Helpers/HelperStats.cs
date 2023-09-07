using BagOfNonsense.Projectiles;
using BagOfNonsense.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense
{
    /// <summary>
    /// Useful random stuff
    /// </summary>
    internal static class HelperStats
    {
        private static int
            KnifeDustEffect;

        /// <summary>
        /// Muzzleflash effect, has 6 frames
        /// </summary>
        public static Texture2D MuzzleFlash => ModContent.Request<Texture2D>("BagOfNonsense/Assets/AR2MuzzleFlashFixed").Value;

        public static int RandomCyanDust => Utils.SelectRandom(Main.rand, 226, 300);

        /// <summary>
        /// Grenade Smoke
        /// </summary>
        public static int GrenadeGore => Main.rand.Next(61, 64);

        /// <summary>
        /// Dust that looks smokey
        /// </summary>
        public static int SmokeyDust => Utils.SelectRandom(Main.rand, DustID.Smoke, DustID.Torch);

        /// <summary>
        /// Goes from 0f to 1f slowly, MP friendly (probably?)
        /// </summary>
        public static float GlobalTick => Main.GameUpdateCount % 1500 / 1500f;

        public static void Fire(Vector2 position, int amount, float scale, float magnitudeRange, float rotationRange = MathHelper.TwoPi)
        {
            for (int i = 0; i < amount; ++i)
            {
                Vector2 velocity = Utils.RandomVector2(Main.rand, -magnitudeRange, magnitudeRange).RotatedByRandom(rotationRange);
                Dust.NewDustDirect(position, 0, 0, SmokeyDust, 0, 0, Scale: scale).velocity = velocity;
            }
        }

        public static void Smoke(IEntitySource source, Vector2 position, int amount, float magnitudeRange, float rotationRange = MathHelper.TwoPi)
        {
            for (int i = 0; i < amount; ++i)
            {
                Vector2 velocity = Utils.RandomVector2(Main.rand, -magnitudeRange, magnitudeRange).RotatedByRandom(rotationRange);
                Gore.NewGore(source, position, velocity, GrenadeGore);
            }
        }

        ///<summary>
        ///Returns a vector pointing from a source, to a target, with a speed.
        ///Simplifies basic projectile, enemy dash, etc aiming calculations to a single call.
        ///If "ballistic" is true it adjusts for gravity. Default is 0.1f, may be stronger or weaker for some projectiles though.
        ///</summary>
        ///<param name="source">The start point of the vector</param>
        ///<param name="target">The end point it is aiming towards</param>
        ///<param name="speed">The length of the resulting vector</param>
        public static Vector2 GenerateTargetingVector(Vector2 source, Vector2 target, float speed)
        {
            Vector2 distance = target - source;
            distance.Normalize();
            return distance * speed;
        }

        /// <summary>
        /// Finds said item type in the player inventory
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item FindItemInInventory(this Player player, int type)
        {
            Item Item = null;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item p = player.inventory[i];
                if (p != null && !p.IsAir && p.active && p.type == type)
                {
                    Item = p;
                }
            }
            return Item;
        }

        /// <summary>
        /// Returns the index of a nearby NPC
        /// </summary>
        /// <param name="pos">The center position</param>
        /// <param name="dist">The distance</param>
        /// <returns></returns>
        public static int FindNearestNPC(Vector2 pos, float dist)
        {
            int npcIndex = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && pos.Distance(npc.Center) < dist && Collision.CanHitLine(pos, 1, 1, npc.Center, 1, 1))
                {
                    npcIndex = i;
                }
            }
            return npcIndex;
        }

        /// <summary>
        /// Returns the index of a player
        /// </summary>
        /// <param name="pos">The center position</param>
        /// <param name="dist">The distance</param>
        /// <returns></returns>
        public static int FindNearestPlayer(Vector2 pos, float dist)
        {
            int playerIndex = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && pos.DistanceSQ(player.Center) < dist * dist)
                {
                    playerIndex = i;
                }
            }
            return playerIndex;
        }

        public static Vector2 GetDrawOrigin(Texture2D texture)
        {
            return texture.Size() * 0.5f;
        }

        /// <summary>
        /// Returns the amount of active dust in this world
        /// </summary>
        /// <returns></returns>
        public static int GetDustAmount()
        {
            int amount = 0;
            for (int i = 0; i < Main.maxDust; i++)
            {
                Dust dust = Main.dust[i];
                if (dust.active)
                {
                    amount++;
                }
            }
            return amount;
        }

        /// <summary>
        /// Returns the amount of active projectiles in this world
        /// </summary>
        /// <returns></returns>
        public static int GetProjectileAmount()
        {
            int amount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active)
                {
                    amount++;
                }
            }
            return amount;
        }

        private static Dust QuickDust(Vector2 pos, Color color, int type)
        {
            Dust obj = Dust.NewDustPerfect(pos, type, Vector2.Zero, 0, color, 1f);
            obj.position = pos;
            obj.velocity = Vector2.Zero;
            obj.fadeIn = 1f;
            obj.noLight = true;
            obj.noGravity = true;
            obj.color = color;
            return obj;
        }

        /// <summary>
        /// Draws dust line
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="dustAmount"></param>
        /// <param name="dustType"></param>
        public static void DustLine(Vector2 start, Vector2 end, float splits, Color color, int type)
        {
            QuickDust(start, color, type).scale = 1f;
            QuickDust(end, color, type).scale = 1f;
            float num = 1f / splits;
            for (float num2 = 0f; num2 < 1f; num2 += num)
            {
                QuickDust(Vector2.Lerp(start, end, num2), color, type).scale = 1f;
            }
        }

        /// <summary>
        /// Finds the nearest enemy projectile
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static int FindNearestHostileProj(float radius, Player player)
        {
            int foundProj = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.hostile && projectile.active && projectile.Center.DistanceSQ(player.Center) <= radius * radius)
                {
                    foundProj = projectile.identity;
                }
            }
            return foundProj;
        }

        /// <summary>
        /// Find and return the projectile index of this projectile type. Needs a player so owner check can be made.
        /// </summary>
        /// <param name="player">Owner</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int FindProjectileIndex(Player player, int type)
        {
            int proj = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.type == type)
                {
                    proj = p.identity;
                }
            }
            return proj;
        }

        public static void KnifeDust(Vector2 position, Projectile proj, int randomNum)
        {
            switch (randomNum)
            {
                case 0:
                    KnifeDustEffect = DustID.Shadowflame;
                    Lighting.AddLight(proj.Center, Color.Purple.ToVector3());
                    break;

                case 1:
                    KnifeDustEffect = DustID.SomethingRed;
                    Lighting.AddLight(proj.Center, Color.Red.ToVector3());
                    break;

                case 2:
                    KnifeDustEffect = DustID.MagicMirror;
                    Lighting.AddLight(proj.Center, Color.AliceBlue.ToVector3());
                    break;
            }
            if (KnifeDustEffect > 0)
            {
                Dust dust = Dust.NewDustDirect(position, proj.width / 2, proj.height, KnifeDustEffect, proj.velocity.X, proj.velocity.Y, 100, default, 1);
                dust.velocity *= (0.25f * Main.rand.NextFloat());
                dust.position -= proj.velocity * 0.5f;
                dust.scale += Main.rand.Next(150) * 0.001f;
                if (Main.rand.NextBool(5))
                    dust.noGravity = true;
            }
        }

        /// <summary>
        /// Gets the index of the NPC that's not the one in the param. This one requires line of sight.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int FindNextNPC(Projectile proj, NPC target, float distance)
        {
            int npc = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC targetNext = Main.npc[i];
                if (targetNext.whoAmI != target.whoAmI && targetNext.active && !targetNext.friendly && targetNext.CanBeChasedBy() && !targetNext.CountsAsACritter && proj.DistanceSQ(targetNext.Center) <= distance * distance && Collision.CanHitLine(proj.Center, 1, 1, targetNext.Center, 1, 1))
                {
                    npc = targetNext.whoAmI;
                }
            }
            return npc;
        }

        /// <summary>
        /// Returns NPC index from projectile center
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public static int FindTargetProjectileCenter(Projectile proj, float maxRange)
        {
            float close = maxRange * maxRange;
            int result = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                bool canBeChased = target.CanBeChasedBy();
                if (proj.localNPCImmunity[i] != 0)
                {
                    canBeChased = false;
                }
                if (!canBeChased)
                {
                    continue;
                }
                else
                {
                    float distance = proj.Center.DistanceSQ(target.Center);
                    if (distance < close)
                    {
                        close = MathHelper.Min(distance, close);
                        result = target.whoAmI;
                    }
                }
            }
            return result;
        }

        /// <summary> Returns a NPC index within the range of maxRange. This one does NOT required line of sight.</summary>>
        public static int FindTargetNoLOS(Projectile proj, float maxRange)
        {
            float num = maxRange;
            int result = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                bool canBeChased = nPC.CanBeChasedBy();
                if (proj.localNPCImmunity[i] != 0)
                {
                    canBeChased = false;
                }
                if (canBeChased)
                {
                    Player player = Main.player[proj.owner];
                    float num2 = player.Distance(nPC.Center);
                    if (num2 < num)
                    {
                        num = MathHelper.Min(num2, num);
                        result = i;
                    }
                }
            }
            return result;
        }

        /// <summary> Returns a NPC index within the range of maxRange. This one requires line of sight from player center.</summary>>
        public static int FindTargetLOSPlayer(Projectile proj, float maxRange, Player player)
        {
            float num = maxRange;
            int result = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                bool flag = nPC.CanBeChasedBy();
                if (proj.localNPCImmunity[i] != 0)
                {
                    flag = false;
                }
                if (flag)
                {
                    float num2 = player.Distance(nPC.Center);
                    if (num2 < num && Collision.CanHitLine(player.Center, 1, 1, nPC.Center, 1, 1))
                    {
                        num = MathHelper.Min(num2, num);
                        result = i;
                    }
                }
            }
            return result;
        }

        /// <summary> Returns a NPC index within the range of maxRange. This one requires line of sight from Projectile center.</summary>>
        public static int FindTargetLOSProjectile(Projectile proj, float maxRange)
        {
            float num = maxRange;
            int result = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC nPC = Main.npc[i];
                bool flag = nPC.CanBeChasedBy();
                if (proj.localNPCImmunity[i] != 0)
                {
                    flag = false;
                }
                if (flag)
                {
                    float num2 = proj.Distance(nPC.Center);
                    if (num2 < num && Collision.CanHitLine(proj.Center, 1, 1, nPC.Center, 1, 1))
                    {
                        num = MathHelper.Min(num2, num);
                        result = i;
                    }
                }
            }
            return result;
        }

        public static bool GhostlyArrowCheck(Projectile proj)
        {
            return proj.arrow
                && !proj.noEnchantments
                && proj.type != ProjectileID.PhantasmArrow
                && proj.type != ModContent.ProjectileType<DoomArrowEX1>()
                && proj.type != ModContent.ProjectileType<DoomArrowEX2>()
                && proj.type != ModContent.ProjectileType<DoomArrowEX3>()
                && proj.type != ModContent.ProjectileType<DoomArrowEX4>();
        }

        public static bool ChameleonProjCheck(Projectile projectile, Player owner)
        {
            int[] projectiles =
                {
                     ProjectileID.PhantasmArrow,
                     ModContent.ProjectileType<PhantasmalGun>(),
                     ModContent.ProjectileType<PhantasmalCelebrationArrow>(),
                     ModContent.ProjectileType<SummonedSword>(),
                     ModContent.ProjectileType<DoomArrowEX1>(),
                     ModContent.ProjectileType<DoomArrowEX2>(),
                     ModContent.ProjectileType<DoomArrowEX3>(),
                     ModContent.ProjectileType<DoomArrowEX4>(),
                     ModContent.ProjectileType<GhostlyArrowProj>(),
                     ModContent.ProjectileType<SunRay>()
                };
            if (!projectiles.Contains(projectile.type)
                && !projectile.npcProj
                && !projectile.hostile
                && !projectile.minion
                && ProjectileID.Sets.MinionShot[projectile.type] != true
                && !projectile.sentry
                && Main.projPet[projectile.type] != true
                && !projectile.CountsAsClass(DamageClass.Summon)
                && projectile.damage > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary> Returns true if it's either a blue star or a red star, used in Prismatic Dreams. </summary>>

        public static bool IsAStarSummon(Projectile projectile)
        {
            int[] stars = { ModContent.ProjectileType<PrismStarRed>(), ModContent.ProjectileType<PrismStarBlue>() };
            if (stars.Contains(projectile.type)) return true;
            return false;
        }

        /// <summary> Returns how many blue stars are active </summary>>

        public static int GetBlueStarCount(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PrismStarBlue>()];
        }
    }
}