using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Minions
{
    //ported from my tAPI mod because I'm lazy
    public class Pacman : ModProjectile
    {
        internal int up = 0;

        internal int down = 1;

        internal int left = 2;

        internal int right = 3;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pacman");
            Main.projFrames[Projectile.type] = 3;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 10;
        }

        public override void AI()
        {
            // Check conditions
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
                modPlayer.pacMinion = false;
            }
            if (modPlayer.pacMinion)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.alpha = 0;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 80)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }

            // Pre-definitions
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
                if (npc.active && !npc.friendly && Projectile.Distance(npc.Center) < minDistance && Projectile.Distance(npc.Center) < 600f && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.CanBeChasedBy())
                {
                    if (npc.type == NPCID.MoonLordCore && eyesAlive)
                    {
                        continue;
                    }
                    target = i;
                    minDistance = Projectile.Distance(npc.Center);
                }
            }
            int thisId = 1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == Projectile.type)
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

            // AI
            Projectile.velocity = Vector2.Zero;
            if (target == -1 || Projectile.Distance(player.Center) > 800f)
            {
                if (Projectile.Distance(player.Center) > (float)(200f + ((thisId - 1) * 30f)))
                {
                    float distanceX = Math.Abs(player.Center.X - Projectile.Center.X);
                    float distanceY = Math.Abs(player.Center.Y - Projectile.Center.Y);
                    bool xFirst = false;
                    if (distanceX > distanceY)
                    {
                        xFirst = true;
                    }
                    if (xFirst)
                    {
                        CheckX(player.Center);
                        CheckY(player.Center);
                    }
                    else
                    {
                        CheckY(player.Center);
                        CheckX(player.Center);
                    }
                }
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.rotation = Projectile.DirectionTo(Projectile.Center + Projectile.velocity * 10).ToRotation();
                }
            }
            else
            {
                NPC npc = Main.npc[target];
                if (Projectile.Distance(npc.Center) > (float)(200f + ((thisId - 1) * 30f)))
                {
                    float distanceX = Math.Abs(npc.Center.X - Projectile.Center.X);
                    float distanceY = Math.Abs(npc.Center.Y - Projectile.Center.Y);
                    bool xFirst = false;
                    if (distanceX > distanceY)
                    {
                        xFirst = true;
                    }
                    if (xFirst)
                    {
                        CheckX(npc.Center);
                        CheckY(npc.Center);
                    }
                    else
                    {
                        CheckY(npc.Center);
                        CheckX(npc.Center);
                    }
                }
                else
                {
                    Projectile.rotation = Projectile.DirectionTo(npc.Center).ToRotation();
                    if (Projectile.frameCounter % 79 == 0 && Projectile.frame == 0)
                    {
                        if (Main.myPlayer == player.whoAmI)
                        {
                            int a2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center.X, npc.Center.Y, (Projectile.Center.X - npc.Center.X) / 30, (Projectile.Center.Y - npc.Center.Y) / 30, ModContent.ProjectileType<PacPellets>(), Projectile.damage, 0, player.whoAmI);
                            Main.projectile[a2].DamageType = DamageClass.Summon;
                            Main.projectile[a2].CritChance = 0;
                        }
                    }
                }
            }
        }

        public void TeleportToOrigin(Player player)
        {
            int type = Utils.SelectRandom(Main.rand, 15, 57, 58);
            Vector2 idlePosition = player.Center;
            Projectile.position = idlePosition;
            Projectile.velocity *= 0.1f;
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item8 with
            {
                Volume = 0.66f
            }, player.position);
        }

        private void GetRotation()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.DirectionTo(Projectile.Center - Projectile.velocity).ToRotation();
            }
        }

        private void CheckX(Vector2 pos)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                if (pos.X > Projectile.Center.X)
                {
                    Projectile.velocity.X++;
                }
                else if (pos.X < Projectile.Center.X)
                {
                    Projectile.velocity.X--;
                }
            }
        }

        private void CheckY(Vector2 pos)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                if (pos.Y > Projectile.Center.Y)
                {
                    Projectile.velocity.Y++;
                }
                else if (pos.Y < Projectile.Center.Y)
                {
                    Projectile.velocity.Y--;
                }
            }
        }

        private void SetDirection(int newDir)
        {
            Vector2 newVel = Vector2.Zero;
            switch (newDir)
            {
                case 0:
                    newVel = new Vector2(0, -1);
                    break;

                case 1:
                    newVel = new Vector2(0, 1);
                    break;

                case 2:
                    newVel = new Vector2(-1, 0);
                    break;

                case 3:
                    newVel = new Vector2(1, 0);
                    break;
            }
            Projectile.velocity = newVel;
        }
    }
}