using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _07
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = File.ReadLines("input.txt").Single().Split(",").Select(int.Parse);

            var maxOutput = 0;
            foreach (var perm in GetPermutations(Enumerable.Range(0, 5), 5))
            {
                var inputSignal = 0;
                var output = int.MinValue;

                // Run all amplifiers
                for (int i = 0; i < 5; i++)
                {
                    var computer = new IntcodeComputer(program, new List<int> { perm[i], inputSignal });
                    output = computer.Run().Value;

                    inputSignal = output;
                }

                // We're looking for the largest output on the final stage
                maxOutput = Math.Max(maxOutput, output);
            }

            Console.WriteLine($"Part 1: {maxOutput}");

            // ---------- Part 2 -----------
            foreach (var perm in GetPermutations(Enumerable.Range(5, 5), 5))
            {
                int? output = 0;
                int result = 0;

                // Create each computer with its respective phase setting as input
                var computers = Enumerable.Range(0, 5).Select(x => new IntcodeComputer(program, new List<int> { perm[x] })).ToArray();

                // Seed the first computer with input 0
                computers[0].Input.Add(0);

                while (output.HasValue)
                {
                    // Run all amplifiers
                    for (int i = 0; i < 5; i++)
                    {
                        var computer = computers[i];
                        output = computer.Run();
                        if (output.HasValue)
                            computers[(i + 1) % 5].Input.Add(output.Value); // Pass to next computer in the chain
                    }

                    if (output.HasValue)
                        result = output.Value;
                }

                // We're looking for the largest output on the final stage
                maxOutput = Math.Max(maxOutput, result);
            }

            Console.WriteLine($"Part 2: {maxOutput}");
        }

        static IEnumerable<T[]> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
        }
    }

    class IntcodeComputer
    {
        private List<int> MEMORY;
        private int pc = 0;
        private const int ADD_OPCODE = 1;
        private const int MUL_OPCODE = 2;
        private const int IN_OPCODE = 3;
        private const int PRINT_OPCODE = 4;
        private const int JIT_OPCODE = 5;
        private const int JIF_OPCODE = 6;
        private const int LT_OPCODE = 7;
        private const int EQ_OPCODE = 8;
        private const int EXIT_OPCODE = 99;

        public List<int> Input { get; private set; }


        public IntcodeComputer(IEnumerable<int> givenMemory, List<int> input = null)
        {
            Input = input;
            MEMORY = givenMemory.ToList();
        }

        public int? Run()
        {
            while (true)
            {
                var instruction = MEMORY[pc];
                var opcode = GetOpcode(instruction);

                var mode1 = (instruction / 100) % 10;
                var mode2 = (instruction / 1000) % 10;
                var mode3 = (instruction / 10000) % 10;

                var param1 = GetParam(mode1, pc + 1);
                var param2 = GetParam(mode2, pc + 2);
                var param3 = GetParam(mode3, pc + 3);

                switch (opcode)
                {
                    case ADD_OPCODE:
                        MEMORY[MEMORY[pc + 3]] = param1.Value + param2.Value;
                        pc += 4;
                        break;
                    case MUL_OPCODE:
                        MEMORY[MEMORY[pc + 3]] = param1.Value * param2.Value;
                        pc += 4;
                        break;
                    case IN_OPCODE:
                        MEMORY[MEMORY[pc + 1]] = Input.First();
                        Input.RemoveAt(0);
                        pc += 2;
                        break;
                    case PRINT_OPCODE:
                        pc += 2;
                        return param1;
                    case JIT_OPCODE:
                        if (param1.Value != 0) pc = param2.Value;
                        else pc += 3;
                        break;
                    case JIF_OPCODE:
                        if (param1.Value == 0) pc = param2.Value;
                        else pc += 3;
                        break;
                    case LT_OPCODE:
                        MEMORY[MEMORY[pc + 3]] = param1 < param2 ? 1 : 0;
                        pc += 4;
                        break;
                    case EQ_OPCODE:
                        MEMORY[MEMORY[pc + 3]] = param1 == param2 ? 1 : 0;
                        pc += 4;
                        break;
                    case EXIT_OPCODE:
                        return null;
                    default:
                        throw new ApplicationException($"Unknown opcode: {opcode}");
                }
            }
        }

        private int? GetParam(int mode, int offset)
        {
            return mode == 0 ?
                MEMORY.ElementAtOrDefault(MEMORY.ElementAtOrDefault(offset)) :
                MEMORY.ElementAtOrDefault(offset);
        }

        private int GetOpcode(int raw)
        {
            return raw % 100;
        }
    }
}
