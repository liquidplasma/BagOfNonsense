using BagOfNonsense.Buffs;
using BagOfNonsense.Dusts;
using BagOfNonsense.Items.Others;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.NPCs.Boss.MagicalCube
{
    //ported from my tAPI mod because I'm lazy

    public class MagicalCube : ModNPC
    {
        private static int hellLayer
        {
            get
            {
                return Main.maxTilesY - 200;
            }
        }

        private const int sphereRadius = 300;

        private int subCool
        {
            get
            {
                return (int)NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = (float)value;
            }
        }

        private float coolDown
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        private float rotationSpeed
        {
            get
            {
                return NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        private float currentMove
        {
            get
            {
                return NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        private Player Player => Main.player[NPC.target];

        private int moveTime = 300;
        private int moveTimer = 60;
        private bool currentlyImmune = false;
        private int lastStage = 0;
        internal int laserTimer = 0;
        internal int laser1 = -1;
        internal int laser2 = -1;
        private Vector2 targetPos;
        private int stage;
        private int[] receivedDamage = new int[5];

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magical Cube");
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                BuffID.Confused, // Most NPCs have this
		        BuffID.Poisoned,
                ModContent.BuffType<DColdtouch>(),
                ModContent.BuffType<DGreenRot>(),
                ModContent.BuffType<DHighwattage>(),
                ModContent.BuffType<DJarate>(), // Modded buffs
	             }
            };
            NPCID.Sets.DebuffImmunitySets[Type] = debuffData;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 200000;
            NPC.damage = 45;
            NPC.defense = 40;
            NPC.knockBackResist = 0f;
            NPC.width = 208;
            NPC.height = 208;
            Main.npcFrameCount[NPC.type] = 1;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 15f;
            NPC.boss = true;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.npcSlots = 10f;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SmartDrag");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = 300000;
            NPC.damage = 60;
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
            {
                NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            coolDown--;

            if (coolDown <= 0)
            {
                if (!currentlyImmune)
                {
                    currentlyImmune = true;
                    currentMove = 0;
                    coolDown = 120;
                    if (ClearNebula(Player))
                    {
                        int a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(-2f, -2f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(-2f, 2f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(2f, -2f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(2f, 2f);

                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(0f, 2f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(0f, -2f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(2f, 0f);
                        a = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        Main.dust[a].velocity = new Vector2(-2f, 0f);
                        Player.statMana = Player.statManaMax2 / 2;
                    }
                }
                else
                {
                    currentlyImmune = false;
                    getNextMove();
                    subCool = 60;
                }
            }
            NPC.rotation += rotationSpeed;
            if (!Player.active || Player.dead || Player.position.Y < hellLayer)
            {
                NPC.TargetClosest(false);
                if (!Player.active || Player.dead || Player.position.Y < hellLayer)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            Player.AddBuff(BuffID.MoonLeech, 2, true);

            if ((Player.statDefense > 200 || Player.lifeRegen > 200) && Player.name != "Tester")
            {
                Main.NewText("Cheating will not help.", 255, 32, 32);
                Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " was judged"), 9999, -Player.direction);
                BitLightning(Player.Center);
            }

            if (currentlyImmune)
            {
                if (rotationSpeed > 0f)
                {
                    rotationSpeed -= 0.005f;
                }
                NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 100, (Player.Center.Y - NPC.Center.Y) / 100);
            }
            else
            {
                if (rotationSpeed < 0.1f)
                {
                    rotationSpeed += 0.01f;
                }
            }

            UpdateStage();

            if (stage == 1 && lastStage == 0)
            {
                /*Main.NewText("The real battle begins.", 255, 32, 32);*/
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Commando");
            }
            if (stage == 2 && lastStage != 2)
            {
                /*Main.NewText("You feel the cube's strength growing.", 255, 0, 0);*/
                NPC.damage += 12;
                NPC.defense += 12;
            }

            // -------------------------------------------------------------------------- ATTACKS
            subCool--;
            if (currentMove == 1)
            {
                rotationSpeed = 0.2f;
                if (subCool <= 0 && stage >= 2)
                {
                    NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 30, (Player.Center.Y - NPC.Center.Y) / 30);
                }
            }
            if (currentMove == 2)
            {
                NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 200, (Player.Center.Y - NPC.Center.Y) / 200);
                BitStorm(Player.Center);
            }
            if (currentMove == 3)
            {
                NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 150, (Player.Center.Y - NPC.Center.Y) / 150);
                BitBeam(Player.Center);
            }
            if (currentMove == 4)
            {
                NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 300, (Player.Center.Y - NPC.Center.Y) / 300);
                rotationSpeed = 0.01f;
                if (subCool <= 0)
                {
                    BitExplosion(Player.Center);
                }
            }
            if (currentMove == 5)
            {
                NPC.velocity = new Vector2((Player.Center.X - NPC.Center.X) / 250, (Player.Center.Y - NPC.Center.Y) / 250);
                rotationSpeed = 0.02f;
                if (subCool == 25)
                {
                    targetPos = Player.Center;
                    for (int i = 0; i < 20; i++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), 5, 5, ModContent.DustType<Neon>(), 0f, 0f, 0, default(Color), 1f);
                        int x = 0;
                        int y = 0;
                        while (x == 0 && y == 0)
                        {
                            x = Main.rand.Next(-6, 6);
                            y = Main.rand.Next(-6, 6);
                        }
                        Main.dust[dust].velocity = new Vector2(x, y);
                        Main.dust[dust].color = new Color(0, 255, 255);
                    }
                }
                if (subCool <= 0)
                {
                    Vector2 sum = Vector2.Zero;
                    sum.X = Main.rand.Next(-120, 121);
                    BitLightning(targetPos + sum);
                    subCool = 30;
                }
            }
            if (subCool <= 0)
            {
                subCool = 60;
            }
            // -------------------------------------------------------------------------- ATTACKS

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
                NPC.netUpdate = true;
            }
            lastStage = stage;
        }

        private bool ClearNebula(Player player)
        {
            bool cleared = false;
            for (int i = 0; i < 22; i++)
            {
                int t = player.buffType[i];
                if (t == BuffID.NebulaUpLife1 || t == BuffID.NebulaUpLife2 || t == BuffID.NebulaUpLife3 || t == BuffID.NebulaUpDmg1 || t == BuffID.NebulaUpDmg2 || t == BuffID.NebulaUpDmg3)
                {
                    player.DelBuff(i);
                    cleared = true;
                }
            }
            return cleared;
        }

        private void BitLightning(Vector2 pos)
        {
            int damage = NPC.damage;
            if (Main.expertMode)
            {
                damage = (int)(damage * 1.1f);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X + Main.rand.Next(-12, 13), 50, 0, 10, ModContent.ProjectileType<CyberInsta>(), damage, 0, Main.myPlayer);
                }
            }
        }

        private void BitStorm(Vector2 pos)
        {
            for (int i = -5; i <= 5; i++)
            {
                Dust.NewDust(new Vector2(pos.X - (i * 15f), pos.Y - 300f + Main.rand.Next(-16, 17)), 12, 16, Utils.SelectRandom(Main.rand, ModContent.DustType<Binary0>(), ModContent.DustType<Binary1>()), 0f, -0.5f, 0, default(Color), 1f);
            }
            int fallSpeed = 10;
            if (Main.expertMode)
            {
                fallSpeed = 15;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int a2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X - (Main.rand.Next(-5, 6) * 15f), pos.Y - 300f + Main.rand.Next(-16, 17), 0, fallSpeed, ModContent.ProjectileType<CyberBoss>(), (int)(NPC.damage * 0.6f), 0, Main.myPlayer);
                Main.projectile[a2].timeLeft = 45;
            }
        }

        private void BitBeam(Vector2 pos)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vector2 = NPC.Center;
                float num200 = (float)pos.X - vector2.X;
                float num201 = (float)pos.Y - vector2.Y;
                num200 += (float)Main.rand.Next(-40, 41) * 0.4f;
                num201 += (float)Main.rand.Next(-32, 33) * 0.4f;
                Vector2 vector12 = vector2 + Vector2.Normalize(new Vector2(num200, num201).RotatedBy((double)(-1.57079637f * (float)NPC.direction), default(Vector2))) * 6f;
                int a2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector12.X, vector12.Y, num200, num201, ModContent.ProjectileType<CyberBoss>(), (int)(NPC.damage * 0.7f), 0, Main.myPlayer);
                Main.projectile[a2].tileCollide = true;
                Main.projectile[a2].timeLeft = 180;
            }
        }

        private void BitExplosion(Vector2 pos)
        {
            int range = 16, count = 50;
            if (Main.expertMode)
            {
                range = 24;
                count = 70;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < count; i++)
                {
                    int a2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Main.rand.Next(-range, range + 1), Main.rand.Next(-range, range + 1), ModContent.ProjectileType<CyberBoss>(), (int)(NPC.damage * 0.7f), 0, Main.myPlayer);
                    Main.projectile[a2].tileCollide = true;
                    Main.projectile[a2].timeLeft = 185;
                }
            }
        }

        private Vector2 getAttackPosition()
        {
            Player player = Main.player[NPC.target];
            return new Vector2(player.Center.X, player.Center.Y);
        }

        private void getNextMove()
        {
            targetPos = Main.player[NPC.target].Center;
            int nextAtk = Main.rand.Next(0, 101);
            if (nextAtk <= 30 || stage == 0)
            {
                currentMove = 1;
                coolDown = 120;
                int divisor = 45;
                if (stage >= 1)
                {
                    divisor = 30;
                    coolDown = 90;
                }
                NPC.velocity = new Vector2((targetPos.X - NPC.Center.X) / divisor, (targetPos.Y - NPC.Center.Y) / divisor);
            }
            else if (nextAtk > 30 && nextAtk <= 50)
            {
                currentMove = 2;
                coolDown = 240;
            }
            else if (nextAtk > 50 && nextAtk <= 70)
            {
                currentMove = 3;
                coolDown = 120;
            }
            else if (nextAtk > 70)
            {
                if (stage == 2 && nextAtk > 85)
                {
                    currentMove = 5;
                    coolDown = 185;
                }
                else
                {
                    currentMove = 4;
                    coolDown = 180;
                }
            }
        }

        public override bool CheckDead()
        {
            if (!NPC.lavaImmune)
            {
                NPC.lavaImmune = true;
                NPC.life = NPC.lifeMax;
                return false;
            }
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)moveTime);
            writer.Write((short)moveTimer);
            if (Main.expertMode)
            {
                //
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            moveTime = reader.ReadInt16();
            moveTimer = reader.ReadInt16();
            if (Main.expertMode)
            {
                //
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
        }

        private void UpdateStage()
        {
            float hp = (float)(NPC.life);
            float hpM = (float)(NPC.lifeMax);
            float hpP = hp / hpM;
            if (!NPC.lavaImmune)
            {
                stage = 0;
            }
            else if (NPC.lavaImmune && hpP > 0.5f)
            {
                stage = 1;
            }
            else
            {
                stage = 2;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            if (currentlyImmune)
            {
                return Color.Red;
            }
            if (stage == 2)
            {
                return Color.Orange;
            }
            if (stage == 1)
            {
                return Color.White;
            }
            return null;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < hit.Damage / NPC.lifeMax * 100.0; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Neon>(), hit.HitDirection, -1f, 0, default, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.rand.NextBool(2))
            {
                target.AddBuff(ModContent.BuffType<DVirus>(), 180);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MagicalCubeTreasureBag>()));
        }

        public int GetHighestDamage()
        {
            int max = receivedDamage[0];
            int bestType = 0;
            for (int i = 1; i < 5; i++)
            {
                if (receivedDamage[i] > max)
                {
                    max = receivedDamage[i];
                    bestType = i;
                }
            }
            return bestType;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The " + NPC.TypeName;
            potionType = ItemID.SuperHealingPotion;
        }

        private int DropLoot(int x, int y, int w, int h, int itemId, int stack = 1, bool broadcast = false, int prefix = 0, bool nodelay = false)
        {
            return Item.NewItem(NPC.GetSource_Loot(), x, y, w, h, itemId, stack, broadcast, prefix, nodelay);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            float mult = 1f;
            if (item.CountsAsClass(DamageClass.Melee))
            {
                receivedDamage[0] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (item.CountsAsClass(DamageClass.Magic))
            {
                receivedDamage[1] += (int)modifiers.FinalDamage.Flat;
                mult = 0.6f;
            }
            else if (item.CountsAsClass(DamageClass.Ranged))
            {
                receivedDamage[2] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (item.CountsAsClass(DamageClass.Summon))
            {
                receivedDamage[3] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (item.CountsAsClass(DamageClass.Throwing))
            {
                receivedDamage[4] += (int)modifiers.FinalDamage.Flat;
                mult = 1.2f;
            }
            if (modifiers.FinalDamage.Flat >= 5000)
            {
                Main.NewText("Cheating will not help.", 255, 32, 32);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was judged"), 9999, -player.direction);
                BitLightning(player.Center);
                mult = 0f;
            }
            modifiers.FinalDamage *= mult;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            float mult = 1f;
            if (projectile.CountsAsClass(DamageClass.Melee))
            {
                receivedDamage[0] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (projectile.CountsAsClass(DamageClass.Magic))
            {
                receivedDamage[1] += (int)modifiers.FinalDamage.Flat;
                mult = 0.6f;
            }
            else if (projectile.CountsAsClass(DamageClass.Ranged))
            {
                receivedDamage[2] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (projectile.minion)
            {
                receivedDamage[3] += (int)modifiers.FinalDamage.Flat;
                mult = 0.75f;
            }
            else if (projectile.CountsAsClass(DamageClass.Throwing))
            {
                receivedDamage[4] += (int)modifiers.FinalDamage.Flat;
                mult = 1.2f;
            }
            if ((int)modifiers.FinalDamage.Flat >= 5000)
            {
                Main.NewText("Cheating will not help.", 255, 32, 32);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was judged"), 9999, -player.direction);
                BitLightning(player.Center);
                mult = 0f;
            }
            modifiers.FinalDamage *= mult;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (currentlyImmune)
            {
                modifiers.FinalDamage *= 0.1f;
                SoundEngine.PlaySound(SoundID.NPCHit3, NPC.position);
            }
            if (stage == 0)
            {
                modifiers.FinalDamage *= 4f;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 0f;
            return false;
        }
    }
}