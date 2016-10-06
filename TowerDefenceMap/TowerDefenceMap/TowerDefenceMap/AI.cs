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
    class AI
    {
        public Point position;
        public string direction { set; get; }
        public Stack<PointCharacterCombo> stack;
        public Point previousPosition;
        public string preferredDirection { set; get; }

        public AI(int x, int y, string direction)
        {
            position = new Point(x, y);
            this.direction = direction;
            previousPosition = new Point(-1,-1);
            stack = new Stack<PointCharacterCombo>();
            preferredDirection = direction;
        }
    }
}
