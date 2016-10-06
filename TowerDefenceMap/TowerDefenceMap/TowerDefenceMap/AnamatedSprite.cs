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
   public class AnamatedSprite : Sprite
    {
        protected Point frameSize;
        public Point currentFrame;
        protected Point sheetSize;

        // anamation time Framerate 
        
        public int millisecondsPerFrame;
        protected int timeSinceLastFrame;

        // misc
        protected Rectangle currentFrameRectangle;

        public AnamatedSprite(Game1 game,                    // ref to the main
                      ref SpriteBatch spriteBatch,    // reference to main's SpriteBatch object
                      string assetPath,               // texture of the sprite
                      Vector2 initialPosition,        // initially were we want the sprite to appear
                      Point frameSize,                // width and height in pixels of an individual sprite image
                      Point sheetSize,                // number of images on the sheet by column and rows
                      int frameRate)                  // frame rate in milliseconds
            : base(game, initialPosition,assetPath) {  // initialize the Sprite class

                this.frameSize = frameSize;
                this.sheetSize = sheetSize;
                this.millisecondsPerFrame = frameRate;

                currentFrame = new Point(0, 0);
                timeSinceLastFrame = 0;
                currentFrameRectangle = new Rectangle((int)Position.X, (int)Position.Y, frameSize.X, frameSize.Y);
                collisionRectangle = new Rectangle((int)this.Position.X - this.skin.Width / 2,
                       (int)this.Position.Y - this.skin.Height / 2,
                       this.frameSize.X,
                       this.frameSize.Y);
        }



        public override void Update(GameTime gameTime)
        {
            // update how long this image has been displayed
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            // and if its been there long enough show the next one
            if (timeSinceLastFrame >= millisecondsPerFrame)
            {
                // reset the frame display time 
                timeSinceLastFrame -= millisecondsPerFrame;

                // grab the next image from the sheet
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }

                // recalculate the display frame rectangle
                currentFrameRectangle.X = currentFrame.X * frameSize.X;
                currentFrameRectangle.Y = currentFrame.Y * frameSize.Y;

            }

            if (Alive)
            {
                Position.X += DeltaX;
                Position.Y += DeltaY;
            }
            collisionRectangle = new Rectangle((int)this.Position.X-this.skin.Width/2,
                                   (int)this.Position.Y-this.skin.Height/2,
                                   this.frameSize.X,
                                   this.frameSize.Y);
        }


        /// <summary>
        /// displays the sprite with its unique properties.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw()  { 

        game.spriteBatch.Draw(skin,                           // this is the entire texture sheet
                             Position,                       // location on screen to display the sprite
                             currentFrameRectangle,          // x and y position of the specific sprite on the sprite sheet
                             Tint,                           // tint color
                             Theta,                          // rotation angle
                             centerOfSprite,                 // cneter of he individual sprite based on its width & height
                             Scale,                          // draw scale
                             SpriteEffects.None,             // flip or not
                             0);                         // layer depth
        }
    }
}
