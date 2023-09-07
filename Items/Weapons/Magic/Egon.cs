using BagOfNonsense.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Items.Weapons.Magic
{
    public class EgonModPLayer : ModPlayer
    {
        public float radiansControl = 0.1f;

        public override void PostUpdate()
        {
            if (Player.HeldItem.type == ModContent.ItemType<Egon>() && Player.channel)
                radiansControl += 0.125f;
        }
    }

    public class Egon : ModItem
    {
        private static SoundStyle Windup => new("BagOfNonsense/Sounds/Weapons/egon_windup")
        {
            Volume = 0.4f,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        private static SoundStyle Loop => new("BagOfNonsense/Sounds/Weapons/egon_loop")
        {
            Volume = 0.4f,
            IsLooped = true,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        private static SoundStyle End => new("BagOfNonsense/Sounds/Weapons/egon_end")
        {
            Volume = 0.4f,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
        };

        private ActiveSound loopActive, windupActive, endActive;
        private int timer = 0;
        private bool justPlayedLoop, justPlayedWindup = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Egon");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.damage = 22;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.LightPurple;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.channel = true;
            Item.mana = 6;
        }

        public override void HoldItem(Player player)
        {
            windupActive = SoundEngine.FindActiveSound(Windup);
            loopActive = SoundEngine.FindActiveSound(Loop);
            endActive = SoundEngine.FindActiveSound(End);

            if (!player.channel && timer > 0) timer--;

            if (player.channel && player.CheckMana(player.HeldItem.mana) && player.statMana > 0)
            {
                timer++;

                if (windupActive == null && timer <= 180 && !justPlayedWindup && !justPlayedLoop)
                {
                    justPlayedWindup = true;
                    SoundEngine.PlaySound(Windup, player.Center);
                }
                if (loopActive == null && timer >= 180 && !justPlayedLoop)
                {
                    windupActive?.Stop();
                    justPlayedWindup = false;
                    justPlayedLoop = true;
                    SoundEngine.PlaySound(Loop, player.Center);
                }

                if (windupActive != null) windupActive.Position = player.Center;
                if (loopActive != null) loopActive.Position = player.Center;

                endActive?.Stop();
            }
            if (!justPlayedLoop && !justPlayedWindup)
            {
                windupActive?.Stop();
                loopActive?.Stop();
            }
            if (timer == 0 || !player.CheckMana(player.HeldItem.mana) || player.statMana == 0)
            {
                justPlayedLoop = justPlayedWindup = false;
            }

            if (!player.channel)
            {
                if (endActive == null && timer != 0)
                {
                    SoundEngine.PlaySound(End, player.Center);
                    justPlayedLoop = justPlayedWindup = false;
                    timer = 2;
                }
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 5f);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 fixPos = player.Center + new Vector2(0, -8);
            position += fixPos.DirectionTo(Main.MouseWorld) * (player.direction > 0 ? 28f : 40f);
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile mainBeam = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<EgonProj>(), damage, knockback, player.whoAmI, 1);
            mainBeam.originalDamage = damage;
            Projectile swirlyBeam = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<EgonProj>(), damage, knockback, player.whoAmI);
            swirlyBeam.originalDamage = damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.MagicMissile)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}