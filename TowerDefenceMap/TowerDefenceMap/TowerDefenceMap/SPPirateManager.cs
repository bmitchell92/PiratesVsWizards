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
    public class SPPirateManager: PirateManager
    {
        int wave;
        float gracePeriodCounter = 0;
        float pirateUpdateCounter = 0;
        int pirateUpdateTime = 10000;
        int gracePeriod = 25000;
        public SPPirateManager(Game1 game):
            base(game)
        {
            pirates = new List<Pirate>(80);
            for (int i = 0; i < 60; i++)
            {
                pirates.Add(new Pirate(game, new Point(20, 2), "Images/swordPirate", 5, 200, 500, 1, 1, new Point(20, 20), new Point(1, 1)));
            }
            wave = 0;
            numberOfPirates = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (numberOfPirates == 0)
            {
                if (wave == 3 && level != 3)
                {
                    level += 1;
                    if (level == 2)
                    {
                        PirateValues.swordPirateHealth = 10;
                        PirateValues.pistolPirateAttack = 3;
                        PirateValues.pistolPirateHealth = 20;
                    }
                    if (level == 3)
                    {
                        PirateValues.swordPirateAttack = 2;
                        PirateValues.swordPirateHealth = 15;
                        PirateValues.pistolPirateAttack = 4;
                        PirateValues.pistolPirateHealth = 30;
                        PirateValues.pirateCaptainAttack = 4;
                        PirateValues.pirateCaptainHealth = 80;
                    }
                    wave = 0;
                }
                if (level == 3)
                {
                    if (wave == 2)
                    {
                        SpawnPirates(25, 10, 5, 10);
                        wave = 3;
                    }
                    if (wave == 1)
                    {
                        SpawnPirates(13, 12, 13, 12);
                        wave = 2;
                    }
                    if (wave == 0)
                    {
                        SpawnPirates(10, 10, 10, 0);
                        wave = 1;
                    }
                }

                if (level == 2)
                {
                    if (wave == 2)
                    {
                        SpawnPirates(13, 12, 25, 0);
                        wave = 3;
                    }
                    if (wave == 1)
                    {
                        SpawnPirates(15, 9, 6, 0);
                        wave = 2;
                    }
                    if (wave == 0)
                    {
                        SpawnPirates(10, 10, 0, 0);
                        wave = 1;
                    }
                }

                if (level == 1)
                {
                    if (wave == 2)
                    {
                        SpawnPirates(10, 20, 0, 0);
                        wave = 3;
                    }
                    if (wave == 1)
                    {
                        SpawnPirates(10, 5, 0, 0);
                        wave = 2;
                    }
                    if (wave == 0)
                    {
                        gracePeriodCounter += gameTime.ElapsedGameTime.Milliseconds;
                        if (gracePeriodCounter >= gracePeriod)
                        {
                            SpawnPirates(10, 0, 0, 0);
                            wave = 1;
                        }
                    }
                }

            }
                pirateUpdateCounter += gameTime.ElapsedGameTime.Milliseconds;
                if (pirateUpdateCounter >= pirateUpdateTime)
                {
                    pirateUpdateCounter -= pirateUpdateTime;
                    foreach (Pirate buccaneer in pirates)
                    {
                        if (buccaneer.Alive)
                        {
                            buccaneer.UpdateDestinationPoint(new Point(game.random.Next(5)+17, 18));
                        }
                    }
                }
            pirateShip.Update(gameTime);
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
        }

        private void SpawnPirates(int numberOfSwordPirates, int numberOfPistolPirates, int numberOfPirateCaptains, int numberOfBombPirates)
        {
            int currentNumberOfPirates = 0;
            for (int i = 0; i < numberOfSwordPirates; i++)
            {
                if (!pirates[i].Alive)
                {
                    pirates[i] = new SwordPirate(game, new Point(game.random.Next(40), 2));
                    pirates[i].Alive = true;
                    numberOfPirates += 1;
                }
            }
            currentNumberOfPirates = numberOfPirates;
            for (int i = 0; i < numberOfPistolPirates; i++)
            {
                if (!pirates[i+currentNumberOfPirates].Alive)
                {
                    pirates[i + currentNumberOfPirates] = new PistolPirate(game, new Point(game.random.Next(40), 2));
                    pirates[i + currentNumberOfPirates].Alive = true;
                    numberOfPirates += 1;
                }
            }
            currentNumberOfPirates = numberOfPirates;
            for (int i = 0; i < numberOfPirateCaptains; i++)
            {
                if (!pirates[i + currentNumberOfPirates].Alive)
                {
                    pirates[i + currentNumberOfPirates] = new PirateCaptain(game, new Point(game.random.Next(40), 2));
                    pirates[i + currentNumberOfPirates].Alive = true;
                    numberOfPirates += 1;
                }
            }
            currentNumberOfPirates = numberOfPirates;
            for (int i = 0; i < numberOfBombPirates; i++)
            {
                if (!pirates[i + currentNumberOfPirates].Alive)
                {
                    pirates[i + currentNumberOfPirates] = new BombPirate(game, new Point(game.random.Next(40), 2));
                    pirates[i + currentNumberOfPirates].Alive = true;
                    numberOfPirates += 1;
                }
            }
            currentNumberOfPirates = numberOfPirates;
        }
        public override void Draw()
        {
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
            UIBackground.Draw();
            game.spriteBatch.Draw(barFilling, new Rectangle(230, 167, (int)(shipBarLength * healthPercentage()), 30), Color.White);
            game.spriteBatch.DrawString(largeFont, "Level: "+level.ToString(), new Vector2(10, 130), Color.Yellow);
            game.spriteBatch.DrawString(largeFont, "Wave: " + wave.ToString(), new Vector2(200, 130), Color.Yellow);
            game.spriteBatch.DrawString(largeFont, pirateShip.health.ToString(), new Vector2(380, 170), Color.Yellow);
            game.spriteBatch.DrawString(largeFont, "Number of pirates: "+numberOfPirates, new Vector2(450, 170), Color.Yellow);
            game.spriteBatch.Draw(barFilling, new Rectangle(0, 160, 860, 5), Color.Black);
            game.spriteBatch.DrawString(largeFont, "Pirate Ship Health:", new Vector2(10, 165), Color.Yellow);
        }
    }
}
