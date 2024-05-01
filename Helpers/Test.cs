using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Helpers
{
    public sealed class AttackCyclingWeapon : ModItem
    {
        private static readonly List<AttackInfo> AttackInfo = new();

        private int _attackIndex;

        public override string Texture => "Terraria/Images/Item_" + ItemID.SpaceGun;

        public override void SetStaticDefaults()
        {
            AttackInfo.Add(new AttackInfo(ProjectileID.Beenade, 16, 10));
            AttackInfo.Add(new AttackInfo(ProjectileID.Grenade, 16, 12));
            AttackInfo.Add(new AttackInfo(ProjectileID.PartyGirlGrenade, 16, 5));
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SpaceGun);
        }

        public override bool CanUseItem(Player player)
        {
            _attackIndex = (_attackIndex + 1) % AttackInfo.Count;
            var current = AttackInfo[_attackIndex];
            Item.shoot = current.ProjectileType;
            Item.shootSpeed = current.ShootSpeed;
            Item.damage = current.Damage;
            return Item.shoot > ProjectileID.None;
        }
    }

    public readonly struct AttackInfo
    {
        public readonly int ProjectileType;

        public readonly float ShootSpeed;

        public readonly int Damage;

        public AttackInfo(int projectileType, float shootSpeed, int damage)
        {
            ProjectileType = projectileType;
            ShootSpeed = shootSpeed;
            Damage = damage;
        }
    }
}