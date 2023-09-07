using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    public abstract class InvaderAI : Minion
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Invader");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 10;
            CustomDefaults();
        }

        public virtual void CustomDefaults()
        {
        }

        public virtual void Attack()
        {
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 idlePosition = player.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1200f)
            {
                TeleportToOrigin(player);
            }
            PlayerChanges modPlayer = player.GetModPlayer<PlayerChanges>();
            if (player.dead)
            {
                modPlayer.invaderMinion = false;
            }
            if (modPlayer.invaderMinion)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.alpha = 0;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 80)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }
            Projectile.rotation = 0;
            int target = -1;
            float minDistance = 9999f;
            bool eyesAlive = false;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && (npc.type == NPCID.MoonLordHead || npc.type == NPCID.MoonLordHand) && !npc.dontTakeDamage)
                {
                    eyesAlive = true;
                    break;
                }
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && player.Distance(npc.Center) < minDistance && player.Distance(npc.Center) < 500f && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.CanBeChasedBy())
                {
                    if (npc.type == NPCID.MoonLordCore && eyesAlive)
                    {
                        continue;
                    }
                    target = i;
                    minDistance = player.Distance(npc.Center);
                }
            }
            int thisId = 1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.Name.Contains("Invader"))
                {
                    if (proj.identity != Projectile.identity)
                    {
                        thisId++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int invaders = GetTotalInvaders(player);

            if (target != -1)
            {
                NPC npc = Main.npc[target];
                if (npc.position.Y - (30 * thisId) < Projectile.Center.Y)
                {
                    Projectile.position.Y--;
                }
                else if (npc.position.Y - (30 * thisId) > Projectile.Center.Y)
                {
                    Projectile.position.Y++;
                }

                if (npc.Center.X < Projectile.Center.X)
                {
                    Projectile.position.X--;
                }
                else if (npc.Center.X > Projectile.Center.X)
                {
                    Projectile.position.X++;
                }

                if (Math.Abs(Projectile.Center.X - npc.Center.X) < 100)
                {
                    if (Projectile.frameCounter % 79 == 0 && Projectile.frame == 1)
                    {
                        Attack();
                    }
                }
            }
            else
            {
                thisId--;
                invaders--;
                int groupQuantity = (int)Math.Floor((float)invaders / 5f);
                if (player.Center.X - getFormPosition(thisId, groupQuantity).X < Projectile.Center.X)
                {
                    Projectile.position.X--;
                }
                else if (player.Center.X - getFormPosition(thisId, groupQuantity).X > Projectile.Center.X)
                {
                    Projectile.position.X++;
                }

                if (player.Center.Y - getFormPosition(thisId, groupQuantity).Y < Projectile.Center.Y)
                {
                    Projectile.position.Y--;
                }
                else if (player.Center.Y - getFormPosition(thisId, groupQuantity).Y > Projectile.Center.Y)
                {
                    Projectile.position.Y++;
                }
            }
        }

        public void TeleportToOrigin(Player player)
        {
            Vector2 idlePosition = player.Center;
            Projectile.position = idlePosition;
            Projectile.velocity *= 0.1f;
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item8 with
            {
                Volume = 0.66f
            }, player.position);
        }

        private Vector2 getFormPosition(int tId, int total)
        {
            Vector2 final;
            float ftId = (float)tId;
            int groupId = (int)Math.Floor(ftId / 5f);
            groupId = (int)Math.Floor((float)tId / 5f);
            int groupT = total;
            final.X = groupId * 30 - groupT * 15;
            final.Y = ((ftId % 5f) + 1) * 30;
            return final;
        }

        private int GetTotalInvaders(Player player)
        {
            return (player.ownedProjectileCounts[ModContent.ProjectileType<Invader1>()] + player.ownedProjectileCounts[ModContent.ProjectileType<Invader2>()] + player.ownedProjectileCounts[ModContent.ProjectileType<Invader3>()]);
        }
    }
}