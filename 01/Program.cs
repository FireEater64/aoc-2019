using System;
using System.IO;
using System.Linq;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Select(long.Parse);

            var part1 = input.Select(CalculateFuel).Sum();
            Console.WriteLine($"Part 1: {part1}");

            var part2 = input.Select(CalculateFuelRecursive).Sum();
            Console.WriteLine($"Part 2: {part2}");
        }

        static long CalculateFuel(long moduleMass) => (moduleMass / 3) - 2;

        static long CalculateFuelRecursive(long moduleMass) 
        {
            var required = CalculateFuel(moduleMass);
            return required > 0 ? required + CalculateFuel(required) : 0;
        }
    }
}
