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
    public class Cursor : Sprite
    {
        //texture2D objects that will hold the according skin for easy refrence;
        private static Texture2D WIZARD_SELECT;
        private static Texture2D PIRATE_SELECT;
        private static Texture2D ERROR_SELECT;
        private string cursorType;

        //the varius modes. in each list are strings of the tiles that the cursor can perform an action upon
        public string[] cursorMode;
        public static string[] SELECT_UNIT = new string[2] { "tower", "demon" };
        public static string[] TOWER_PLACEMENT = new string[1] { "land"};
        public static string[] WALL_PLACEMENT = new string[1] { "land" };
        public static string[] DEMON_PLACEMENT = new string[2] { "forest", "water" };

        public static string[] SELECT_PIRATE = new string[1] { "pirate" };
        public static string[] PIRATE_PLACEMENT = new string[3] { "land","water","forest" };

        int cursorMoveCounter = 0;
        bool moved;

        // a method to easily call the cursors position in accordance to the mapgrid
        public Point locationOnMap()
        {
            return MapManager.coordinateToGrid(this.Position);
        }

        /*
         * traverses each string in the mode passed to the method, and checks the string against its current position's tile terrain type
         * if it is the same then the cursor can perform an action here
         */
        public bool isCursorLocationGood(string[] cursorMode)
        {           
            foreach (string array in cursorMode)
            {
                if (game.mapManager.mapGrid[MapManager.coordinateToGrid(this.Position).X, MapManager.coordinateToGrid(this.Position).Y].terrain == array) { return true; }
            }
            if (cursorMode == SELECT_PIRATE)
            {
                if (game.mapManager.mapGrid[locationOnMap().X, locationOnMap().Y].unit is Pirate)
                {
                    return true;
                }
            }
            return false;              
        }

        /*
         * sets the cursor up depending on the string passed to the constructor either pirate or wizard
         */
        public Cursor(Game1 game, string cursorType)
            : base(game, Vector2.Zero, "Images/WizardSelect")
        {
            WIZARD_SELECT = game.Content.Load<Texture2D>("Images/WizardSelect");
            PIRATE_SELECT = game.Content.Load<Texture2D>("Images/PirateSelect");
            ERROR_SELECT = game.Content.Load<Texture2D>("Images/ErrorSelect");
            this.cursorType = cursorType;

            cursorMode = SELECT_UNIT;

            if (this.cursorType == "wizard")
            {
                this.skin = WIZARD_SELECT;
                this.Position = MapManager.gridToCoordinate(new Point(19, 20));
            }
            else if (this.cursorType == "pirate")
            {
                this.skin = PIRATE_SELECT;
                this.Position = MapManager.gridToCoordinate(new Point(19, 2));
            }
        }

        /*
         * wizard controls
         *      left : move cursor to the left
         *      right: move cursor to the right
         *      up : move cursor up
         *      down : move cursor down
         *      1 : set cursor mode to select unit
         *      2 : set cursor mode to tower placement
         *      3 : set cursor mode to water demon placement
         *      4 : set cursor mode to forest demon placement
         *      enter : set wizard managers selected tile to cursors current position
         * pirate controls
         *      A : move cursor to the left
         *      D : move cursor to the right
         *      W : move cursor up
         *      S : move cursor down
         *      5 : set cursor mode to pirate select
         *      6 : set cursor mode to pirate placement
         */
        public override void Update(GameTime gameTime)
        {
            cursorMoveCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (cursorMoveCounter >= 50)
            {
                cursorMoveCounter = 50;
            }
            moved = false;
            if (cursorType == "wizard")
            {
                
                if (game.currentKeyboardState.IsKeyUp(Keys.Left) && game.previousKeyboardState.IsKeyDown(Keys.Left) || game.currentgamePadState.ThumbSticks.Left.X < 0 || game.currentgamePadState.IsButtonUp(Buttons.DPadLeft) && game.previousgamePadState.IsButtonDown(Buttons.DPadLeft))
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (game.wizardManager.statsSelected == false)
                        {
                            if (game.wizardManager.statCursor.Position.X > 177)
                            {
                                game.wizardManager.statCursor.Position.X -= 95;
                                game.wizardManager.statCursorValue -= 1;
                            }
                        }
                        else
                        {
                            if (this.Position.X > 0)
                                this.Position.X -= 21;
                        }
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.Right) && game.previousKeyboardState.IsKeyDown(Keys.Right) || game.currentgamePadState.ThumbSticks.Left.X > 0 || game.currentgamePadState.IsButtonUp(Buttons.DPadRight) && game.previousgamePadState.IsButtonDown(Buttons.DPadRight))
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (game.wizardManager.statsSelected == false)
                        {
                            if (game.wizardManager.statCursor.Position.X < 462)
                            {
                                game.wizardManager.statCursor.Position.X += 95;
                                game.wizardManager.statCursorValue += 1;
                            }
                        }
                        else
                        {
                            if (this.Position.X < 820)
                                this.Position.X += 21;
                        }
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.Up) && game.previousKeyboardState.IsKeyDown(Keys.Up) || game.currentgamePadState.ThumbSticks.Left.Y > 0 || game.currentgamePadState.IsButtonUp(Buttons.DPadUp) && game.previousgamePadState.IsButtonDown(Buttons.DPadUp))
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (game.wizardManager.statsSelected == false)
                        {
                            switch (game.wizardManager.statCursorValue)
                            {
                                case 1:
                                    if (game.wizardManager.health < 80)
                                        game.wizardManager.health += 5;
                                    break;
                                case 2:
                                    if (game.wizardManager.range < 10)
                                        game.wizardManager.range += 1;
                                    break;
                                case 3:
                                    if (game.wizardManager.damage < 10)
                                        game.wizardManager.damage += 1;
                                    break;
                                case 4:
                                    if (game.wizardManager.speedCounter < 10)
                                    {
                                        game.wizardManager.speedCounter += 1;
                                    }

                                    switch (game.wizardManager.speedCounter)
                                    {
                                        case 1:
                                            game.wizardManager.speed = 2000;
                                            break;
                                        case 2:
                                            game.wizardManager.speed = 1800;
                                            break;
                                        case 3:
                                            game.wizardManager.speed = 1600;
                                            break;
                                        case 4:
                                            game.wizardManager.speed = 1400;
                                            break;
                                        case 5:
                                            game.wizardManager.speed = 1200;
                                            break;
                                        case 6:
                                            game.wizardManager.speed = 1000;
                                            break;
                                        case 7:
                                            game.wizardManager.speed = 800;
                                            break;
                                        case 8:
                                            game.wizardManager.speed = 600;
                                            break;
                                        case 9:
                                            game.wizardManager.speed = 400;
                                            break;
                                        case 10:
                                            game.wizardManager.speed = 200;
                                            break;

                                    }
                                    break;

                            }

                        }
                        else
                        {
                            if (this.Position.Y > 200)
                                this.Position.Y -= 21;
                        }
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.Down) && game.previousKeyboardState.IsKeyDown(Keys.Down)|| game.currentgamePadState.ThumbSticks.Left.Y < 0 || game.currentgamePadState.IsButtonUp(Buttons.DPadDown) && game.previousgamePadState.IsButtonDown(Buttons.DPadDown))
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (game.wizardManager.statsSelected == false)
                        {
                            switch (game.wizardManager.statCursorValue)
                            {
                                case 1:
                                    if (game.wizardManager.health > 5)
                                        game.wizardManager.health -= 5;
                                    break;
                                case 2:
                                    if (game.wizardManager.range > 1)
                                        game.wizardManager.range -= 1;
                                    break;
                                case 3:
                                    if (game.wizardManager.damage > 1)
                                        game.wizardManager.damage -= 1;
                                    break;
                                case 4:
                                    if (game.wizardManager.speedCounter > 1)
                                    {
                                        game.wizardManager.speedCounter -= 1;
                                    }

                                    switch (game.wizardManager.speedCounter)
                                    {
                                        case 1:
                                            game.wizardManager.speed = 2000;
                                            break;
                                        case 2:
                                            game.wizardManager.speed = 1800;
                                            break;
                                        case 3:
                                            game.wizardManager.speed = 1600;
                                            break;
                                        case 4:
                                            game.wizardManager.speed = 1400;
                                            break;
                                        case 5:
                                            game.wizardManager.speed = 1200;
                                            break;
                                        case 6:
                                            game.wizardManager.speed = 1000;
                                            break;
                                        case 7:
                                            game.wizardManager.speed = 800;
                                            break;
                                        case 8:
                                            game.wizardManager.speed = 600;
                                            break;
                                        case 9:
                                            game.wizardManager.speed = 400;
                                            break;
                                        case 10:
                                            game.wizardManager.speed = 200;
                                            break;

                                    }
                                    break;


                            }

                        }
                        else
                        {
                            if (this.Position.Y < 620)
                                this.Position.Y += 21;
                        }
                    }
                }

            }
            else if (cursorType == "pirate")
            {
                if (game.currentKeyboardState.IsKeyUp(Keys.A) && game.previousKeyboardState.IsKeyDown(Keys.A) || game.currentgamePadStatep2.ThumbSticks.Left.X < 0)
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (this.Position.X > 0)
                            this.Position.X -= 21;
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.D) && game.previousKeyboardState.IsKeyDown(Keys.D) || game.currentgamePadStatep2.ThumbSticks.Left.X > 0)
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (this.Position.X < 820)
                            this.Position.X += 21;
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.W) && game.previousKeyboardState.IsKeyDown(Keys.W) || game.currentgamePadStatep2.ThumbSticks.Left.Y > 0)
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (this.Position.Y > 200)
                            this.Position.Y -= 21;
                    }
                }
                if (game.currentKeyboardState.IsKeyUp(Keys.S) && game.previousKeyboardState.IsKeyDown(Keys.S) || game.currentgamePadStatep2.ThumbSticks.Left.Y < 0)
                {
                    if (cursorMoveCounter >= 50)
                    {
                        moved = true;
                        game.soundBank.PlayCue("move");
                        if (this.Position.Y < 620)
                            this.Position.Y += 21;
                    }
                }
                if (game.pirateManager.selectedPirate == null)
                {
                    cursorMode = SELECT_PIRATE;
                }
                if (game.pirateManager.selectedPirate != null)
                {
                    cursorMode = PIRATE_PLACEMENT;
                }

                if (game.currentKeyboardState.IsKeyUp(Keys.Add) && game.previousKeyboardState.IsKeyDown(Keys.Add) || game.currentgamePadStatep2.ThumbSticks.Right.X > 0 && game.previousgamePadStatep2.ThumbSticks.Right.X == 0)
                {
                    if (game.pirateManager.pirateSelectCursor.Position.X < 590)
                    {
                        game.pirateManager.pirateSelectCursor.Position.X += 60;
                        game.pirateManager.selectedPirateValue += 1;
                    }
                    else
                    {
                        game.pirateManager.pirateSelectCursor.Position.X = 410;
                        game.pirateManager.selectedPirateValue = 1;
                    }
                }

                if (game.currentKeyboardState.IsKeyUp(Keys.Subtract) && game.previousKeyboardState.IsKeyDown(Keys.Subtract) || game.currentgamePadStatep2.ThumbSticks.Right.X < 0 && game.previousgamePadStatep2.ThumbSticks.Right.X == 0)
                {
                    if (game.pirateManager.pirateSelectCursor.Position.X > 410)
                    {
                        game.pirateManager.pirateSelectCursor.Position.X -= 60;
                        game.pirateManager.selectedPirateValue -= 1;
                    }
                    else
                    {
                        game.pirateManager.pirateSelectCursor.Position.X = 590;
                        game.pirateManager.selectedPirateValue = 4;
                    }
                }
            }
            if (moved)
            {
                cursorMoveCounter -= 50;
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {

            if (isCursorLocationGood(cursorMode))
            {
                if (cursorType == "wizard") { this.skin = WIZARD_SELECT; }
                if (cursorType == "pirate") { this.skin = PIRATE_SELECT; }
            }
            else
            {
                this.skin = ERROR_SELECT;
            }

            base.Draw();
        }

    }
}
