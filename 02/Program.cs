using System;
using System.IO;
using System.Linq;

namespace _02
{
    class Program
    {
        private static readonly int ADD_OPCODE = 1;
        private static readonly int MUL_OPCODE = 2;
        private static readonly int EXIT_OPCODE = 99;

        static void Main(string[] args)
        {
            // Part 1
            Console.WriteLine($"Part 1: {RunProgram(12, 2)}");

            // Part 2
            var target = 19690720;

            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    if (RunProgram(noun, verb) == target)
                    {
                        Console.WriteLine($"Part 2: {100 * noun + verb}");
                        return;
                    }
                }
            }
        }

        static int RunProgram(int initialNoun, int initialVerb)
        {
            var mem = File.ReadLines("input.txt").Single().Split(",").Select(int.Parse).ToList();

            // Restore state
            mem[1] = initialNoun;
            mem[2] = initialVerb;

            var pc = 0;

            while (mem[pc] != EXIT_OPCODE)
            {
                var opcode = mem[pc];
                var address1 = mem[pc + 1];
                var address2 = mem[pc + 2];
                var address3 = mem[pc + 3];

                if (opcode == ADD_OPCODE)
                    mem[address3] = mem[address1] + mem[address2];
                else if (opcode == MUL_OPCODE)
                    mem[address3] = mem[address1] * mem[address2];


                pc += 4;
            }

            return mem[0];
        }
    }
}
