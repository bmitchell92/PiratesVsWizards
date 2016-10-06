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
    public class Unit: AnamatedSprite
    {
        public int health { set; get; }
        public int attackSpeed { set; get; }
        public int range { set; get; }
        public int damage { set; get; }
        public Point gridPosition;
        public Rectangle attackRectangle;
        protected int attackspeedCounter;


        public Unit(Game1 game, Point startPosition, string assetPath, int health, int attackSpeed, int range, int damage,Point frameSize,Point sheetSize)
            : base(game, ref game.spriteBatch,assetPath,MapManager.gridToCoordinate(startPosition),frameSize,sheetSize,100)
        {
            this.health = health;
            this.attackSpeed = attackSpeed;
            this.range = range;
            this.damage = damage;

            this.gridPosition = startPosition;
            this.attackRectangle = new Rectangle((int)((Position.X + centerOfSprite.X) - (range * 21)), (int)(((Position.Y + centerOfSprite.Y) - (range * 21))),
            range * 42, range * 42);

        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0 && Alive == true)
            {
                Alive = false;
                if (this is Pirate)
                {
                    game.pirateManager.numberOfPirates -= 1;
                    if (this is BombPirate)
                    {
                        BombPirate pirate = (BombPirate)this;
                        pirate.explosion = new Explosion(game, this.Position + new Vector2(190, 10));
                    }
                }
                if (game.wizardManager.castle == this)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            game.mapManager.mapGrid[j + 16, i + 18].unit = null;
                        }
                    }
                    game.soundBank.PlayCue("you_suck");
                }
            }
            if (Alive)
            {
                attackspeedCounter += gameTime.ElapsedGameTime.Milliseconds;
                if (attackspeedCounter > attackSpeed)
                {
                    attackspeedCounter = attackSpeed;
                }
                game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = this;
            }
            else
            {
                game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
            }
            Position = MapManager.gridToCoordinate(gridPosition);
            base.Update(gameTime);
            collisionRectangle = new Rectangle((int)this.Position.X + (int)centerOfSprite.X - this.skin.Width / 2,
           (int)this.Position.Y + (int)centerOfSprite.Y - this.skin.Height / 2,
           this.frameSize.X,
           this.frameSize.Y);
            this.attackRectangle = new Rectangle((int)(this.Position.X + (int)frameSize.X/2 - (range * 21)), (int)(this.Position.Y + (int)frameSize.Y/2 - (range * 21)),
            range * 42, range * 42);
        }

        public override void Draw()
        {
            if (Alive)
            {
                game.spriteBatch.Draw(skin,
                     Position+new Vector2(frameSize.X/2,frameSize.Y/2),
                     currentFrameRectangle,
                     Tint,   
                     Theta,               
                     new Vector2(frameSize.X/2,frameSize.Y/2),     
                     Scale,             
                     SpriteEffects.None,          
                     0);
            }
        }

        public virtual void Attack(Unit target)
        {
        }
    }
}
