﻿using System;
using System.Globalization;

namespace aoc2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Day04p2 solver = new Day04p2(args[0]);
            int result = solver.Solve();
            Console.WriteLine($"Result is : {result}");
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}
