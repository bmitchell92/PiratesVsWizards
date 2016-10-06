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

namespace TowerDefenceMap
{
    public class Projectile:Sprite
    {
        Unit shooter;
                public Projectile(Game1 game, Point startPosition, string assetPath, Unit shooter)
            : base(game, MapManager.gridToCoordinate(startPosition), assetPath)
        {
            this.shooter = shooter;
            Alive = false;
        }

                public override void Update(GameTime gameTime)
                {
                    if (Alive)
                    {
                        collisionRectangle = new Rectangle((int)this.Position.X,
                       (int)this.Position.Y,
                       this.skin.Width,
                       this.skin.Height);
                        foreach (Unit unit in game.wizardManager.WizardUnitList)
                        {
                            if (this.collisionRectangle.Intersects(unit.collisionRectangle)&& unit != shooter)
                            {
                                if (unit.Alive)
                                {
                                    unit.health -= shooter.damage;
                                    Alive = false;
                                    Position = shooter.Position;
                                    break;
                                }
                            }
                        }
                        if (this.collisionRectangle.Intersects(game.wizardManager.wizard.collisionRectangle))
                        {
                            if (game.wizardManager.wizard.Alive)
                            {
                                game.wizardManager.wizard.health -= shooter.damage;
                                Alive = false;
                                Position = shooter.Position;
                            }
                        }
                        if (this.collisionRectangle.Intersects(game.pirateManager.pirateShip.collisionRectangle))
                        {
                            if (game.pirateManager.pirateShip.Alive)
                            {
                                game.pirateManager.pirateShip.health -= shooter.damage;
                                Alive = false;
                                Position = shooter.Position;
                            }
                        }
                        foreach (Pirate pirate in game.pirateManager.pirates)
                        {
                            if (this.collisionRectangle.Intersects(pirate.collisionRectangle)&& pirate != shooter)
                            {
                                if (pirate.Alive)
                                {
                                    pirate.health -= shooter.damage;
                                    Alive = false;
                                    Position = shooter.Position;
                                    break;
                                }
                            }
                        }
                        if (!this.collisionRectangle.Intersects(shooter.attackRectangle))
                        {
                            Alive = false;
                        }
                    }
                    base.Update(gameTime);
                }

                public override void Draw()
                {
                    if (Alive)
                    {
                        base.Draw();
                    }
                }
    }
}
