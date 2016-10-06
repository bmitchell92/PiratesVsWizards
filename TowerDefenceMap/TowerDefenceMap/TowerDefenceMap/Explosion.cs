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
    public class Explosion: AnamatedSprite
    {
        public bool attacked;
        public Explosion(Game1 game, Vector2 position)
            : base(game,ref game.spriteBatch, "Images/explosion", position, new Point(60, 60), new Point(7, 1), 100)
        {
            currentFrame = new Point(0, 0);
            Alive = true;
            attacked = false;
            game.soundBank.PlayCue("explosion");
        }

        public override void Update(GameTime gameTime)
        {
            if(Alive)
            {
                base.Update(gameTime);
                if (currentFrame.X == 1)
                {
                    if (!attacked)
                    {
                        foreach (Unit unit in game.wizardManager.WizardUnitList)
                        {
                            if (this.collisionRectangle.Intersects(unit.collisionRectangle))
                            {
                                if (unit.Alive)
                                {
                                    unit.health -= 50;
                                }
                            }
                        }
                        if (this.collisionRectangle.Intersects(game.wizardManager.wizard.collisionRectangle))
                        {
                            if (game.wizardManager.wizard.Alive)
                            {
                                game.wizardManager.wizard.health -= 50;
                            }
                        }
                        foreach (Pirate pirate in game.pirateManager.pirates)
                        {
                            if (this.collisionRectangle.Intersects(pirate.collisionRectangle))
                            {
                                if (pirate.Alive)
                                {
                                    pirate.health -= 50;
                                }
                            }
                        }
                    }
                    attacked = true;
                }
                if (currentFrame.X == 6)
                {
                    Alive = false;
                    attacked = false;
                }
            }
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
