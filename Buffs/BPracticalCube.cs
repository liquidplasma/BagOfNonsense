using BagOfNonsense.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace BagOfNonsense.Buffs
{
    public class BPracticalCube : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Practical Cube");
            // Description.SetDefault("'Still evil, but it is cute, so you pet it'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PlayerChanges>().practicalCube = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PracticalCube>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<PracticalCube>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}