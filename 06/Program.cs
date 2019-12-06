using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace _06
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Select(ParseInput);

            var planets = new ConcurrentDictionary<string, Planet>();

            foreach (var pair in input)
            {
                var source = planets.GetOrAdd(pair.source, new Planet(pair.source));
                var orbiter = planets.GetOrAdd(pair.orbits, new Planet(pair.orbits));
                source.AddOrbitingPlanet(orbiter);
            }

            // Part 1
            var com = planets["COM"];
            Console.WriteLine($"Part 1: {com.CountOrbits(0)}");

            // Part 2
            var me = "YOU";
            var santa = "SAN";
            var lca = FindLCA(com, me, santa);
            var distance = FindLevel(lca, me, 0) + FindLevel(lca, santa, 0);
            Console.WriteLine($"Part 2: {distance}");

        }

        public static int? FindLevel(Planet root, string dest, int depth)
        {
            if (root == null)
                return null;

            if (root.Name == dest)
                return depth;
            
            var l = FindLevel(root.Left, dest, depth + 1);
            if (l.HasValue)
                return l;
            return FindLevel(root.Right, dest, depth + 1);
        }

        public static Planet FindLCA(Planet root, string source, string destination)
        {
            if (root == null)
                return null;

            if (root.Name == source || root.Name == destination)
                return root;

            var leftLca = FindLCA(root.Left, source, destination);
            var rightLca = FindLCA(root.Right, source, destination);

            if (leftLca != null && rightLca != null)
                return root;

            if (leftLca != null)
                return leftLca;

            if (rightLca != null)
                return rightLca;

            return null;
        }

        private static (string source, string orbits) ParseInput(string input) 
        {
            var parts = input.Split(')');
            return (parts[0], parts[1]);
        }
    }

    class Planet
    {
        public string Name { get; private set; }

        public Planet Left { get; private set; }

        public Planet Right { get; private set; }

        public Planet(string name)
        {
            Name = name;
        }

        public void AddOrbitingPlanet(Planet s)
        {
            if (Left == null)
                Left = s;
            else if (Right == null)
                Right = s;
            else 
                throw new ApplicationException("Orbit error");
        }

        public int CountOrbits(int depth) => depth + 
            (Left?.CountOrbits(depth + 1) ?? 0) + 
            (Right?.CountOrbits(depth + 1) ?? 0);

    }
}
