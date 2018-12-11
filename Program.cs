using System;
using System.Globalization;

namespace aoc2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Day08p1 solver = new Day08p1(args[0]);
            var result = solver.Solve();
            Console.WriteLine($"Result is : {result}");
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}
