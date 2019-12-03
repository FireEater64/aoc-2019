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

            var board = new bool[50000, 50000];
            var startX = X_START;
            var startY = Y_START;

            foreach (var instruction in wire1)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));

                switch (instruction[0])
                {
                    case 'U':
                        for (int y = 0; y < distance; y++)
                            board[startX, startY + y] = true;
                        startY += distance;
                        break;
                    case 'R':
                        for (int x = 0; x < distance; x++)
                            board[startX + x, startY] = true;
                        startX += distance;
                        break;
                    case 'L':
                        for (int x = 0; x < distance; x++)
                            board[startX - x, startY] = true;
                        startX -= distance;
                        break;
                    case 'D':
                        for (int y = 0; y < distance; y++)
                            board[startX, startY - y] = true;
                        startY -= distance;
                        break;
                    default:
                        throw new ApplicationException("Invalid instruction");
                }
            }

            var minDistance = int.MaxValue;
            startX = X_START;
            startY = Y_START;

            foreach (var instruction in wire2)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));

                switch (instruction[0])
                {
                    case 'U':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY + y])
                                minDistance = Math.Min(minDistance, CalcDistance(startX, startY + y));
                        }
                        startY += distance;
                        break;
                    case 'R':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX + x, startY])
                                minDistance = Math.Min(minDistance, CalcDistance(startX + x, startY));
                        }
                        startX += distance;
                        break;
                    case 'L':
                        for (int x = 0; x < distance; x++)
                        {
                            if (board[startX - x, startY])
                                minDistance = Math.Min(minDistance, CalcDistance(startX - x, startY));
                        }
                        startX -= distance;
                        break;
                    case 'D':
                        for (int y = 0; y < distance; y++)
                        {
                            if (board[startX, startY - y])
                                minDistance = Math.Min(minDistance, CalcDistance(startX, startY - y));
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
