using System;

namespace _Power
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\t\t<< n의 m승을 계산하는 메소드 >>\t\t");
            for (int i = 0; i <= 20; i++)
                Console.WriteLine("2^ {0,2} = {1,7}", i, Power(2, i));
        }

        private static int Power(int n, int m)
        {
            int p = 1;
            for (int i = 1; i <= m; i++)
                p *= n;
            return p;
        }
    }
}