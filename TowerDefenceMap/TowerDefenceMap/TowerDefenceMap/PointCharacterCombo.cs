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

namespace TowerDefenceMap
{
    class PointCharacterCombo
    {
        public Point position;
        public char character;

        public PointCharacterCombo(int x, int y, char character)
        {
            position = new Point(x, y);
            this.character = character;
        }
    }
}
