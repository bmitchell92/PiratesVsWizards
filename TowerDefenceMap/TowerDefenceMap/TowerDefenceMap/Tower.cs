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
    public class Tower : Unit
    {
        List<Projectile> bullets;

        public Tower(Game1 game, Point startPosition, int health, int attackSpeed, int range, int damage)
            : base(game, startPosition, "Images/tower", health, attackSpeed, range, damage,new Point(20,20), new Point(1,1))
        {
            bullets = new List<Projectile>(20);
            for (int i = 0; i < 20; i++)
            {
                bullets.Add(new Projectile(game, this.gridPosition, "Images/magicblast", this));
            }
        }

        public override void Attack(Unit target)
        {
            foreach (Projectile bullet in bullets)
            {
                if (!bullet.Alive)
                {
                    bullet.DeltaY = (float)Math.Sin(Math.Atan2(target.Position.Y - this.Position.Y, target.Position.X - this.Position.X)) * 4;
                    bullet.DeltaX = (float)Math.Cos(Math.Atan2(target.Position.Y - this.Position.Y, target.Position.X - this.Position.X)) * 4;
                    bullet.Position = this.Position + this.centerOfSprite;
                    bullet.Alive = true;
                    break;
                }
            }
            attackspeedCounter = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                foreach (Unit unit in game.pirateManager.pirates)
                {
                    if (this.attackRectangle.Intersects(unit.collisionRectangle) && unit.Alive)
                    {
                        if (attackspeedCounter >= attackSpeed)
                        {
                            Pirate pirate = (Pirate)unit;
                            if (game.wizardManager.withinBounds(gridPosition))
                            {
                                if (gridPosition.Y <19)
                                {
                                    if (game.mapManager.mapGrid[gridPosition.X, gridPosition.Y + 1].terrain == "land" || game.mapManager.mapGrid[gridPosition.X, gridPosition.Y + 1].terrain == "forest" || game.mapManager.mapGrid[gridPosition.X, gridPosition.Y + 1].terrain == "water")
                                    {
                                        if (pirate.UpdateDestinationPoint(new Point(gridPosition.X, gridPosition.Y + 1)))
                                        {
                                            pirate.UpdateDestinationPoint(new Point(gridPosition.X, gridPosition.Y + 1));
                                        }
                                    }
                                }
                                if (gridPosition.X <40)
                                {
                                    if (game.mapManager.mapGrid[gridPosition.X + 1, gridPosition.Y].terrain == "land" || game.mapManager.mapGrid[gridPosition.X + 1, gridPosition.Y].terrain == "forest" || game.mapManager.mapGrid[gridPosition.X + 1, gridPosition.Y].terrain == "water")
                                    {
                                        if (pirate.UpdateDestinationPoint(new Point(gridPosition.X + 1, gridPosition.Y)))
                                        {
                                            pirate.UpdateDestinationPoint(new Point(gridPosition.X + 1, gridPosition.Y));
                                        }
                                    }
                                }
                                if (gridPosition.Y > 0)
                                {
                                    if (game.mapManager.mapGrid[gridPosition.X, gridPosition.Y - 1].terrain == "land" || game.mapManager.mapGrid[gridPosition.X, gridPosition.Y - 1].terrain == "forest" || game.mapManager.mapGrid[gridPosition.X, gridPosition.Y - 1].terrain == "water")
                                    {
                                        if (pirate.UpdateDestinationPoint(new Point(gridPosition.X, gridPosition.Y - 1)))
                                        {
                                            pirate.UpdateDestinationPoint(new Point(gridPosition.X, gridPosition.Y - 1));
                                        }
                                    }
                                }
                                if (gridPosition.X > 0)
                                {
                                    if (pirate.UpdateDestinationPoint(new Point(gridPosition.X - 1, gridPosition.Y)))
                                    {
                                        pirate.UpdateDestinationPoint(new Point(gridPosition.X - 1, gridPosition.Y));
                                    }
                                }



                            }
                            Attack(unit);
                            break;
                        }
                    }
                }

                if (this.attackRectangle.Intersects(game.pirateManager.pirateShip.collisionRectangle))
                {
                    if (game.pirateManager.pirateShip.Alive)
                    {
                        if (attackspeedCounter >= attackSpeed)
                        {
                            Attack(game.pirateManager.pirateShip);
                        }
                    }
                }
            }
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
            if (Alive)
            {
                base.Draw();
            }
        }
    }

    

}
