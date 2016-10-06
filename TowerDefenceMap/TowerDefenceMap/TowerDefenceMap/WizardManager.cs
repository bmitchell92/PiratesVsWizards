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
    public class WizardManager
    {
        public Cursor wizardCursor;
        public Sprite statCursor;
        SelectedTile selectedTile;
        Game1 game;

        //public static Texture2D STAT_SELECT;

        AnamatedSprite pentagram;
        SpriteFont verylargeFont;
        SpriteFont largeFont;
        SpriteFont mediumFont;
        SpriteFont smallFont;
        Sprite UIBackground;
        Sprite castleHealthBar;
        Sprite A_Button;
        Sprite B_Button;
        Sprite X_Button;
        Sprite Y_Button;
        Sprite LB_Button;
        //Sprite RB_Button;
        protected int millisecondsPerFrame;
        bool startclock;
        bool canPlaceUnit;

        public List<Unit> WizardUnitList;
        public MovingUnit wizard;
        public Unit castle;

        protected int timeTillSpawn;
        protected int ResetTimeTillSpawn = 1000;

        public int barLength;
        public Texture2D barFilling;
        public Texture2D UIBarHorizontal_1;
        public Texture2D UIBarVertical_1;
        public Texture2D UIBarVertical_2;
        public Texture2D UIBarVerticalThin_1;
        public Texture2D UIBarVerticalThin_2;
        public Texture2D UIBarVerticalThin_3;
        public Texture2D UIBarVerticalThin_4;

        int counter;
        Tower tower;
        Unit wall;
        Unit Demon;
        Sprite towerDisplay;
        Sprite wallDisplay;
        Sprite demonDisplay;

        public int health;
        public int range;
        public int damage;
        public int speed;
        public int speedCounter;

        public int timerSec;
        public int timerMin;

        public int statCursorValue;

        bool done = false;
        public bool statsSelected = true;

        public string unitToPlace;

        public struct SelectedTile
        {
            public Point location;
            public bool used;


            public SelectedTile(bool whatever)
            {
                location = new Point(1, 1);
                used = true;
            }
        }
        
        

        public float healthPercentage()
        {
            float temp;
            temp = (float)castle.health / 500;
            return temp;
        }

        public float calculateTime(int health, int range, int damage, int speed)
        {
            float time = 0;

            time += ((health / 5) * 0.2f);
            time += (range * 0.4f);
            time += (damage * 0.4f);
            time += (speed * 0.4f);

            
            return time;
        }

        public bool withinBounds(Point currentPosition)
        {
            if (currentPosition.Y <= 20 && currentPosition.Y >= 0 && currentPosition.X >= 0 && currentPosition.X <= 40)
            {
                
                return true;
            }
            return false;
        }

        public WizardManager(Game1 game)
        {
            Point point = new Point(20, 20);
            Vector2 vector = new Vector2(600, 600);
            this.game = game;
            wizardCursor = new Cursor(game, "wizard");
            statCursor = new Sprite(game, new Vector2(177,685),"Images/statSelect");
            selectedTile = new SelectedTile();
            pentagram = new AnamatedSprite(game,ref game.spriteBatch , "Images/pentagramSheet", new Vector2(600, 600) , new Point(20, 20) , new Point(11, 1), 60);
            wizard = new MovingUnit(game,new Point(15,20), "images/wizard", 5, 200, 0, 0, 0, new Point(20,20),new Point(1,1));
            wizard.Alive = true;
            wizard.Theta = MathHelper.Pi;
            WizardUnitList = new List<Unit>();
            timeTillSpawn  = ResetTimeTillSpawn;
            UIBackground = new Sprite(game, new Vector2(0, 640), "Images/WizardUIBackground");
            verylargeFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_35");
            largeFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_20");
            mediumFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_15");
            smallFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_12");
            castleHealthBar = new Sprite(game, new Vector2(210, 645), "Images/CastleHealthBar");

            A_Button = new Sprite(game, new Vector2(675, 690), "Images/a_Button");
            B_Button = new Sprite(game, new Vector2(675, 720), "Images/b_Button");
            X_Button = new Sprite(game, new Vector2(675, 750), "Images/x_Button");
            Y_Button = new Sprite(game, new Vector2(675, 780), "Images/y_Button");
            LB_Button = new Sprite(game, new Vector2(675, 810), "Images/lb_Button");
            //RB_Button = new Sprite(game, new Vector2(675, 700), "Images/rb_Button");

            towerDisplay = new Sprite(game, new Vector2(38, 715), "Images/Tower");
            towerDisplay.Scale = 5;

            wallDisplay = new Sprite(game, new Vector2(36, 715), "images/wall");
            wallDisplay.Scale = 5;

            demonDisplay = new Sprite(game, new Vector2(36, 715), "images/demondisplay");
            demonDisplay.Scale = 3;

            barLength = 150;
            barFilling = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarHorizontal_1 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVertical_1 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVertical_2 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVerticalThin_1 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVerticalThin_2 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVerticalThin_3 = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBarVerticalThin_4 = game.Content.Load<Texture2D>(@"Images/barFilling");
            statCursorValue = 1;
            health = 5;
            range = 1;
            damage = 1;
            speed = 2000;
            speedCounter = 1;
            pentagram.Alive = false;
            canPlaceUnit = true;
            castle = new Unit(game, new Point(16, 18), "Images/castle", 500, 0, 0, 0,new Point(167,62), new Point(1,1));
            castle.Alive = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    game.mapManager.mapGrid[j + 16, i + 18].unit = castle;
                }
            }
            WizardUnitList.Add(castle);
        }

        public virtual void Update(GameTime gameTime)
        {
            timerSec = (int)gameTime.TotalGameTime.Seconds;
            timerMin = (int)gameTime.TotalGameTime.Minutes;

            if (game.currentKeyboardState.IsKeyUp(Keys.Enter) && game.previousKeyboardState.IsKeyDown(Keys.Enter) || game.currentgamePadState.IsButtonUp(Buttons.A) && game.previousgamePadState.IsButtonDown(Buttons.A))
            {
                if (statsSelected == false)
                {
                    pentagram.millisecondsPerFrame = (int)(calculateTime(health, range, damage, speedCounter) * 1000) / 11;
                    timeTillSpawn = (int)(calculateTime(health, range, damage, speedCounter) * 1000);
                    statsSelected = true;
                }
                else
                {
                    if (canPlaceUnit)
                    {
                        canPlaceUnit = false;

                        //if (wizardCursor.cursorMode == Cursor.TOWER_PLACEMENT)
                        //{
                            if (wizard.Alive == false)
                            {
                                wizard.gridPosition = new Point(15,20);
                                wizard.health = 5;
                                wizard.Theta = MathHelper.Pi;
                                wizard.Alive = true;
                            }
                            selectedTile.location = wizardCursor.locationOnMap();
                            pentagram.Position = new Vector2(MapManager.gridToCoordinate(selectedTile.location).X + 130, MapManager.gridToCoordinate(selectedTile.location).Y + 10);
                            //pentagram.Position = new Vector2(selectedTile.location.X, selectedTile.location.Y);

                            if (game.mapManager.mapGrid[selectedTile.location.X, selectedTile.location.Y].terrain == "land" && game.mapManager.mapGrid[selectedTile.location.X, selectedTile.location.Y].unit == null)
                            {
                                wizard.UpdateDestinationPoint(selectedTile.location);
                                if (wizardCursor.cursorMode == Cursor.TOWER_PLACEMENT)
                                {
                                    unitToPlace = "tower";
                                }
                                else if (wizardCursor.cursorMode == Cursor.WALL_PLACEMENT)
                                {
                                    unitToPlace = "wall";
                                }
                                else if (wizardCursor.cursorMode == Cursor.DEMON_PLACEMENT)
                                {
                                    unitToPlace = "demon";
                                }
                            }
                            wizardCursor.cursorMode = Cursor.SELECT_UNIT;
                            statsSelected = true;
                        //}
                    }
                }
            }

            if (startclock)
            {
                timeTillSpawn -= gameTime.ElapsedGameTime.Milliseconds;

                // and if its been there long enough show the next one
                if (timeTillSpawn <= 0)
                {
                    // reset the frame display time 
                    //timeTillSpawn = (int)(calculateTime(health, range, damage, speed) * 1000);

                    done = true;
                }
            }
            if (game.currentKeyboardState.IsKeyUp(Keys.Enter) && game.previousKeyboardState.IsKeyDown(Keys.Enter) || game.currentgamePadState.IsButtonUp(Buttons.A) && game.previousgamePadState.IsButtonDown(Buttons.A))
            {
                statsSelected = true;
            }
            if (wizard.Alive == false)
            {
                if (game.summon.IsPlaying)
                {
                    game.summon.Pause();
                }
                startclock = false;
                pentagram.currentFrame.X = 0;
                pentagram.Alive = false;
                timeTillSpawn = (int)(calculateTime(health, range, damage, speedCounter) * 1000);
            }
            if (game.currentKeyboardState.IsKeyUp(Keys.T) && game.previousKeyboardState.IsKeyDown(Keys.T) || game.currentgamePadState.IsButtonUp(Buttons.X) && game.previousgamePadState.IsButtonDown(Buttons.X))
            {
                statsSelected = false;
                canPlaceUnit = true;

                if (wizardCursor.cursorMode == Cursor.TOWER_PLACEMENT)
                {
                    health = 5;
                    damage = 1;
                    speedCounter = 1;
                    range = 1;
                    wizardCursor.cursorMode = Cursor.SELECT_UNIT;
                    statsSelected = true;
                }
                else
                {
                    wizardCursor.cursorMode = Cursor.TOWER_PLACEMENT;
                }
            }

            if (game.currentKeyboardState.IsKeyUp(Keys.W) && game.previousKeyboardState.IsKeyDown(Keys.W) || game.currentgamePadState.IsButtonUp(Buttons.B) && game.previousgamePadState.IsButtonDown(Buttons.B))
            {
                statsSelected = false;
                canPlaceUnit = true;

                if (wizardCursor.cursorMode == Cursor.WALL_PLACEMENT)
                {
                    health = 5;
                    wizardCursor.cursorMode = Cursor.SELECT_UNIT;
                    statsSelected = true;
                }
                else
                {
                    wizardCursor.cursorMode = Cursor.WALL_PLACEMENT;
                }
            }

            if (game.currentKeyboardState.IsKeyUp(Keys.D) && game.previousKeyboardState.IsKeyDown(Keys.D) || game.currentgamePadState.IsButtonUp(Buttons.Y) && game.previousgamePadState.IsButtonDown(Buttons.Y))
            {
                statsSelected = false;
                canPlaceUnit = true;

                if (wizardCursor.cursorMode == Cursor.DEMON_PLACEMENT)
                {
                    health = 5;
                    damage = 1;
                    speedCounter = 1;
                    range = 1;
                    wizardCursor.cursorMode = Cursor.SELECT_UNIT;
                    statsSelected = true;
                }
                else
                {
                    wizardCursor.cursorMode = Cursor.DEMON_PLACEMENT;
                }
            }

            //if (game.currentKeyboardState.IsKeyUp(Keys.U) && game.previousKeyboardState.IsKeyDown(Keys.U) || game.currentgamePadState.IsButtonUp(Buttons.A) && game.previousgamePadState.IsButtonDown(Buttons.A))
            //{
            //    health = 5;
            //    damage = 1;
            //    speedCounter = 1;
            //    range = 1;
            //    wizardCursor.cursorMode = Cursor.SELECT_UNIT;
            //}

            foreach (Sprite wizardUnit in WizardUnitList)
            {
                wizardUnit.Update(gameTime);
            }

            castleHealthBar.Update(gameTime);
                
            wizard.Update(gameTime);
            if (pentagram.Alive)
            {
                pentagram.Update(gameTime);
            }
            wizardCursor.Update(gameTime);
            towerDisplay.Update(gameTime);
            statCursor.Update(gameTime);
            UIBackground.Update(gameTime);
            castle.Update(gameTime);

            if (wizard.gridPosition == selectedTile.location)
            {
                if (!startclock)
                {
                    if (game.summon.IsPaused)
                    {
                        game.summon.Resume();
                    }
                    else
                    {
                        if (!game.summon.IsPlaying)
                        {
                            game.summon.Play();
                        }
                    }
                }
                startclock = true;
                pentagram.Alive = true;

                if (withinBounds(wizard.gridPosition))
                {
                    if (wizard.gridPosition.Y >= 20)
                    {
                        wizard.gridPosition.Y -= 1;
                        wizard.Theta = 0;
                    }
                    else if ((game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y + 1].terrain == "land" || game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y + 1].terrain == "forest" && wizard.gridPosition.Y < 19 )&& game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y + 1].unit == null)
                    {
                        wizard.gridPosition.Y += 1;
                        wizard.Theta = MathHelper.Pi;
                    }
                    else if ((game.mapManager.mapGrid[wizard.gridPosition.X + 1, wizard.gridPosition.Y].terrain == "land" || game.mapManager.mapGrid[wizard.gridPosition.X + 1, wizard.gridPosition.Y].terrain == "forest") && game.mapManager.mapGrid[wizard.gridPosition.X + 1, wizard.gridPosition.Y].unit == null)
                    {
                        wizard.gridPosition.X += 1;
                        wizard.Theta = MathHelper.PiOver2;
                    }
                    else if ((game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].terrain == "land" || game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].terrain == "forest") && game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].unit == null)
                    {
                        wizard.gridPosition.Y -= 1;
                        wizard.Theta = 0;
                    }
                    else
                    {
                        wizard.gridPosition.X -= 1;
                        wizard.Theta = (MathHelper.PiOver2 * 3);
                    }       


                }
               
            }
            if (done)
            {
                canPlaceUnit = true;
                pentagram.Alive = false;
                if (unitToPlace == "tower")
                {
                    tower = new Tower(game, new Point(selectedTile.location.X, selectedTile.location.Y), health, speed, range, damage);
                    tower.Alive = true;
                    WizardUnitList.Add(tower);
                }
                else if (unitToPlace == "wall")
                {
                    wall = new Unit(game, new Point(selectedTile.location.X, selectedTile.location.Y), "Images/wall", health, 0, 0, 0, new Point(20, 20), Point.Zero);
                    wall.Alive = true;
                    WizardUnitList.Add(wall);
                }
                else if (unitToPlace == "demon")
                {
                    string temp = "";
                    if (game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].terrain == "forest")
                    {
                        temp = "Images/ForestDemon";
                    }
                    else if (game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].terrain == "water")
                    {
                        temp = "Images/WaterDemon";
                    }
                    else if (game.mapManager.mapGrid[wizard.gridPosition.X, wizard.gridPosition.Y - 1].terrain == "land")
                    {
                        temp = "Images/ForestDemon";
                    }

                    Demon = new Unit(game, new Point(selectedTile.location.X, selectedTile.location.Y), temp, health, 0, 0, 0, new Point(20, 20), Point.Zero);
                    Demon.Alive = true;
                    WizardUnitList.Add(Demon);
                }

                game.mapManager.mapGrid[selectedTile.location.X, selectedTile.location.Y].unit = new Unit(game, new Point(1, 1), "Images/wizard", 0, 0, 0, 0, Point.Zero, Point.Zero);
                startclock = false;
                done = false;
                statsSelected = false;
                game.summon.Pause();
               
            }
        }

        public virtual void Draw()
        {
           
            wizard.Draw();
           

            foreach (Sprite wizardUnit in WizardUnitList)
            {
                wizardUnit.Draw();
            }

            UIBackground.Draw();


            game.spriteBatch.Draw(barFilling, new Rectangle(210, 645,(int)(barLength * healthPercentage()), 30), Color.White);
            game.spriteBatch.Draw(UIBarHorizontal_1, new Rectangle(0, 680, 860, 5), Color.Black);
            game.spriteBatch.Draw(UIBarVertical_1, new Rectangle(650, 680, 5, 200), Color.Black);
            game.spriteBatch.Draw(UIBarVertical_2, new Rectangle(175, 680, 5, 200), Color.Black);

            if (wizardCursor.cursorMode == Cursor.TOWER_PLACEMENT)
            {
                game.spriteBatch.Draw(UIBarVerticalThin_1, new Rectangle(270, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_2, new Rectangle(365, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_3, new Rectangle(460, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_4, new Rectangle(555, 680, 2, 200), Color.Black);
                game.spriteBatch.DrawString(mediumFont, "Health", new Vector2(197, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Range", new Vector2(292, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Damage", new Vector2(381, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Speed", new Vector2(485, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(largeFont, "Time", new Vector2(575, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, health.ToString(), new Vector2(210, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, range.ToString(), new Vector2(305, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, damage.ToString(), new Vector2(400, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, speedCounter.ToString(), new Vector2(495, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, calculateTime(health,range,damage,speedCounter).ToString(), new Vector2(575, 740), Color.MediumPurple);
                if (statsSelected == false)
                {
                    statCursor.Draw();
                }


            }

            if (wizardCursor.cursorMode == Cursor.WALL_PLACEMENT)
            {
                wallDisplay.Draw();
                game.spriteBatch.DrawString(mediumFont, "Health", new Vector2(197, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(largeFont, "Wall", new Vector2(50, 685), Color.MediumPurple);
                game.spriteBatch.Draw(UIBarVerticalThin_1, new Rectangle(270, 680, 2, 200), Color.Black);
                game.spriteBatch.DrawString(verylargeFont, health.ToString(), new Vector2(210, 740), Color.MediumPurple);
                game.spriteBatch.Draw(UIBarVerticalThin_4, new Rectangle(555, 680, 2, 200), Color.Black);
                game.spriteBatch.DrawString(verylargeFont, calculateTime(health, range, damage, speedCounter).ToString(), new Vector2(575, 740), Color.MediumPurple);
                if (statsSelected == false)
                {
                    statCursor.Draw();
                }
            }

            if (wizardCursor.cursorMode == Cursor.DEMON_PLACEMENT)
            {
                demonDisplay.Draw();
                game.spriteBatch.DrawString(largeFont, "Demons", new Vector2(50, 685), Color.MediumPurple);
                game.spriteBatch.Draw(UIBarVerticalThin_1, new Rectangle(270, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_2, new Rectangle(365, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_3, new Rectangle(460, 680, 2, 200), Color.Black);
                game.spriteBatch.Draw(UIBarVerticalThin_4, new Rectangle(555, 680, 2, 200), Color.Black);
                game.spriteBatch.DrawString(mediumFont, "Health", new Vector2(197, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Range", new Vector2(292, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Damage", new Vector2(381, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(mediumFont, "Speed", new Vector2(485, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(largeFont, "Time", new Vector2(575, 685), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, health.ToString(), new Vector2(210, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, range.ToString(), new Vector2(305, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, damage.ToString(), new Vector2(400, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, speedCounter.ToString(), new Vector2(495, 740), Color.MediumPurple);
                game.spriteBatch.DrawString(verylargeFont, calculateTime(health, range, damage, speedCounter).ToString(), new Vector2(575, 740), Color.MediumPurple);
                if (statsSelected == false)
                {
                    statCursor.Draw();
                }
            }

            game.spriteBatch.DrawString(largeFont, "Castle Health", new Vector2(10, 642), Color.MediumPurple);
            game.spriteBatch.DrawString(largeFont, castle.health.ToString(), new Vector2(365, 642), Color.MediumPurple);
            game.spriteBatch.DrawString(largeFont, "Time:", new Vector2(650, 642), Color.MediumPurple);
            game.spriteBatch.DrawString(largeFont,timerMin.ToString() + ":" + timerSec.ToString(), new Vector2(730, 642), Color.MediumPurple);
            game.spriteBatch.DrawString(largeFont, "Exp:", new Vector2(470, 642), Color.MediumPurple);
            game.spriteBatch.DrawString(largeFont, "0", new Vector2(545, 642), Color.MediumPurple);

            game.spriteBatch.DrawString(smallFont, "Select a Unit", new Vector2(705, 693), Color.MediumPurple);
            game.spriteBatch.DrawString(smallFont, "Spawn a Wall", new Vector2(705, 723), Color.MediumPurple);
            game.spriteBatch.DrawString(smallFont, "Spawn a Tower", new Vector2(705, 753), Color.MediumPurple);
            game.spriteBatch.DrawString(smallFont, "Spawn a Demon", new Vector2(705, 783), Color.MediumPurple);
            game.spriteBatch.DrawString(smallFont, "Upgrade Stats", new Vector2(705, 813), Color.MediumPurple);

            castleHealthBar.Draw();
            if (pentagram.Alive)
            {
                pentagram.Draw();
            }
            castle.Draw();

            wizardCursor.Draw();
            A_Button.Draw();
            B_Button.Draw();
            X_Button.Draw();
            Y_Button.Draw();
            LB_Button.Draw();
            //RB_Button.Draw();

            if (wizardCursor.cursorMode == Cursor.TOWER_PLACEMENT)
            {
                towerDisplay.Draw();
                game.spriteBatch.DrawString(largeFont, "Tower", new Vector2(50, 685), Color.MediumPurple);                
            }
        }
    }
}
