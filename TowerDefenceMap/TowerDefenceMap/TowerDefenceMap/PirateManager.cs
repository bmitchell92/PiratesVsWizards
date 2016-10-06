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
    public class PirateManager
    {
        public Cursor pirateCursor;
        public Sprite pirateSelectCursor;
        public int selectedPirateValue;
        string pirateString;

        public Unit pirateShip;

        protected Game1 game;
        Point selectedTile;
        public List<Pirate> pirates;
        public Pirate selectedPirate;
        public int barLength;
        public int shipBarLength;
        public int timeToIncrement;
        public int barCounter;
        public int numberOfPirates;
        public Sprite bar;
        public Texture2D barFilling;

        protected SpriteFont verylargeFont;
        protected SpriteFont largeFont;
        protected SpriteFont mediumFont;
        protected SpriteFont smallFont;

        Sprite A_Button;
        Sprite B_Button;
        Sprite X_Button;
        Sprite Y_Button;
        Sprite LB_Button;
        Sprite RB_Button;

        Sprite[] lines;

        public int level;

        protected Sprite UIBackground;

        public PirateManager(Game1 game)
        {
            this.game = game;
            pirates = new List<Pirate>(20);
            pirateCursor = new Cursor(game, "pirate");
            pirateSelectCursor = new Sprite(game, new Vector2(410, 0), "Images/pirateUnitSelect");
            for (int i = 0; i < 20; i++)
            {
                pirates.Add(new Pirate(game, new Point(20, 2), "Images/swordPirate",5,200,500,1,1,new Point(20,20),new Point(1,1)));
            }
            pirateShip = new Unit(game, new Point(20, 0), "Images/pirateShip", 500, 0, 0, 0, new Point(167, 62), new Point(1, 1));
            pirateShip.Alive = true;
            pirateString = "Sword Pirate";
            selectedPirate = null;
            selectedPirateValue = 1;
            barLength = 0;
            timeToIncrement = 500;
            level = 1;
            barCounter = 0;
            bar = new Sprite(game, new Vector2(0, 50), "Images/bar");
            shipBarLength = 150;
            barFilling = game.Content.Load<Texture2D>(@"Images/barFilling");
            UIBackground = new Sprite(game, new Vector2(0, 0), "Images/WizardUIBackground");
            numberOfPirates = 0;
            lines = new Sprite[9];
            for (int i = 0; i < 9; i++)
            {
                lines[i] = new Sprite(game, new Vector2((i*20) + 24, 54), "Images/barLine");
            }

            verylargeFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_35");
            largeFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_20");
            mediumFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_15");
            smallFont = game.Content.Load<SpriteFont>(@"Font/SWGothe_12");

            A_Button = new Sprite(game, new Vector2(675, 10), "Images/a_Button");
            B_Button = new Sprite(game, new Vector2(675, 40), "Images/b_Button");
            X_Button = new Sprite(game, new Vector2(675, 70), "Images/x_Button");
            Y_Button = new Sprite(game, new Vector2(675, 100), "Images/y_Button");
            LB_Button = new Sprite(game, new Vector2(675, 130), "Images/lb_Button");
            RB_Button = new Sprite(game, new Vector2(725, 170), "Images/rb_Button");

        }

        public virtual void Update(GameTime gameTime)
        {
            barCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (barCounter >= timeToIncrement)
            {
                barCounter -= timeToIncrement;
                barLength += 2;
                if (barLength > 200)
                {
                    barLength = 200;
                }
            }

            bar.Update(gameTime);
            foreach (Pirate buccaneer in pirates)
            {
                if (buccaneer is BombPirate)
                {
                    BombPirate pirate = (BombPirate)buccaneer;
                    if (pirate.explosion != null)
                    {
                        pirate.explosion.Update(gameTime);
                    }
                }
                    buccaneer.Update(gameTime);
            }
            pirateShip.Update(gameTime);
            if (game.currentKeyboardState.IsKeyUp(Keys.Enter) && game.previousKeyboardState.IsKeyDown(Keys.Enter) || game.currentgamePadStatep2.IsButtonUp(Buttons.A) && game.previousgamePadStatep2.IsButtonDown(Buttons.A))
            {
                selectedTile = pirateCursor.locationOnMap();
            }
            pirateSelectCursor.Update(gameTime);

            if (selectedPirateValue == 1)
            {
                pirateString = "Sword Pirate";
            }
            if (selectedPirateValue == 2)
            {
                pirateString = "Pistol Pirate";
            }
            if (selectedPirateValue == 3)
            {
                pirateString = "Pirate Captain";
            }
            if (selectedPirateValue == 4)
            {
                pirateString = "Bomb Pirate";
            }

            if (game.currentKeyboardState.IsKeyUp(Keys.D7) && game.previousKeyboardState.IsKeyDown(Keys.D7) || game.currentgamePadStatep2.IsButtonUp(Buttons.X) && game.previousgamePadStatep2.IsButtonDown(Buttons.X))
            {
                if (pirateString == "Sword Pirate")
                {
                    if (barLength >= 10 && numberOfPirates < 20)
                    {
                        barLength -= 10;
                        numberOfPirates += 1;
                        for (int i = 0; i < pirates.Capacity; i++)
                        {
                            if (!pirates[i].Alive)
                            {
                                pirates[i] = new SwordPirate(game, new Point(game.random.Next(40), 2));
                                pirates[i].Alive = true;
                                break;
                            }
                        }
                    }
                }
                if (pirateString == "Pistol Pirate")
                {
                    if (barLength >= 20 && numberOfPirates < 20)
                    {
                        barLength -= 20;
                        numberOfPirates += 1;
                        for (int i = 0; i < pirates.Capacity; i++)
                        {
                            if (!pirates[i].Alive)
                            {
                                pirates[i] = new PistolPirate(game, new Point(game.random.Next(40), 2));
                                pirates[i].Alive = true;
                                break;
                            }
                        }
                    }
                }
                if (pirateString == "Pirate Captain")
                {
                    if (barLength >= 60 && numberOfPirates < 20)
                    {
                        barLength -= 60;
                        numberOfPirates += 1;
                        for (int i = 0; i < pirates.Capacity; i++)
                        {
                            if (!pirates[i].Alive)
                            {
                                pirates[i] = new PirateCaptain(game, new Point(game.random.Next(40), 2));
                                pirates[i].Alive = true;
                                break;
                            }
                        }
                    }
                }
                if (pirateString == "Bomb Pirate")
                {
                    if (barLength >= 80 && numberOfPirates < 20)
                    {
                        barLength -= 80;
                        numberOfPirates += 1;
                        for (int i = 0; i < pirates.Capacity; i++)
                        {
                            if (!pirates[i].Alive)
                            {
                                pirates[i] = new BombPirate(game, new Point(game.random.Next(40), 2));
                                pirates[i].Alive = true;
                                break;
                            }
                        }
                    }
                }
            }
            pirateCursor.Update(gameTime);

            if (game.currentKeyboardState.IsKeyUp(Keys.Enter) && game.previousKeyboardState.IsKeyDown(Keys.Enter) || game.currentgamePadStatep2.IsButtonUp(Buttons.A) && game.previousgamePadStatep2.IsButtonDown(Buttons.A))
            {
                if (pirateCursor.isCursorLocationGood(Cursor.PIRATE_PLACEMENT))
                {
                    if (selectedPirate != null)
                    {
                        selectedPirate.UpdateDestinationPoint(pirateCursor.locationOnMap());
                    }
                }
            }
            if (game.currentKeyboardState.IsKeyUp(Keys.RightShift) && game.previousKeyboardState.IsKeyDown(Keys.RightShift) || game.currentgamePadStatep2.IsButtonUp(Buttons.B) && game.previousgamePadStatep2.IsButtonDown(Buttons.B))
            {
                if (pirateCursor.isCursorLocationGood(Cursor.SELECT_PIRATE))
                {
                    selectedPirate = (Pirate)game.mapManager.mapGrid[pirateCursor.locationOnMap().X, pirateCursor.locationOnMap().Y].unit;
                }
                else
                {
                    selectedPirate = null;
                }
            }
        }

        public float healthPercentage()
        {
            float temp;
            temp = (float)pirateShip.health / 500;
            return temp;
        }


        public virtual void Draw()
        {
            if (selectedPirate != null)
            {
                if (selectedPirate.Alive)
                {
                    game.spriteBatch.Draw(selectedPirate.collisionSkin, selectedPirate.attackRectangle, Color.White);
                }
            }
            foreach (Pirate buccaneer in pirates)
            {
                if (buccaneer is BombPirate)
                {
                    BombPirate pirate = (BombPirate)buccaneer;
                    if (pirate.explosion != null)
                    {
                        pirate.explosion.Draw();
                    }
                }
                if (buccaneer.Alive)
                {
                    buccaneer.Draw();
                }
            }
            pirateShip.Draw();
            pirateCursor.Draw();
            if (selectedPirate != null)
            {
                if (selectedPirate.Alive)
                {
                    game.spriteBatch.Draw(game.selectionTexture, new Rectangle((int)selectedPirate.Position.X,(int)selectedPirate.Position.Y, 20,20), Color.White);
                }
            }
            UIBackground.Draw();
            game.spriteBatch.Draw(barFilling, new Rectangle(230, 167, (int)(shipBarLength * healthPercentage()), 30), Color.White);
            game.spriteBatch.DrawString(largeFont,pirateShip.health.ToString(), new Vector2(380, 170), Color.Yellow);
            game.spriteBatch.Draw(barFilling, new Rectangle(0, 160, 860, 5), Color.Black);
            game.spriteBatch.Draw(barFilling, new Rectangle(650, 0, 5, 160), Color.Black);
            game.spriteBatch.Draw(barFilling, new Rectangle(225, 0, 5, 160), Color.Black);
            game.spriteBatch.Draw(barFilling, new Rectangle(405, 0, 5, 160), Color.Black);
            bar.Draw();
            game.spriteBatch.Draw(barFilling, new Rectangle(4,54,barLength,40), Color.White);
            foreach (Sprite line in lines)
            {
                line.Draw();
            }

            game.spriteBatch.DrawString(smallFont, "Move Pirate to Location", new Vector2(705, 10), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "Select/Deselect a Pirate", new Vector2(705, 40), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "Spawn a Pirate", new Vector2(705, 70), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "Spawn Random Pirates", new Vector2(705, 100), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "Upgrade Stats", new Vector2(705, 130), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "Unit Info", new Vector2(755, 170), Color.Yellow);

            A_Button.Draw();
            B_Button.Draw();
            X_Button.Draw();
            Y_Button.Draw();
            LB_Button.Draw();
            RB_Button.Draw();

            game.spriteBatch.DrawString(largeFont, "Pirate Ship Health:", new Vector2(10, 165), Color.Yellow);
            game.spriteBatch.DrawString(largeFont, "Spawn Bar:", new Vector2(10, 10), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "5% to spawn sword pirate", new Vector2(5, 95), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "10% to spawn pistol pirate", new Vector2(5, 110), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "30% to spawn pirate captain", new Vector2(5, 125), Color.Yellow);
            game.spriteBatch.DrawString(smallFont, "40% to spawn bomb pirate", new Vector2(5, 140), Color.Yellow);

            if (selectedPirate != null)
            {
                if (selectedPirate is SwordPirate)
                {
                    game.spriteBatch.DrawString(mediumFont, "Sword Pirate", new Vector2(270, 5), Color.Yellow);
                    game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/swordPirate"), new Rectangle(285, 30, 60, 60), Color.White);
                    game.spriteBatch.DrawString(smallFont, "Attack Speed: 5", new Vector2(230, 105), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Movement Speed: 5", new Vector2(230, 120), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Damage: "+selectedPirate.damage, new Vector2(230, 135), Color.Yellow);
                }
                if (selectedPirate is PistolPirate)
                {
                    game.spriteBatch.DrawString(mediumFont, "Pistol Pirate", new Vector2(270, 5), Color.Yellow);
                    game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/pistolPirate"), new Rectangle(285, 30, 60, 60), Color.White);
                    game.spriteBatch.DrawString(smallFont, "Attack Speed: 3", new Vector2(230, 105), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Movement Speed: 3", new Vector2(230, 120), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Damage: "+selectedPirate.damage, new Vector2(230, 135), Color.Yellow);
                }
                if (selectedPirate is PirateCaptain)
                {
                    PirateCaptain capn = (PirateCaptain)selectedPirate;
                    game.spriteBatch.DrawString(mediumFont, "Pirate Captain", new Vector2(270, 5), Color.Yellow);
                    game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/pirateCaptain"), new Rectangle(285, 30, 60, 60), Color.White);
                    game.spriteBatch.DrawString(smallFont, "Attack Speed: 1", new Vector2(230, 105), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Movement Speed: 1", new Vector2(230, 120), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Damage: " + selectedPirate.damage + "x"+capn.ShotCount, new Vector2(230, 135), Color.Yellow);
                }
                if (selectedPirate is BombPirate)
                {
                    game.spriteBatch.DrawString(mediumFont, "Bomb Pirate", new Vector2(270, 5), Color.Yellow);
                    game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/bombPirate"), new Rectangle(285, 30, 60, 60), Color.White);
                    game.spriteBatch.DrawString(smallFont, "Attack Speed: one attack", new Vector2(230, 105), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Movement Speed: 4", new Vector2(230, 120), Color.Yellow);
                    game.spriteBatch.DrawString(smallFont, "Damage: " + selectedPirate.damage, new Vector2(230, 135), Color.Yellow);
                }
                game.spriteBatch.DrawString(smallFont, "Health: " + selectedPirate.health + "/" + selectedPirate.maxHealth, new Vector2(230, 90), Color.Yellow);
                game.spriteBatch.DrawString(smallFont, "Range: " + selectedPirate.range, new Vector2(330, 135), Color.Yellow);
            }

            game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/swordPirate"), new Rectangle(410, 0, 60, 60), Color.White);
            game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/pistolPirate"), new Rectangle(470, 0, 60, 60), Color.White);
            game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/pirateCaptain"), new Rectangle(530, 0, 60, 60), Color.White);
            game.spriteBatch.Draw(game.Content.Load<Texture2D>(@"Images/bombPirate"), new Rectangle(590, 0, 60, 60), Color.White);

            pirateSelectCursor.Draw();

            game.spriteBatch.DrawString(largeFont, pirateString, new Vector2(415, 130), Color.Yellow);
        }
    }
}
