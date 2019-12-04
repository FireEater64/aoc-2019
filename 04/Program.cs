using System;
using System.Linq;

namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            var min = 382345;
            var max = 843167;

            var range = Enumerable.Range(min, max - min + 1);

            Console.WriteLine($"Part 1 : {range.Count(IsValidRule1)}");
            Console.WriteLine($"Part 2 : {range.Count(IsValidRule2)}");
        }

        static bool IsValidRule1(int given)
        {
            var digits = GetDigits(given);
            var pairs = digits.Zip(digits.Skip(1), (a, b) => (First: a, Second: b));
            return digits.Count() == 6 && pairs.Any(x => x.First == x.Second) && pairs.All(x => x.First <= x.Second);
        }

        static bool IsValidRule2(int given)
        {
            var digits = GetDigits(given);
            var pairs = digits.Zip(digits.Skip(1), (a, b) => (First: a, Second: b));
            var digitGroup = digits.GroupBy(x => x);
            return digits.Count() == 6 && digitGroup.Any(x => x.Count() == 2) && pairs.All(x => x.First <= x.Second);
        }

        private static int[] GetDigits(int given) => 
            given.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
    }
}
