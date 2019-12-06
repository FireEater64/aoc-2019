using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _02
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadLines("input.txt").Single().Split(",").Select(int.Parse);

            // Part 1
            var computer = new IntcodeComputer(code);
            computer.Run(1);

            // Part 2
            computer = new IntcodeComputer(code);
            computer.Run(5);
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

        public IntcodeComputer(IEnumerable<int> givenMemory)
        {
            MEMORY = givenMemory.ToList();
        }

        public void Run(params int[] input)
        {
            var i = input.ToList();
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
                        MEMORY[MEMORY[pc + 1]] = i.First();
                        i.RemoveAt(0);
                        pc += 2;
                        break;
                    case PRINT_OPCODE:
                        Console.WriteLine(param1);
                        pc += 2;
                        break;
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
                        return;
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
