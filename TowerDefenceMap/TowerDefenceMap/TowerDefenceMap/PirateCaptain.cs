using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;
using System.IO;

namespace TowerDefenceMap
{
    public class PirateCaptain: Pirate
    {
        List<Projectile> bullets;
        int shotcount;
        public PirateCaptain(Game1 game, Point startPosition)
            : base(game,startPosition, "Images/pirateCaptain_animated", PirateValues.pirateCaptainHealth, 1000, 3000, 2, PirateValues.pirateCaptainAttack, new Point(20,20),new Point(4,1))
        {
            shotcount = PirateValues.pirateCaptainShotCount;
            bullets = new List<Projectile>(shotcount*4);
            for (int i = 0; i < ((shotcount*4)-1); i++)
            {
                bullets.Add(new Projectile(game, this.gridPosition, "Images/musketball", this));
            }
        }

            public override void Attack(Unit target)
            {
                game.soundBank.PlayCue("blunderbuss");
                int counter = 0;
                foreach (Projectile bullet in bullets)
                {
                    if (!bullet.Alive)
                    {
                        bullet.DeltaY = (float)Math.Sin(Math.Atan2(target.Position.Y - this.Position.Y + game.random.Next(30) - 15, target.Position.X - this.Position.X + game.random.Next(30) - 15)) * 4;
                        bullet.DeltaX = (float)Math.Cos(Math.Atan2(target.Position.Y - this.Position.Y + game.random.Next(30) - 15, target.Position.X - this.Position.X + game.random.Next(30) - 15)) * 4;
                        bullet.Alive = true;
                        bullet.Position = this.Position + new Vector2(frameSize.X / 2, frameSize.Y / 2);
                        counter += 1;
                    }
                    if (counter == shotcount)
                    {
                        break;
                    }
                }
                attackspeedCounter = 0;
                base.Attack(target);
            }

            public override void Update(GameTime gameTime)
            {
                foreach (Projectile bullet in bullets)
                {
                    bullet.Update(gameTime);
                }
                base.Update(gameTime);
            }

            public override void Draw()
            {
                foreach (Projectile bullet in bullets)
                {
                    bullet.Draw();
                }
                base.Draw();
            }

            public int ShotCount
            {
                get { return shotcount; }
            }
    }
}
