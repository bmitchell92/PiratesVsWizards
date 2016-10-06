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
    public class SwordPirate: Pirate
    {
        public SwordPirate(Game1 game, Point startPosition)
            : base(game, startPosition, "Images/swordPirate_animated", PirateValues.swordPirateHealth, 200, 500, 1, PirateValues.swordPirateAttack, new Point(20,20),new Point(4,1))
        {
            millisecondsPerFrame = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Attack(Unit target)
        {
            game.soundBank.PlayCue("sword");
                target.health -= damage;
                attackspeedCounter = 0;
                base.Attack(target);
        }
    }
}
