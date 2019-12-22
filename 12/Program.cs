using System;
using System.IO;
using System.Linq;
using MoreLinq;
using MathNet.Numerics;

namespace _12
{
    class Program
    {
        private static Moon[] moons;

        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Select(x => new Moon(x));

            moons = input.ToArray();
            for (int i = 0; i < 1000; i++)
            {
                RunSimulation();
            }

            Console.WriteLine($"Part 1: {moons.Sum(x => x.TotalEnergy)}");

            moons = input.ToArray();
            var initial = input.ToArray();

            var zeroVelocity = new Point();
            var steps = 1;

            var stepMap = new long?[3];

            RunSimulation();
            while(!stepMap.All(x => x.HasValue))
            {
                RunSimulation();
                steps++;

                if (moons.Select((moon, idx) => (moon, idx)).All(m => m.moon.Velocity.X == 0 && m.moon.Position.X == initial[m.idx].Position.X))
                    stepMap[0] = steps;
                if (moons.Select((moon, idx) => (moon, idx)).All(m => m.moon.Velocity.Y == 0 && m.moon.Position.Y == initial[m.idx].Position.Y))
                    stepMap[1] = steps;
                if (moons.Select((moon, idx) => (moon, idx)).All(m => m.moon.Velocity.Z == 0 && m.moon.Position.Z == initial[m.idx].Position.Z))
                    stepMap[2] = steps;
            }

            Console.WriteLine($"Part 2: {Euclid.LeastCommonMultiple(stepMap.Select(x => (long) x.Value).ToList())}");
        }

        private static void RunSimulation()
        {
            // Apply Gravity
            foreach (var (first, second) in moons.Subsets(2).Select(x => (x[0], x[1])))
            {
                if (first.Position.X < second.Position.X) { first.Velocity.X++; second.Velocity.X--; }
                if (first.Position.X > second.Position.X) { first.Velocity.X--; second.Velocity.X++; }
                if (first.Position.Y < second.Position.Y) { first.Velocity.Y++; second.Velocity.Y--; }
                if (first.Position.Y > second.Position.Y) { first.Velocity.Y--; second.Velocity.Y++; }
                if (first.Position.Z < second.Position.Z) { first.Velocity.Z++; second.Velocity.Z--; }
                if (first.Position.Z > second.Position.Z) { first.Velocity.Z--; second.Velocity.Z++; }
            }

            // Apply velocity
            foreach (var moon in moons)
            {
                moon.Position += moon.Velocity;
            }
        }
    }

    class Moon
    {
        public Point Position { get; set; }
        public Point Velocity { get; set; }
        public Moon(string raw)
        {
            var parts = raw.Split(",").Select(int.Parse).ToArray();
            Position = new Point { X = parts[0], Y = parts[1], Z = parts[2] };
            Velocity = new Point();
        }

        private int PotentialEnergy =>
            Math.Abs(this.Position.X) + Math.Abs(this.Position.Y) + Math.Abs(this.Position.Z);
        private int KineticEnergy =>
            Math.Abs(this.Velocity.X) + Math.Abs(this.Velocity.Y) + Math.Abs(this.Velocity.Z);

        public int TotalEnergy => this.PotentialEnergy * this.KineticEnergy;
    }

    class Point 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y &&
                   Z == point.Z;
        }

        public bool Equals(Point other)
        {
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";
        }

        public static Point operator +(Point a, Point b) => new Point { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };

    }
}
