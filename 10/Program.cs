using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using MoreLinq;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = CalculateAngle(new Point(8,3), new Point(7, 0));
            var input = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToList();
            var points = GetPoints(input).ToList();

            var home = points.MaxBy(x => CountVisible(x, points)).Single();
            Console.WriteLine($"Part 1: {home} - {CountVisible(home, points)}");

            var toDestroy = points.ToList();
            toDestroy.Remove(home); // We're not going to laser ourselves (I hope)

            // Build the spiral
            var orderedTargets = toDestroy.OrderBy(x => CalculateAngle(home, x)).ThenBy(x => CalculateDistance(home, x));
            var groups = orderedTargets.GroupBy(x => CalculateAngle(home, x)).ToDictionary(x => x.Key, x => x.ToList());
            var targetBuckets = new SortedDictionary<double, List<Point>>(groups);

            var target = 200;
            var current = 1;
            while(current < target)
            {
                foreach(var bucket in targetBuckets)
                {
                    // Empty group
                    if (!bucket.Value.Any())
                        continue;

                    var destroyed = bucket.Value.First();
                    bucket.Value.RemoveAt(0);

                    if (current == target)
                    {
                        // O o
                        // /¯_____________________________________
                        // | BLAAAAAAAAAAAAAAAAAAAHHHHHHHH!!!!!
                        // \_‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾ 

                        Console.WriteLine($"Part 2: {destroyed} - {destroyed.X * 100 + destroyed.Y}");
                        break;
                    }

                    current++;
                }
            }
        }

        static int CountVisible(Point p, List<Point> universe)
        {
            var angles = universe.Where(x => x != p).Select(x => CalculateAngle(p, x));
            return angles.Distinct().Count();
        }

        static double CalculateAngle(Point p1, Point p2)
        {
            var xDiff = (p2.X - p1.X);
            var yDiff = (p1.Y - p2.Y);
            var degrees = RadToDegrees(Math.Atan2(xDiff, yDiff));
            return (degrees >= 0) ? degrees : 360 + degrees;
        }

        static double RadToDegrees(double rad) => rad * (180 / Math.PI);

        static double CalculateDistance(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2)+Math.Pow(p1.Y - p2.Y, 2));

        static IEnumerable<Point> GetPoints(List<char[]> input)
        {
            // Assume square universe
            var height = input.Count();
            var length = input.First().Count();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < length; x++)
                    if (input[y][x] == '#')
                        yield return new Point(x, y);
        }
    }
}
