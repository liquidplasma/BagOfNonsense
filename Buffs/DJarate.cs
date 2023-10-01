using BagOfNonsense.Items.Weapons.Ranged;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class DJarate : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jarate");
            // Description.SetDefault("Is this what I think it is......?");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<JarateDebuff>().CoatedJarate = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<JarateDebuffPlayer>().CoatedJarate = true;
        }
    }

    public class JarateDebuffPlayer : ModPlayer
    {
        public bool CoatedJarate = false;

        public override void ResetEffects()
        {
            CoatedJarate = false;
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (CoatedJarate)
            {
                drawInfo.colorArmorBody = Color.YellowGreen;
                drawInfo.colorArmorHead = Color.YellowGreen;
                drawInfo.colorArmorLegs = Color.YellowGreen;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (CoatedJarate)
            {
                modifiers.IncomingDamageMultiplier *= 1.35f;
                TF2Crit.MiniCritFX(Player);
                if (Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(Player.position, Player.width, Player.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                    }
                }
            }
            base.ModifyHurt(ref modifiers);
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (CoatedJarate)
            {
                modifiers.IncomingDamageMultiplier *= 1.35f;
                TF2Crit.MiniCritFX(Player);
                if (Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(Player.position, Player.width, Player.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                    }
                }
            }
            base.ModifyHitByNPC(npc, ref modifiers);
        }
    }

    public class JarateDebuff : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool CoatedJarate = false;
        private static Texture2D MiniCritTexture => ModContent.Request<Texture2D>("BagOfNonsense/Other/miniCrit", AssetRequestMode.ImmediateLoad).Value;

        public override void ResetEffects(NPC npc)
        {
            CoatedJarate = false;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (CoatedJarate)
            {
                Main.EntitySpriteDraw(MiniCritTexture,
                    npc.Top - new Vector2(0, 16) - Main.screenPosition,
                    new Rectangle(0, 0, MiniCritTexture.Width, MiniCritTexture.Height),
                    Color.White,
                    0,
                    MiniCritTexture.Size() * 0.5f,
                    1,
                    SpriteEffects.None,
                    0);
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (CoatedJarate)
            {
                drawColor = Color.Yellow;
                Lighting.AddLight(npc.Center, Color.Yellow.ToVector3());
                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Vector2 vector5 = npc.position;
                        vector5.X -= 2f;
                        vector5.Y -= 2f;
                        if (Main.rand.NextBool(2))
                        {
                            Dust dust12 = Dust.NewDustDirect(vector5, npc.width + 4, npc.height + 2, DustID.DesertWater2, 0f, 0f, 50, Color.Yellow, 0.8f);
                            if (Main.rand.NextBool(2))
                                dust12.alpha += 25;
                            if (Main.rand.NextBool(2))
                                dust12.alpha += 25;
                            dust12.noLight = true;
                            dust12.velocity *= 0.2f;
                            dust12.velocity.Y += 0.2f;
                            dust12.velocity += npc.velocity;
                        }
                    }
                }
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (CoatedJarate)
            {
                modifiers.FinalDamage *= 1.35f;
                if (npc.friendly)
                    modifiers.FinalDamage *= 5f;
                TF2Crit.MiniCritFX(npc);
                modifiers.Knockback *= 1.2f;
                if (Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                    }
                }
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (CoatedJarate)
            {
                Player player = Main.player[projectile.owner];
                if ((player.GetModPlayer<SashaCritTime>().critTimer > 0) && player.HeldItem.type == ModContent.ItemType<Sasha>())
                    return;

                modifiers.FinalDamage *= 1.35f;
                modifiers.Knockback *= 1.2f;
                if (npc.friendly)
                    modifiers.FinalDamage *= 5f;
                TF2Crit.MiniCritFX(npc);
                if (Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ichor, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 0, default);
                    }
                }
            }
        }
    }
}