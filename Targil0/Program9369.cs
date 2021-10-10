using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome9369();
            Welcome5259();
            Console.ReadKey();
        }

        static partial void Welcome5259();

        private static void Welcome9369()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}
