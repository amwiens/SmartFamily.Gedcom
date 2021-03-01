using SmartFamily.Gedcom.Helpers;

using System;

namespace GedcomTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var startTime = DateTime.Now;



            var endTime = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine($"Start Time: {startTime}");
            Console.WriteLine($"End Time: {endTime}");
            Console.WriteLine($"Processing Time: {endTime.Subtract(startTime)}");
        }
    }
}