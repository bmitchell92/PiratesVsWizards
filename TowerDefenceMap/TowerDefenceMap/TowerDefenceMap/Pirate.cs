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
    public class Pirate: MovingUnit
    {
        public int maxHealth;
        public Pirate(Game1 game, Point startPosition, string assetPath, int health, int movementSpeed, int attackSpeed, int range, int damage, Point frameSize, Point sheetSize)
            : base(game, startPosition, assetPath, health, movementSpeed, attackSpeed, range, damage,frameSize,sheetSize)
        {
            if (game.pirateManager != null)
            {
                if (!(game.pirateManager is SPPirateManager))
                {
                    if (game.pirateManager.pirateCursor.isCursorLocationGood(Cursor.PIRATE_PLACEMENT))
                    {
                        UpdateDestinationPoint(game.pirateManager.pirateCursor.locationOnMap());
                    }
                }
            }
            Theta = 0;
            currentFrame.Y = 0;
            maxHealth = health;
        }

         public override void Update(GameTime gameTime)
         {
             if (Alive)
             {
                 if (game.mapManager.mapGrid[this.gridPosition.X, this.gridPosition.Y].terrain == "water")
                 {
                     currentMovementSpeed = (int)(movementSpeed * .5f);
                 }
                 else if (game.mapManager.mapGrid[this.gridPosition.X, this.gridPosition.Y].terrain == "forest")
                 {
                     currentMovementSpeed = (int)(movementSpeed * 2f);
                 }
                 else
                 {
                     currentMovementSpeed = movementSpeed;
                 }
                 foreach (Unit unit in game.wizardManager.WizardUnitList)
                 {
                     if (this.attackRectangle.Intersects(unit.collisionRectangle)&&unit.Alive)
                     {
                         if (attackspeedCounter >= attackSpeed)
                         {
                             Attack(unit);
                             break;
                         }
                     }
                 }
                 if (this.attackRectangle.Intersects(game.wizardManager.wizard.collisionRectangle))
                 {
                     if (attackspeedCounter >= attackSpeed&game.wizardManager.wizard.Alive)
                     {
                         Attack(game.wizardManager.wizard);
                     }
                 }
             }
             base.Update(gameTime);
         }

         public override void Attack(Unit target)
         {
             currentFrame.Y = 1;
             currentFrame.X = 0;
             Theta = (float)Math.Atan2(target.Position.Y - this.Position.Y, target.Position.X - this.Position.X)-MathHelper.PiOver2;
         }
         public override void Draw()
         {
             base.Draw();
             if (stack.Count != 0 && game.pirateManager.selectedPirate == this)
             {
                 flag.Draw();
             }
         }
    }
}
