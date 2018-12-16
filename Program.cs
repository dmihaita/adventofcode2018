using System;
using System.Globalization;

namespace aoc2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Day14p2.Day14p2 solver = new Day14p2.Day14p2(args[0]);
            var result = solver.Solve();
            Console.WriteLine($"Result is : {result}");
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}
