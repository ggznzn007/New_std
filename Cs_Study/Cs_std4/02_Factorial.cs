using System;

namespace Facto
{
    class Pro
    {
        public static void Main()
        {
            int n, fact = 1;

            Console.Write("Enter the number: ");
            n = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= n; i++)
            {
                fact = fact * i;
            }

            Console.Write("Factorial of " + n + " is " + fact);
            Console.ReadKey(true);
        }
    }
}