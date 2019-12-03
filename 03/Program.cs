using System;
using System.IO;
using System.Linq;

namespace _03
{
    class Program
    {
        private static int X_START = 25000;
        private static int Y_START = 25000;

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var wire1 = lines[0].Split(",");
            var wire2 = lines[1].Split(",");
            //var wire1 = new[] { "R75", "D30", "R83", "U83", "L12", "D49", "R71", "U7", "L72" };
            //var wire2 = new[] { "U62", "R66", "U55", "R34", "D71", "R55", "D58", "R83" };

            var board = new int[50000, 50000];
            var startX = X_START;
            var startY = Y_START;
            var steps = 0;

            foreach (var instruction in wire1)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));

                switch (instruction[0])
                {
                    case 'U':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY + y] == 0)
                                board[startX, startY + y] = steps;
                            steps++;
                        }
                        startY += distance;
                        break;
                    case 'R':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX + x, startY] == 0)
                                board[startX + x, startY] = steps;
                            steps++;
                        }
                        startX += distance;
                        break;
                    case 'L':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX - x, startY] == 0)
                                board[startX - x, startY] = steps;
                            steps++;
                        }
                        startX -= distance;
                        break;
                    case 'D':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY - y] == 0)
                                board[startX, startY - y] = steps;
                            steps++;
                        }
                        startY -= distance;
                        break;
                    default:
                        throw new ApplicationException("Invalid instruction");
                }
            }

            var minDistance = int.MaxValue;
            startX = X_START;
            startY = Y_START;
            steps = 0;

            foreach (var instruction in wire2)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));

                switch (instruction[0])
                {
                    case 'U':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY + y] > 0)
                                minDistance = Math.Min(minDistance, board[startX, startY + y] + steps);
                            steps++;
                        }
                        startY += distance;
                        break;
                    case 'R':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX + x, startY] > 0)
                                minDistance = Math.Min(minDistance, board[startX + x, startY] + steps);
                            steps++;
                        }
                        startX += distance;
                        break;
                    case 'L':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX - x, startY] > 0)
                                minDistance = Math.Min(minDistance, board[startX - x, startY] + steps);
                            steps++;
                        }
                        startX -= distance;
                        break;
                    case 'D':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY - y] > 0)
                                minDistance = Math.Min(minDistance, board[startX, startY - y] + steps);
                            steps++;
                        }
                        startY -= distance;
                        break;
                    default:
                        throw new ApplicationException("Invalid instruction");
                }
            }

            Console.WriteLine(minDistance);
        }

        static int CalcDistance(int x, int y)
        {
            var distance = Math.Abs(x - X_START) + Math.Abs(y - Y_START);
            if (distance == 0)
                return int.MaxValue;
            return distance;
        }
    }
}
