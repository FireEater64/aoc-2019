using System;
using System.IO;
using System.Linq;
using MoreLinq;

namespace _08
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Single().Select(x => int.Parse(x.ToString()));

            var NUM_X = 25;
            var NUM_Y = 6;
            var lengthOfLayer = NUM_X * NUM_Y;

            var layers = input.Batch(lengthOfLayer);
            var fewestZeros = layers.MinBy(x => x.Count(y => y == 0)).Single();
            Console.WriteLine($"Part 1: {fewestZeros.Count(x => x == 1) * fewestZeros.Count(x => x == 2)}");
            
            Console.WriteLine("Part 2:");
            for (int y = 0; y < NUM_Y; y++)
            {
                for (int x = 0; x < NUM_X; x++)
                {
                    var offset = y * NUM_X + x;
                    var valueForPixel = input.Skip(offset).TakeEvery(lengthOfLayer).First(v => v != 2);

                    if (valueForPixel == 1)
                        Console.Write('X');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

    }
}
