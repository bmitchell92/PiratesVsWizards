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
    public class MovingUnit: Unit
    {
        protected int movementSpeed { set; get; }
        public int currentMovementSpeed;
        public Stack<Point> stack;
        public string direction;
        protected Point destinationPoint;
        int timeInterval;
        protected Sprite flag;


        public MovingUnit(Game1 game, Point startPosition, string assetPath, int health, int movementSpeed, int attackSpeed, int range, int damage,Point frameSize, Point sheetSize)
            : base(game, startPosition,assetPath, health,attackSpeed,range, damage,frameSize,sheetSize)
        {
            this.movementSpeed = movementSpeed;
            currentMovementSpeed = movementSpeed;

            direction = "south";
            destinationPoint = new Point(19,17);
            stack = new Stack<Point>();
            Queue<Point> q = getGoToPoints("south");
            timeInterval = 0;
            if (q.Count != 0)
            {
                stack.Push(destinationPoint);
                while(q.Count != 0)
                {
                    stack.Push(q.Dequeue());
                }
            }
            flag = new Sprite(game, MapManager.gridToCoordinate(destinationPoint), "Images/pirateflag");
        }

        private static void replaceCharacter(ref string[] map, int x, int y, char character)
        {
            string newRow = "";
            for (int i = 0; i < 43; i++)
            {
                if (i != x)
                {
                    newRow += map[y][i];
                }
                else
                {
                    newRow += character;
                }
            }
            map[y] = newRow;
        }

        public bool UpdateDestinationPoint(Point newPoint)
        {
            if (destinationPoint == newPoint)
            {
                return false;
            }
            else
            {
                destinationPoint = newPoint;
                while (stack.Count != 0)
                {
                    stack.Pop();
                }
                Queue<Point> q = BestOfFour();
                if (q.Count != 0)
                {
                    stack.Push(destinationPoint);
                    while (q.Count != 0)
                    {
                        stack.Push(q.Dequeue());
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                flag.Position = MapManager.gridToCoordinate(destinationPoint);
                timeInterval += gameTime.ElapsedGameTime.Milliseconds;
                if (timeInterval > currentMovementSpeed)
                {
                    game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = this;
                    timeInterval -= currentMovementSpeed;
                    if (stack.Count != 0)
                    {
                        if (stack.Peek().X > gridPosition.X && game.mapManager.mapGrid[gridPosition.X + 1, gridPosition.Y].unit == null)
                        {
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
                            game.mapManager.mapGrid[gridPosition.X + 1, gridPosition.Y].unit = this;
                            gridPosition.X += 1;
                            Theta = (MathHelper.PiOver2*3);

                        }
                        if (stack.Peek().X < gridPosition.X && game.mapManager.mapGrid[gridPosition.X - 1, gridPosition.Y].unit == null)
                        {
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
                            game.mapManager.mapGrid[gridPosition.X - 1, gridPosition.Y].unit = this;
                            gridPosition.X -= 1;
                            Theta = MathHelper.PiOver2;
                        }
                        if (stack.Peek().Y > gridPosition.Y && game.mapManager.mapGrid[gridPosition.X, gridPosition.Y + 1].unit == null)
                        {
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y + 1].unit = this;
                            gridPosition.Y += 1;
                            Theta = 0;

                        }
                        if (stack.Peek().Y < gridPosition.Y && game.mapManager.mapGrid[gridPosition.X, gridPosition.Y - 1].unit == null)
                        {
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
                            game.mapManager.mapGrid[gridPosition.X, gridPosition.Y - 1].unit = this;
                            gridPosition.Y -= 1;
                            Theta = MathHelper.Pi;
                        }
                        if (stack.Peek() == gridPosition)
                        {
                            stack.Pop();
                        }
                    }
                }
            }
            else
            {
                game.mapManager.mapGrid[gridPosition.X, gridPosition.Y].unit = null;
            }
            Position = MapManager.gridToCoordinate(gridPosition);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
        protected Queue<Point> getGoToPoints(string direction)
        {
            Queue<Point> q;
            q = new Queue<Point>();
            String[] map = game.mapManager.get2DArray();
            AI pirate = new AI(gridPosition.X+1,gridPosition.Y-1, direction);
            PointCharacterCombo point = new PointCharacterCombo(destinationPoint.X+1, destinationPoint.Y-1, 'P');
            PointCharacterCombo currentPoint = new PointCharacterCombo(point.position.X, point.position.Y, 'D');
            while (map[currentPoint.position.Y + 1][currentPoint.position.X] == 'O')
            {
                replaceCharacter(ref map, currentPoint.position.X, currentPoint.position.Y + 1, 'D');
                currentPoint.position.Y += 1;
            }
            currentPoint.position.Y = point.position.Y;
            while (map[currentPoint.position.Y - 1][currentPoint.position.X] == 'O')
            {
                replaceCharacter(ref map, currentPoint.position.X, currentPoint.position.Y - 1, 'D');
                currentPoint.position.Y -= 1;
            }
            currentPoint.position.Y = point.position.Y;
            while (map[currentPoint.position.Y][currentPoint.position.X + 1] == 'O')
            {
                replaceCharacter(ref map, currentPoint.position.X + 1, currentPoint.position.Y, 'D');
                currentPoint.position.X += 1;
            }
            currentPoint.position.X = point.position.X;
            while (map[currentPoint.position.Y][currentPoint.position.X - 1] == 'O')
            {
                replaceCharacter(ref map, currentPoint.position.X - 1, currentPoint.position.Y, 'D');
                currentPoint.position.X -= 1;
            }
            replaceCharacter(ref map, point.position.X, point.position.Y, 'D');
            while (map[pirate.position.Y][pirate.position.X] != 'D')
            {
                if ((map[pirate.position.Y + 1][pirate.position.X] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D') &&
                    (map[pirate.position.Y - 1][pirate.position.X] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D')
                    && (map[pirate.position.Y][pirate.position.X + 1] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D') &&
                    (map[pirate.position.Y][pirate.position.X - 1] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D'))
                {
                    if (pirate.stack.Count != 0)
                    {
                        if (map[pirate.position.Y][pirate.position.X] == 'T')
                        {
                            pirate.stack.Pop();
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                        }
                        while ((map[pirate.position.Y + 1][pirate.position.X] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D') &&
                        (map[pirate.position.Y - 1][pirate.position.X] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D')
                        && (map[pirate.position.Y][pirate.position.X + 1] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D') &&
                        (map[pirate.position.Y][pirate.position.X - 1] != 'O' && map[pirate.position.Y + 1][pirate.position.X] != 'D'))
                        {
                            if (pirate.stack.Count == 0)
                            {
                                return q;
                            }
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                            pirate.position.X = pirate.stack.Peek().position.X;
                            pirate.position.Y = pirate.stack.Peek().position.Y;
                            pirate.stack.Pop();
                        }
                        replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                        pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                        //pirate.position.X = pirate.stack.Peek().x;
                        //pirate.position.Y = pirate.stack.Peek().y;
                    }
                    else
                    {
                    }
                }
                if (pirate.direction == "south")
                {
                    if (map[pirate.position.Y + 1][pirate.position.X] == 'O' || map[pirate.position.Y + 1][pirate.position.X] == 'D')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, '.'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                            pirate.position.Y += 1;
                        }
                        else
                        {
                            pirate.position.Y += 1;
                        }
                    }
                    else if (map[pirate.position.Y + 1][pirate.position.X] == 'X' || map[pirate.position.Y + 1][pirate.position.X] == '.' || map[pirate.position.Y + 1][pirate.position.X] == 'T')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                        }
                        if (point.position.X >= pirate.position.X)
                        {
                            if (map[pirate.position.Y][pirate.position.X + 1] == 'X' || map[pirate.position.Y][pirate.position.X + 1] == '.' || map[pirate.position.Y][pirate.position.X + 1] == 'T')
                                pirate.direction = "west";
                            else
                                pirate.direction = "east";
                        }
                        if (point.position.X < pirate.position.X)
                        {
                            if (map[pirate.position.Y][pirate.position.X - 1] == 'X' || map[pirate.position.Y][pirate.position.X - 1] == '.' || map[pirate.position.Y][pirate.position.X - 1] == 'T')
                                pirate.direction = "east";
                            else
                                pirate.direction = "west";
                        }
                        pirate.preferredDirection = "south";
                    }
                }
                if (pirate.direction == "east")
                {
                    if (map[pirate.position.Y][pirate.position.X + 1] == 'O' || map[pirate.position.Y][pirate.position.X + 1] == 'D')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, '.'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                            pirate.position.X += 1;
                        }
                        else
                        {
                            pirate.position.X += 1;
                        }
                    }
                    else if (map[pirate.position.Y][pirate.position.X + 1] == 'X' || map[pirate.position.Y][pirate.position.X + 1] == '.' || map[pirate.position.Y][pirate.position.X + 1] == 'T')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                        }
                        if (point.position.Y >= pirate.position.Y)
                        {
                            if (map[pirate.position.Y + 1][pirate.position.X] == 'X' || map[pirate.position.Y + 1][pirate.position.X] == '.' || map[pirate.position.Y + 1][pirate.position.X] == 'T')
                                pirate.direction = "north";
                            else
                                pirate.direction = "south";
                        }
                        if (point.position.Y < pirate.position.Y)
                        {
                            if (map[pirate.position.Y - 1][pirate.position.X] == 'X' || map[pirate.position.Y - 1][pirate.position.X] == '.' || map[pirate.position.Y - 1][pirate.position.X] == 'T')
                                pirate.direction = "south";
                            else
                                pirate.direction = "north";
                        }
                        pirate.preferredDirection = "east";
                    }
                }
                if (pirate.direction == "north")
                {
                    if (map[pirate.position.Y - 1][pirate.position.X] == 'O' || map[pirate.position.Y - 1][pirate.position.X] == 'D')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, '.'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                            pirate.position.Y -= 1;
                        }
                        else
                        {
                            pirate.position.Y -= 1;
                        }
                    }
                    else if (map[pirate.position.Y - 1][pirate.position.X] == 'X' || map[pirate.position.Y - 1][pirate.position.X] == '.' || map[pirate.position.Y - 1][pirate.position.X] == 'T')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                        }
                        if (point.position.X >= pirate.position.X)
                        {
                            if (map[pirate.position.Y][pirate.position.X + 1] == 'X' || map[pirate.position.Y][pirate.position.X + 1] == '.' || map[pirate.position.Y][pirate.position.X + 1] == 'T')
                                pirate.direction = "west";
                            else
                                pirate.direction = "east";
                        }
                        if (point.position.X < pirate.position.X)
                        {
                            if (map[pirate.position.Y][pirate.position.X - 1] == 'X' || map[pirate.position.Y][pirate.position.X - 1] == '.' || map[pirate.position.Y][pirate.position.X - 1] == 'T')
                                pirate.direction = "east";
                            else
                                pirate.direction = "west";
                        }
                        pirate.preferredDirection = "north";
                    }
                }
                if (pirate.direction == "west")
                {
                    if (map[pirate.position.Y][pirate.position.X - 1] == 'O' || map[pirate.position.Y][pirate.position.X - 1] == 'D')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, '.'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, '.');
                            pirate.position.X -= 1;
                        }
                        else
                        {
                            pirate.position.X -= 1;
                        }
                    }
                    else if (map[pirate.position.Y][pirate.position.X - 1] == 'X' || map[pirate.position.Y][pirate.position.X - 1] == '.' || map[pirate.position.Y][pirate.position.X - 1] == 'T')
                    {
                        if (map[pirate.position.Y][pirate.position.X] != 'T')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                        }
                        if (point.position.Y >= pirate.position.Y)
                        {
                            if (map[pirate.position.Y + 1][pirate.position.X] == 'X' || map[pirate.position.Y + 1][pirate.position.X] == '.' || map[pirate.position.Y + 1][pirate.position.X] == 'T')
                                pirate.direction = "north";
                            else
                                pirate.direction = "south";
                        }
                        if (point.position.Y < pirate.position.Y)
                        {
                            if (map[pirate.position.Y - 1][pirate.position.X] == 'X' || map[pirate.position.Y - 1][pirate.position.X] == '.' || map[pirate.position.Y - 1][pirate.position.X] == 'T')
                                pirate.direction = "south";
                            else
                                pirate.direction = "north";
                        }
                        pirate.preferredDirection = "west";
                    }
                }
                if (pirate.preferredDirection != pirate.direction)
                {
                    if (pirate.preferredDirection == "south")
                    {
                        if (map[pirate.position.Y + 1][pirate.position.X] == 'O' || map[pirate.position.Y + 1][pirate.position.X] == 'P')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                            pirate.position.Y += 1;
                            pirate.direction = "south";
                        }

                    }
                    if (pirate.preferredDirection == "north")
                    {
                        if (map[pirate.position.Y - 1][pirate.position.X] == 'O' || map[pirate.position.Y - 1][pirate.position.X] == 'D')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                            pirate.position.Y -= 1;
                            pirate.direction = "north";
                        }

                    }
                    if (pirate.preferredDirection == "east")
                    {
                        if (map[pirate.position.Y][pirate.position.X + 1] == 'O' || map[pirate.position.Y][pirate.position.X + 1] == 'D')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                            pirate.position.X += 1;
                            pirate.direction = "east";
                            if (map[pirate.position.Y + 1][pirate.position.X] == 'X')
                            {
                                pirate.preferredDirection = "south";
                            }
                        }

                    }
                    if (pirate.preferredDirection == "west")
                    {
                        if (map[pirate.position.Y][pirate.position.X - 1] == 'O' || map[pirate.position.Y][pirate.position.X - 1] == 'D')
                        {
                            pirate.stack.Push(new PointCharacterCombo(pirate.position.X, pirate.position.Y, 'T'));
                            replaceCharacter(ref map, pirate.position.X, pirate.position.Y, 'T');
                            pirate.position.X -= 1;
                            pirate.direction = "west";
                            if (map[pirate.position.Y + 1][pirate.position.X] == 'X')
                            {
                                pirate.preferredDirection = "south";
                            }
                        }

                    }

                }
            }
            q.Enqueue(new Point(pirate.position.X - 1, pirate.position.Y + 1));
            while (pirate.stack.Count != 0)
            {
                if (pirate.stack.Peek().character == 'T')
                {
                    q.Enqueue(new Point(pirate.stack.Peek().position.X - 1, pirate.stack.Peek().position.Y + 1));
                }
                pirate.stack.Pop();
            }

            return q;
        }
        protected Queue<Point> BestOfFour()
        {
            Queue<Point> best = getGoToPoints("south");
            Queue<Point> north = getGoToPoints("north");
            if (north.Count < best.Count && north.Count > 0)
            {
                best = north;
            }
            else if (north.Count > best.Count && best.Count == 0)
            {
                best = north;
            }

            Queue<Point> east = getGoToPoints("east");
            if (east.Count < best.Count && east.Count > 0)
            {
                best = east;
            }
            else if (east.Count > best.Count && best.Count == 0)
            {
                best = east;
            }

            Queue<Point> west = getGoToPoints("west");
            if (west.Count < best.Count && west.Count > 0)
            {
                best = west;
            }
            else if (west.Count > best.Count && best.Count == 0)
            {
                best = west;
            }

            return best;
        }
    }
}
