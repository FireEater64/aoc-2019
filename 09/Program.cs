using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _09
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Single().Split(",").Select(long.Parse);//new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99};

            var computer1 = new IntcodeComputer(input, new List<int> { 1 });

            Console.WriteLine($"Part 1: {computer1.Run()}");

            var computer2 = new IntcodeComputer(input, new List<int> { 2 });

            Console.WriteLine($"Part 2: {computer2.Run()}");
        }
    }

    class IntcodeComputer
    {
        private Dictionary<long, long> MEMORY;
        private long pc = 0;
        private long relative = 0;
        private const int ADD_OPCODE = 1;
        private const int MUL_OPCODE = 2;
        private const int IN_OPCODE = 3;
        private const int PRINT_OPCODE = 4;
        private const int JIT_OPCODE = 5;
        private const int JIF_OPCODE = 6;
        private const int LT_OPCODE = 7;
        private const int EQ_OPCODE = 8;
        private const int REL_OPCODE = 9;
        private const int EXIT_OPCODE = 99;

        public List<int> Input { get; private set; }


        public IntcodeComputer(IEnumerable<long> givenMemory, List<int> input = null)
        {
            Input = input;
            MEMORY = givenMemory.Select((v, i) => (i, v)).ToDictionary(x => (long) x.i, x => x.v);
        }

        public long? Run()
        {
            while (true)
            {
                var instruction = MEMORY[pc];
                var opcode = GetOpcode(instruction);

                var mode1 = (int)(instruction / 100) % 10;
                var mode2 = (int)(instruction / 1000) % 10;
                var mode3 = (int)(instruction / 10000) % 10;

                var addr1 = GetAddress(mode1, 1);
                var addr2 = GetAddress(mode2, 2);
                var addr3 = GetAddress(mode3, 3);

                long param1 = MEMORY.GetValueOrDefault(addr1, 0);
                long param2 = MEMORY.GetValueOrDefault(addr2, 0);

                switch (opcode)
                {
                    case ADD_OPCODE:
                        MEMORY[addr3] = param1 + param2;
                        pc += 4;
                        break;
                    case MUL_OPCODE:
                        MEMORY[addr3] = param1 * param2;
                        pc += 4;
                        break;
                    case IN_OPCODE:
                        MEMORY[addr1] = Input.First();
                        Input.RemoveAt(0);
                        pc += 2;
                        break;
                    case PRINT_OPCODE:
                        pc += 2;
                        return param1;
                    case JIT_OPCODE:
                        if (param1 != 0) pc = param2;
                        else pc += 3;
                        break;
                    case JIF_OPCODE:
                        if (param1 == 0) pc = param2;
                        else pc += 3;
                        break;
                    case LT_OPCODE:
                        MEMORY[addr3] = param1 < param2 ? 1 : 0;
                        pc += 4;
                        break;
                    case EQ_OPCODE:
                        MEMORY[addr3] = param1 == param2 ? 1 : 0;
                        pc += 4;
                        break;
                    case REL_OPCODE:
                        relative += param1;
                        pc += 2;
                        break;
                    case EXIT_OPCODE:
                        return null;
                    default:
                        throw new ApplicationException($"Unknown opcode: {opcode}");
                }
            }
        }

        private long GetAddress(int mode, int offset)
        {
            switch (mode)
            {
                case 0: return MEMORY.GetValueOrDefault(pc + offset, 0);
                case 1: return pc + offset;
                case 2: return relative + MEMORY.GetValueOrDefault(pc + offset, 0);
                default: throw new ApplicationException($"Unknown parameter mode: {mode}");
            }
        }

        private long GetOpcode(long raw)
        {
            return raw % 100;
        }
    }
}
