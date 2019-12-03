using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace _03
{
    class Program
    {
        private static Dictionary<char, Point> directionLookup = new Dictionary<char, Point>
        {
            {'U', new Point(0, 1)},
            {'D', new Point(0, -1)},
            {'R', new Point(1, 0)},
            {'L', new Point(-1, 0)}
        };

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

            var point = new Point(X_START, Y_START);
            var steps = 1;

            foreach (var instruction in wire1)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));

                var translation = directionLookup[direction];
                for (int i = 0; i < distance; i++)
                {
                    point.Offset(translation);
                    if (board[point.X, point.Y] == 0)
                        board[point.X, point.Y] = steps;
                    steps++;
                }
            }

            var minDistance = int.MaxValue;
            point = new Point(X_START, Y_START);
            steps = 1;

            foreach (var instruction in wire2)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));
                var translation = directionLookup[direction];
                for (int i = 0; i < distance; i++)
                {
                    point.Offset(translation);
                    if (board[point.X, point.Y] > 0)
                        minDistance = Math.Min(minDistance, board[point.X, point.Y] + steps);
                    steps++;
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
