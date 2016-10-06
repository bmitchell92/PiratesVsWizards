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
    public class BombPirate: Pirate
    {
        public Explosion explosion;
        public BombPirate(Game1 game, Point startPosition)
            : base(game,startPosition, "Images/bombPirate_animated", PirateValues.bombPirateHealth, 300, 200, 1, PirateValues.bombPirateAttack,new Point(20,20),new Point(4,1))
        {

        }

        public override void Attack(Unit target)
        {
            health -= damage;
            base.Attack(target);
            explosion = new Explosion(game, this.Position+new Vector2(190,10));
        }
    }
}
