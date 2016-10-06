using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TowerDefenceMap
{
    public class MapManager
    {
        public Tile[,] mapGrid;
        Game1 game;
        Vector2 randomPosition;

        //returns a new position for a unit that needs to move by 1 position
        public Vector2 newPosition(Vector2 position, int i)
        {
            Vector2 newPos = position;
            switch(i)
            {
                // move up 1 slot
                case 0:
                    newPos.X += 0;
                    newPos.Y -= 1;
                    break;
                // move right 1 slot
                case 1:
                    newPos.X += 1;
                    newPos.Y -= 0;
                    break;
                // move down 1 slot
                case 2:
                    newPos.X += 0;
                    newPos.Y += 1;
                    break;
                // move left 1 slot
                case 3:
                    newPos.X -= 1;
                    newPos.Y -= 0;
                    break;
            }

            return newPos;
        }

        /*
         * a recursive method to poplulate the map with rocks
         * checks to see if the terrain your placing is a rock
         * checks to see if your placing it on land and not water and double checks to make sure object is within bounds
         * move in a direction( starting at 12 o'clock position and moves clockwise) pull a random number if its within bounds place a rock 
         * and then check to see if chanceofplacement allows for another rock if so the method calls itself
         * else exit method
         */
        public void terrainPlacement(Vector2 position, float chanceOfPlacement, String newTerrain)
        {
            // forces any position passed into range
            float randTemp;
            if (position.X > 40) { position.X = 40; }
            if (position.X < 0) { position.X = 0; }
            if (position.Y > 17) { position.Y = 17; }
            if (position.Y < 0) { position.Y = 0; }

            if (newTerrain == "rock")
            {

                if (mapGrid[(int)position.X, (int)position.Y].terrain == "land" &&
                    mapGrid[(int)position.X, (int)position.Y].terrain != "water" &&
                    position.X > 0 && position.X <= 40 && position.Y > 0 && position.Y <= 20)
                {
                    for (int i = 0; i < 8; ++i)
                    {
                        randTemp = game.random.Next(100);

                        if (randTemp <= chanceOfPlacement)
                        {
                            mapGrid[(int)position.X, (int)position.Y].skin = game.Content.Load<Texture2D>("images/rockTile");
                            mapGrid[(int)position.X, (int)position.Y].terrain = "rock";
                        }

                        if (chanceOfPlacement > 0)
                        {
                            terrainPlacement(newPosition(position, i), chanceOfPlacement - 20, newTerrain);
                        }
                    }
                }
            }
        }


        /*
         * a recursive method to poplulate the map with a river and a forest
         * the river is wieghted so that its more vertical
         * the forest is wieghted  so that its more horizontal
         *      does this my splitting chance of place placement into horizontal and vertical
         *      if the recursive method moves up or down checks random number against chanceOfVerticlePlacement
         *      if the recursive method moves left or right checks random number against chanceOfHorizontalPlacement
         * checks to see if the terrain your placing is a rock
         * checks to see if your placing it on land and not water and double checks to make sure object is within bounds
         * move in a direction( starting at 12 o'clock position and moves clockwise) pull a random number if its within bounds place a rock 
         * and then check to see if chanceofplacement allows for another rock if so the method calls itself
         * else exit method
         */
        public void terrainPlacement(Vector2 position, float chanceOfHorizontalPlacement, float chanceOfVerticalPlacement, String newTerrain)
        {
            float randTemp;
            if (position.X > 40) { position.X = 40; }
            if (position.X < 0) { position.X = 0; }
            if (position.Y > 17) { position.Y = 17; }
            if (position.Y < 0) { position.Y = 0; }

            if (newTerrain == "river")
            {
                if (mapGrid[(int)position.X, (int)position.Y].terrain == "land" &&
                    mapGrid[(int)position.X, (int)position.Y].terrain != "water" &&
                    position.X > 0 && position.X <= 40 && position.Y > 0 && position.Y <= 20)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        if (i == 0 || i == 2)
                        {
                            randTemp = game.random.Next(100);

                            if (randTemp <= chanceOfVerticalPlacement)
                            {
                                mapGrid[(int)position.X, (int)position.Y].skin = game.Content.Load<Texture2D>("images/waterTile");
                                mapGrid[(int)position.X, (int)position.Y].terrain = "water";
                            }

                            if (chanceOfVerticalPlacement > 0)
                            {
                                terrainPlacement(newPosition(position, i), chanceOfHorizontalPlacement - 50, chanceOfVerticalPlacement - 10, newTerrain);
                            }
                        }
                        else
                        {
                            randTemp = game.random.Next(100);

                            if (randTemp <= chanceOfHorizontalPlacement)
                            {
                                mapGrid[(int)position.X, (int)position.Y].skin = game.Content.Load<Texture2D>("images/waterTile");
                                mapGrid[(int)position.X, (int)position.Y].terrain = "water";
                            }

                            if (chanceOfHorizontalPlacement > 0)
                            {
                                terrainPlacement(newPosition(position, i), chanceOfHorizontalPlacement - 50, chanceOfVerticalPlacement - 10, newTerrain);
                            }
                        }
                    }
                }
            }
            if (newTerrain == "woods")
            {
                if (mapGrid[(int)position.X, (int)position.Y].terrain == "land" &&
                    mapGrid[(int)position.X, (int)position.Y].terrain != "water" &&
                    position.X > 0 && position.X <= 40 && position.Y > 0 && position.Y <= 20)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        if (i == 0 || i == 2)
                        {
                            randTemp = game.random.Next(100);

                            if (randTemp <= chanceOfVerticalPlacement)
                            {
                                mapGrid[(int)position.X, (int)position.Y].skin = game.Content.Load<Texture2D>("images/treeTile");
                                mapGrid[(int)position.X, (int)position.Y].terrain = "forest";
                            }

                            if (chanceOfVerticalPlacement > 0)
                            {
                                terrainPlacement(newPosition(position, i), chanceOfHorizontalPlacement - 5, chanceOfVerticalPlacement - 30, newTerrain);
                            }
                        }
                        else
                        {
                            randTemp = game.random.Next(100);

                            if (randTemp <= chanceOfHorizontalPlacement)
                            {
                                mapGrid[(int)position.X, (int)position.Y].skin = game.Content.Load<Texture2D>("images/treeTile");
                                mapGrid[(int)position.X, (int)position.Y].terrain = "forest";
                            }

                            if (chanceOfHorizontalPlacement > 0)
                            {
                                terrainPlacement(newPosition(position, i), chanceOfHorizontalPlacement - 5, chanceOfVerticalPlacement - 30, newTerrain);
                            }
                        }
                    }
                }
            }
        }

        //places 3 rows of water then randomly generates 3 rock formations, 1 river, and 1 forest
        public void generateMap()
        {
            for (int i = 0; i < 41; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    mapGrid[i, j].skin = game.Content.Load<Texture2D>("Images/waterTile");
                    mapGrid[i, j].terrain = "water";
                }
            }

            for (int b = 0; b < 3; ++b)
            {
                randomPosition.X = game.random.Next(0,40);
                randomPosition.Y = game.random.Next(3, 17);
                for (int stop = 0; stop == 0; ++stop)
               
                terrainPlacement(randomPosition, 100,"rock");
            }

            for (int b = 0; b < 1; ++b)
            {
                randomPosition.X = game.random.Next(0, 40);
                randomPosition.Y = 3;
                for (int stop = 0; stop == 0; ++stop)

                    terrainPlacement(randomPosition, 100, 100, "river");
            }

            for (int b = 0; b < 1; ++b)
            {
                randomPosition.X = game.random.Next(0, 40);
                randomPosition.Y = game.random.Next(3, 17);
                for (int stop = 0; stop == 0; ++stop)

                    terrainPlacement(randomPosition, 100, 100, "woods");
            }

            for (int i = 0; i < 41; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    mapGrid[i, j].terrain = "ocean";
                }
            }

        }

        // method to convert mapgrid to xna position
        public static Vector2 gridToCoordinate(Point grid)
        {
            Vector2 v;
            v = new Vector2(21 * (grid.X), (21 * (grid.Y)) + 200);
            return v;
        }

        // method to convert xna position to mapgrid
        public static Point coordinateToGrid(Vector2 coordinate)
        {
            Point p;
            p = new Point(Convert.ToInt32(Math.Floor(coordinate.X / 21)), Convert.ToInt32(Math.Floor((coordinate.Y-200) / 21)));
            return p;
        }


        //when created it covers entire mapgrid with land tiles
        public MapManager(Game1 game)
        {
            this.game = game;

            mapGrid = new Tile[41, 21];

            for (int i = 0; i <= 40; ++i)
            {
                for (int j = 0; j <= 20; ++j)
                {
                    mapGrid[i, j] = new Tile(game, new Vector2(21 * (i), (21 * (j)) + 200), "Images/landTile");
                    mapGrid[i, j].terrain = "land";
                }
            }
            generateMap();

            
        }

        public virtual void Update(GameTime gameTime)
        {
            if (game.currentKeyboardState.IsKeyUp(Keys.Space) && game.previousKeyboardState.IsKeyDown(Keys.Space))
            {
               //generateMap();
            }
            
            foreach (Tile tile in mapGrid)
            {
                tile.Update(gameTime);
            }

            
        }

        public virtual void Draw()
        {
            foreach (Tile tile in mapGrid)
            {
                tile.Draw();
            }
        }

        public string[] get2DArray()
        {
            string[] map = new string[21];
            map[0] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            for (int i = 1; i < 20; i++)
            {
                map[i] += "X";
                for (int j = 1; j < 42; j++)
                {
                    if (mapGrid[j-1, i+1].terrain == "rock")
                    {
                        map[i] += "X";
                    }
                    else
                    {
                        if (mapGrid[j - 1, i + 1].unit != null)
                        {
                            map[i] += "X";
                        }
                        else
                        {
                            map[i] += "O";
                        }
                    }
                }
                map[i] += "X";
            }
            map[20] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            return map;
        }
    }
}
