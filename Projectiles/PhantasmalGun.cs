using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BagOfNonsense.Projectiles
{
    public class PhantasmalGun : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantasmal Missile");
        }

        public override void SetDefaults() => Projectile.CloneDefaults(ProjectileID.VortexBeater);

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = 0;
            return true;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.ai[0] += 1f;
            int num33 = 0;
            if (Projectile.ai[0] >= 40f) num33++;
            if (Projectile.ai[0] >= 80f) num33++;
            if (Projectile.ai[0] >= 120f) num33++;
            int num34 = 5;
            int num35 = 0;
            Projectile.ai[1] -= 1f;
            bool flag13 = false;
            int num36 = -1;
            if (Projectile.ai[1] <= 0f)
            {
                Projectile.ai[1] = num34 - num35 * num33;
                flag13 = true;
                int num37 = (int)Projectile.ai[0] / (num34 - num35 * num33);
                if (num37 % 7 == 0) num36 = 0;
            }

            Projectile.frameCounter += 1 + num33;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            if (flag13 && Main.myPlayer == Projectile.owner)
            {
                bool flag14 = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed;
                int num38 = 14;
                float scaleFactor9 = 14f;
                int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                float weaponKnockback = player.inventory[player.selectedItem].knockBack;
                if (flag14)
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out num38, out scaleFactor9, out weaponDamage, out weaponKnockback, out num38, flag14);
                    weaponKnockback = player.GetWeaponKnockback(player.inventory[player.selectedItem], weaponKnockback);
                    float scaleFactor10 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    Vector2 value16 = vector;
                    Vector2 value17 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - value16;
                    if (player.gravDir == -1f)
                    {
                        value17.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - value16.Y;
                    }

                    Vector2 vector18 = Vector2.Normalize(value17);
                    if (float.IsNaN(vector18.X) || float.IsNaN(vector18.Y))
                    {
                        vector18 = -Vector2.UnitY;
                    }

                    vector18 *= scaleFactor10;
                    vector18 = vector18.RotatedBy(Main.rand.NextDouble() * 0.13089969754219055 - 0.065449848771095276, default);
                    if (vector18.X != Projectile.velocity.X || vector18.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }

                    Projectile.velocity = vector18;

                    for (int m = 0; m < 1; m++)
                    {
                        Vector2 spinningpoint2 = Vector2.Normalize(Projectile.velocity) * scaleFactor9;
                        spinningpoint2 = spinningpoint2.RotatedBy(Main.rand.NextDouble() * 0.09634954631328583 - 0.0098174773156642914, default);
                        if (float.IsNaN(spinningpoint2.X) || float.IsNaN(spinningpoint2.Y))
                        {
                            spinningpoint2 = -Vector2.UnitY;
                        }
                        Projectile.NewProjectile(player.GetSource_FromThis(), value16.X, value16.Y, spinningpoint2.X, spinningpoint2.Y, num38, weaponDamage, weaponKnockback, Projectile.owner, 0f, 0f);

                        Vector2 speeen = Vector2.Normalize(Projectile.velocity) * scaleFactor9;
                        speeen = speeen.RotatedBy(Main.rand.NextDouble() * 0.09634954631328583 - 0.0098174773156642914, default);
                        if (float.IsNaN(speeen.X) || float.IsNaN(speeen.Y))
                        {
                            speeen = -Vector2.UnitY;
                        }
                        int random;
                        if ((Projectile.ai[0] - 1) % 25 == 0)
                        {
                            random = ProjectileID.VortexBeaterRocket;
                            weaponDamage += Main.rand.Next(150, 250);
                        }
                        else
                        {
                            random = num38;
                        }
                        var proj = Projectile.NewProjectile(player.GetSource_FromThis(), value16.X, value16.Y, speeen.X, speeen.Y, random, weaponDamage, weaponKnockback, Projectile.owner, 0f, 0f);
                        Main.projectile[proj].noDropItem = true;
                    }

                    if (num36 == 0)
                    {
                        num38 = 616;
                        scaleFactor9 = 8f;
                        for (int n = 0; n < 1; n++)
                        {
                            Vector2 spinningpoint3 = Vector2.Normalize(Projectile.velocity) * scaleFactor9;
                            spinningpoint3 = spinningpoint3.RotatedBy(Main.rand.NextDouble() * 0.39269909262657166 - 0.19634954631328583, default);
                            if (float.IsNaN(spinningpoint3.X) || float.IsNaN(spinningpoint3.Y))
                            {
                                spinningpoint3 = -Vector2.UnitY;
                            }
                        }
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }
    }
}