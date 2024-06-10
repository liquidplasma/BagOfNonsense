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

        public int burningTime;

        private int timer;

        public bool CanBurn => burningTime > 0;
        public Player Player { get; set; }

        private static SoundStyle Hit => new("BagOfNonsense/Sounds/FireBurnHit/hit", 10)
        {
            Pitch = Main.rand.NextFloat(.8f, 1.2f),
            Volume = 0.1f
        };

        private static int DustType => Utils.SelectRandom(Main.rand, 259, 64, 158);

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            if (CanBurn)
                burningTime--;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Player != null && Player.active && CanBurn && npc.active && !npc.friendly)
            {
                drawColor = Color.IndianRed;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (Player != null && Player.active && CanBurn && !npc.friendly && npc.active)
            {
                timer++;
                Lighting.AddLight(npc.Center, Color.IndianRed.ToVector3());
                if (timer >= 30)
                {
                    timer = 0;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 randomVector = Utils.RandomVector2(Main.rand, -2, 2);
                        var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustType, randomVector.X, randomVector.Y, 100, default, 1f);
                        dust.velocity *= 1.2f + Utils.SelectRandom(Main.rand, .15f, .3f, .45f) * Utils.SelectRandom(Main.rand, -1, 1);
                    }
                    int hitDamage = (20 * (Player.HasFireBirdUpgrade() ? 5 : 1)) + Main.rand.Next(Player.HasFireBirdUpgrade() ? 84 : 21);
                    if (Player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.useAmmo), npc.Center, Vector2.Zero, ModContent.ProjectileType<UfoMissileBits>(), hitDamage, 2f, Player.whoAmI, 1);
                    SoundEngine.PlaySound(Hit, npc.Center);
                }
            }
        }
    }
}