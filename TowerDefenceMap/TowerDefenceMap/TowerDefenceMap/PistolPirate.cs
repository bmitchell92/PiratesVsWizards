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
    public class PistolPirate: Pirate
    {
        List<Projectile> bullets;
        public PistolPirate(Game1 game, Point startPosition)
            : base(game,startPosition, "Images/pistolPirate_animated", PirateValues.pistolPirateHealth, 400, 1000, 4, PirateValues.pistolPirateAttack,new Point(20,20), new Point(8,1))
        {
            bullets = new List<Projectile>(20);
            for (int i = 0; i < 20; i++)
            {
                bullets.Add(new Projectile(game, this.gridPosition, "Images/musketball", this));
            }
        }

        public override void Attack(Unit target)
        {
            game.soundBank.PlayCue("pistol");
            foreach (Projectile bullet in bullets)
            {
                if (!bullet.Alive)
                {
                    bullet.DeltaY = (float)Math.Sin(Math.Atan2(target.Position.Y - this.Position.Y, target.Position.X - this.Position.X))*4;
                    bullet.DeltaX = (float)Math.Cos(Math.Atan2(target.Position.Y - this.Position.Y, target.Position.X - this.Position.X))*4;
                    bullet.Position = this.Position + new Vector2(frameSize.X / 2, frameSize.Y / 2);
                    bullet.Alive = true;
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
    }
}
