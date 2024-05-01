using BagOfNonsense.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles.Pets
{
    public class SwordThrowerProjectiles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 12;
            // DisplayName.SetDefault("Sword");
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 1f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha <= 210) { return Color.White; }
            return base.GetAlpha(lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(12);
        }

        public override void AI()
        {
            Projectile.alpha -= 13;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Utils.RandomVector2(Main.rand, -6, 6) * Main.rand.NextFloat();
                Dust dusty = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.TintableDustLighted);
                dusty.velocity = velocity;
                dusty.scale *= Main.rand.NextFloat();
                Color lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
                dusty.color = lightColor;
            }
        }
    }

    public class SwordThrower : ModProjectile
    {
        private Player Player => Main.player[Projectile.owner];
        private Vector2 pos;

        private int npcIndex;

        private int shootStyle;

        private int delayShot;

        private float radiansShot;

        private NPC Target
        {
            get
            {
                if (Main.npc.IndexInRange(npcIndex))
                    return Main.npc[npcIndex];
                else
                    return null;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SwordSpirit");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 28;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        /*public override bool PreDraw(ref Color lightColor)
        {
            int shaderID = ContentSamples.ItemsByType[ItemID.BloodbathDye].dye;
            Main.instance.PrepareDrawnEntityDrawing(Projectile, shaderID);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rect = new(0, 0, texture.Width, texture.Height);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, lightColor, Projectile.rotation, rect.Size() / 2, Projectile.scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            return false;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            shootStyle = Main.rand.Next(1, 4);
            pos = Player.Center;
            npcIndex = (int)Projectile.ai[0];
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3());
            if (Target != null)
            {
                if (!Target.active || Target.friendly) Projectile.Kill();
                switch (shootStyle)
                {
                    case 1:
                        delayShot = 30 + Utils.SelectRandom(Main.rand, -6, 6);
                        radiansShot = 1;
                        pos = Target.Top + new Vector2(Main.rand.Next(-16, 16), Main.rand.Next(-296, -240));
                        break;

                    case 2:
                        delayShot = 30;
                        radiansShot = 15;
                        pos = Target.Top + new Vector2(Main.rand.Next(-16, 16), Main.rand.Next(-296, -240));
                        break;

                    case 3:
                        delayShot = 90;
                        radiansShot = 36;
                        pos = Target.Center;
                        break;
                }
                Projectile.ai[0]++;
                Projectile.SmoothHoming(pos, 0.1f, 8f);
                Projectile.rotation = Projectile.velocity.X * 0.04f;
                Projectile.ai[1] += Main.rand.Next(1, 3);
                if (Projectile.ai[1] >= delayShot)
                {
                    Projectile.ai[1] = 0;
                    Vector2 aim = Projectile.Center.DirectionTo(Target.Center) * 12f;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        float fixDamage = Projectile.damage * Player.GetTotalDamage(DamageClass.Melee).Additive;
                        if (fixDamage <= 0) fixDamage = 1;
                        switch (shootStyle)
                        {
                            case 1:
                                Projectile fast = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, aim.RotatedByRandom(MathHelper.ToRadians(radiansShot)), ModContent.ProjectileType<SwordThrowerProjectiles>(), (int)(fixDamage * 0.15f), Projectile.knockBack, Player.whoAmI);
                                break;

                            case 2:
                                for (int i = 0; i < 3; i++)
                                {
                                    Projectile shotgun = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, aim.RotatedByRandom(MathHelper.ToRadians(radiansShot)), ModContent.ProjectileType<SwordThrowerProjectiles>(), (int)(fixDamage * 0.33f), Projectile.knockBack, Player.whoAmI);
                                }
                                break;

                            case 3:
                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile circle = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, aim.RotatedBy(MathHelper.ToRadians(radiansShot * i)), ModContent.ProjectileType<SwordThrowerProjectiles>(), (int)(fixDamage * 0.1f), Projectile.knockBack, Player.whoAmI);
                                    circle.ArmorPenetration = Target.defense * 2;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}