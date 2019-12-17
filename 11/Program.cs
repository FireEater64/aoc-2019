using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace _11
{
    class Program
    {
        private static List<Point> positions = new List<Point>
        {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0),
        };

        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Single().Split(",").Select(long.Parse).ToArray();

            var coloursPart1 = RunPaintJob(input, 0);

            Console.WriteLine($"Part 1: {coloursPart1.Count()}");

            var coloursPart2 = RunPaintJob(input, 1);

            // Print the damn board
            Console.WriteLine("Part 2:");

            var minX = coloursPart2.Min(x => x.Key.X);
            var minY = coloursPart2.Min(x => x.Key.Y);
            var maxX = coloursPart2.Max(x => x.Key.X);
            var maxY = coloursPart2.Max(x => x.Key.Y);
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    if (coloursPart2.GetValueOrDefault(new Point(x, y), 0) == 1)
                        Console.Write("#");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static Dictionary<Point, int> RunPaintJob(long[] program, int initialColour)
        {
            var computer = new IntcodeComputer(program, new List<int>());

            var robotPosition = new Point(0, 0);
            var robotOrientaton = 0;
            var colours = new Dictionary<Point, int>
            {
                {new Point(0, 0), initialColour}
            };

            computer.Input.Add(colours.GetValueOrDefault(robotPosition, 0));
            var output = computer.Run();

            while(output != null)
            {
                // First read the painted colour
                colours[robotPosition] = (int)output;

                // Then read the new direction
                var turnValue = computer.Run();
                robotOrientaton = turnValue == 1 ?
                    (robotOrientaton + 1) % 4 : 
                    robotOrientaton - 1;
                if (robotOrientaton < 0) robotOrientaton = 3; // TODO: ???

                robotPosition.Offset(positions[robotOrientaton]);

                // Finally, read input for next move
                computer.Input.Add(colours.GetValueOrDefault(robotPosition, 0));
                output = computer.Run();
            }

            return colours;
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
