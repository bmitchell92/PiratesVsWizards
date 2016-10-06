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
    public class Wizard : MovingUnit
    {
        public Wizard(Game1 game, Point startPosition, string assetPath, int health, int movementSpeed, int attackSpeed, int range, int damage)
            : base(game, startPosition, assetPath, health, movementSpeed, attackSpeed, range, damage, new Point(20,20),new Point(1,1))
        {

        }
    }
}
