using BagOfNonsense.Helpers;
using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class FireBurnGlobal : GlobalNPC
    {
        public bool burning;
        private int timer;
        public Player Player { get; set; }

        private static SoundStyle Hit => new("BagOfNonsense/Sounds/FireBurnHit/hit", 10)
        {
            Pitch = Main.rand.NextFloat(.8f, 1.2f),
            Volume = 0.1f
        };

        private int DustType => Utils.SelectRandom(Main.rand, 259, 64, 158);

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            burning = false;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (burning && npc.active && !npc.friendly)
            {
                drawColor = Color.IndianRed;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (burning && !npc.friendly && npc.active && Player != null)
            {
                timer++;
                Lighting.AddLight(npc.Center, Color.IndianRed.ToVector3());
                if (timer >= 30 && Player.whoAmI == Main.myPlayer)
                {
                    timer = 0;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 randomVector = Utils.RandomVector2(Main.rand, -2, 2);
                        var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustType, randomVector.X, randomVector.Y, 100, default, 1f);
                        dust.velocity *= 1.2f + Utils.SelectRandom(Main.rand, .15f, .3f, .45f) * Utils.SelectRandom(Main.rand, -1, 1);
                    }
                    int hitDamage = (20 * (Player.HasFireBirdUpgrade() ? 5 : 1)) + Main.rand.Next(Player.HasFireBirdUpgrade() ? 84 : 21);
                    Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), npc.Center, Vector2.Zero, ModContent.ProjectileType<UfoMissileBits>(), hitDamage, 2f, Player.whoAmI, 1);
                    SoundEngine.PlaySound(Hit, npc.Center);
                }
            }
        }
    }

    public class DFireBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Burn");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<FireBurnGlobal>().burning = true;
        }
    }
}