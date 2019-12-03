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

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var wire1 = lines[0].Split(",");
            var wire2 = lines[1].Split(",");

            var pointsVisitedWire1 = TraceLine(wire1);
            var pointsVisitedWire2 = TraceLine(wire2);

            var pointsVisitedByBothWires = pointsVisitedWire1.Intersect(pointsVisitedWire2)
                .Where(x => x != new Point());

            var distance1 = pointsVisitedWire1.Select((p, i) => new { Point = p, Index = i })
                .GroupBy(x => x.Point).ToDictionary(x => x.Key, x => x.First().Index);
            var distance2 = pointsVisitedWire2.Select((p, i) => new { Point = p, Index = i })
                .GroupBy(x => x.Point).ToDictionary(x => x.Key, x => x.First().Index);

            Console.WriteLine($"Part 1: {pointsVisitedByBothWires.Select(x => CalcDistance(x)).Min()}");
            Console.WriteLine($"Part 2: {pointsVisitedByBothWires.Select(x => distance1[x] + distance2[x]).Min()}");
        }

        private static IEnumerable<Point> TraceLine(IEnumerable<string> instructions)
        {
            // Start at 0, 0
            var point = new Point();
            yield return point;

            foreach (var instruction in instructions)
            {
                var direction = instruction[0];
                var distance = int.Parse(instruction.Substring(1));
                var translation = directionLookup[direction];
                for (int i = 0; i < distance; i++)
                {
                    point.Offset(translation);
                    yield return point;
                }
            }
        }

        private static int CalcDistance(Point p)
        {
            var distance = Math.Abs(p.X) + Math.Abs(p.Y);
            if (distance == 0)
                return int.MaxValue;
            return distance;
        }
    }
}
