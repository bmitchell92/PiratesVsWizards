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
    // used to hold information for locations within mapgrid
    public class Tile : Sprite
    {
        public string terrain;

        public Unit unit { get; set; }

        public void setTile(Unit unit)
        {
            this.unit = unit;
        }

        public Tile(Game1 game, Vector2 position, string assetPath) :base(game,position, assetPath){}

        public virtual void  Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual void Draw()
        {
            base.Draw();
        }
    }
}
