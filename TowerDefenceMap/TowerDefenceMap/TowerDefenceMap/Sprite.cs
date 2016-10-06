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

//using ManagerDemo;
//using Madines_Managers;

namespace TowerDefenceMap
{
    public class Sprite
    {
        public Vector2 centerOfSprite;               // orgin of the sprite
        public Texture2D skin;                          // sprite texture
        public Texture2D collisionSkin;                 // collision sprite texture
        protected Game1 game;                           // reference back to the instantiation class

        protected SpriteEffects spriteEffects;
       
        public virtual Rectangle collisionRectangle { set; get; }

        public Vector2 Position; //{ set; get; }           // x & y coordinets on the screen where we will draw the image
        public float Theta { set; get; }                // angle of rotation used to draw the image
        public float Scale { set; get; }                // magnification factor used when drawing
        public Color Tint { set; get; }                 // tinting color applied when we draw the sprite
        public float DeltaX { set; get; }               // incremential change across the X axis 
        public float DeltaY { set; get; }               // incremential change across the Y axis
        public Boolean Alive { set; get; }              // status flag
        public float layer { set; get; }                // layer of image
        protected Vector2 speed { set; get; }           // speed of 
   

        /// <summary>
        /// creates a sprite object usint the specified texture and position.
        /// </summary>
        /// <param name="game">ref to the main</param>
        /// <param name="assetPath">complete path to the texture using only slash characters as delimiters</param>
        /// <param name="initialPosition">display position of the sprite</param>

        public Sprite(Game1 game,                       // ref to the main
                     Vector2 position,          // initially were we want the sprite to appear
                     string assetPath)

        {

            this.game = game;
            skin = game.Content.Load<Texture2D>(assetPath);
            this.Position = position;
            this.DeltaX = DeltaX;
            this.DeltaY = DeltaY;
            layer = 0;
            speed = Vector2.Zero;

            Theta = 0.0f;
            Scale = 1.0f;

            Tint = Color.White;
            spriteEffects = SpriteEffects.None;

            centerOfSprite = new Vector2(skin.Width / 2, skin.Height / 2);

            spriteEffects = SpriteEffects.None;

            collisionSkin = game.Content.Load<Texture2D>("Images/collisionSkin");

            // build a collision rectangle for this sprite for later, and
            // as the draw method is referencing the center, you must compensate for that.
            collisionRectangle = new Rectangle((int)this.Position.X,
                                               (int)this.Position.Y,
                                               this.skin.Width,
                                               this.skin.Height);
        }

        public Sprite(Game1 game,                       // ref to the main
                      string assetPath,                 // texture of the sprite
                      Vector2 initialPosition,          // initially were we want the sprite to appear
                      float DeltaX,                     // velocity across the x axis
                      float DeltaY)                     // velocity accross the y axis
                     
        {             

            this.game = game;
            skin = game.Content.Load<Texture2D>(assetPath);
            Position = initialPosition;
            this.DeltaX = DeltaX;
            this.DeltaY = DeltaY;

            Theta = 0.0f;
            Scale = 1.0f;

            Tint = Color.White;
            spriteEffects = SpriteEffects.None;

            centerOfSprite = new Vector2(skin.Width / 2, skin.Height / 2);

            spriteEffects = SpriteEffects.None;

            collisionSkin = game.Content.Load<Texture2D>("Images/collisionSkin");

            // build a collision rectangle for this sprite for later, and
            // as the draw method is referencing the center, you must compensate for that.
            collisionRectangle = new Rectangle((int)this.Position.X,
                                               (int)this.Position.Y,
                                               this.skin.Width,
                                               this.skin.Height);
         }

        public Sprite(Game1 game,                       // ref to the main
                      string assetPath,                 // texture of the sprite
                      Vector2 initialPosition,          // initially were we want the sprite to appear
                      float DeltaX,                     // velocity across the x axis
                      float DeltaY,                     // velocity accross the y axis
                      float layer) // layer of image
        {

            this.game = game;
            skin = game.Content.Load<Texture2D>(assetPath);
            Position = initialPosition;
            this.DeltaX = DeltaX;
            this.DeltaY = DeltaY;
            this.layer = layer;

            
            Theta = 0.0f;
            Scale = 1.0f;
            
            Tint = Color.White;
            spriteEffects = SpriteEffects.None;

            centerOfSprite = new Vector2(skin.Width / 2, skin.Height / 2);

            spriteEffects = SpriteEffects.None;

            collisionSkin = game.Content.Load<Texture2D>("Images/collisionSkin");

            // build a collision rectangle for this sprite for later, and
            // as the draw method is referencing the center, you must compensate for that.
            collisionRectangle = new Rectangle((int)this.Position.X,
                                               (int)this.Position.Y,
                                               this.skin.Width,
                                               this.skin.Height);
        }

        public Sprite(Game1 game,                       // ref to the main
                      string assetPath,                 // texture of the sprite
                      Vector2 initialPosition,          // initially were we want the sprite to appear
                      float DeltaX,                     // velocity across the x axis
                      float DeltaY,                     // velocity accross the y axis
                      float layer, // layer of image
                      float scale)
        {

            this.game = game;
            skin = game.Content.Load<Texture2D>(assetPath);
            Position = initialPosition;
            this.DeltaX = DeltaX;
            this.DeltaY = DeltaY;
            this.layer = layer;
            this.Scale = scale;

            
            Theta = 0.0f;
            Scale = 1.0f;
            
            Tint = Color.White;
            spriteEffects = SpriteEffects.None;

            centerOfSprite = new Vector2(skin.Width / 2, skin.Height / 2);

            spriteEffects = SpriteEffects.None;

            collisionSkin = game.Content.Load<Texture2D>("Images/collisionSkin");

            // build a collision rectangle for this sprite for later, and
            // as the draw method is referencing the center, you must compensate for that.
            collisionRectangle = new Rectangle((int)this.Position.X,
                                               (int)this.Position.Y,
                                               this.skin.Width,
                                               this.skin.Height);
        }





        /// <summary>
        /// Updates all dynamic characterstics of the sprite
        /// </summary>
        /// <param name="gameTime">total elapsed time since beginning of game</param>
        public virtual void Update(GameTime gameTime) 
        {
            if (game.currentKeyboardState.IsKeyDown(Keys.Space) || game.currentgamePadState.IsButtonDown(Buttons.RightShoulder))
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (Alive)
            {
                Position.X += DeltaX;
                Position.Y += DeltaY;
            }
            collisionRectangle = new Rectangle((int)this.Position.X,
                                   (int)this.Position.Y,
                                   this.skin.Width,
                                   this.skin.Height);
            
        }


        /// <summary>
        /// displays the sprite with its unique properties.
        /// </summary>
        public virtual void Draw()  {

                game.spriteBatch.Draw(skin,                 // sprite to draw
                                 Position,                  // where to draw it
                                 null,                      // source rectangle of sprite, used with sprite sheets else null
                                 Tint,                      // tinting color
                                 Theta,                     // rotation angle in radians
                    //centerOfSprite,            // specifies the origin of the sprite when rotation is used
                                 Vector2.Zero,
                                 Scale,                     // scale value 
                                 spriteEffects,             // such as flip horz || vert 
                                 layer);                        // layer depth
            if (game.DEBUG)
               collisionRectangleDraw();
       }

        //for collision debugging
        public virtual void collisionRectangleDraw()
        {
            game.spriteBatch.Draw(collisionSkin, this.collisionRectangle, Color.White);

        }
    }
}
