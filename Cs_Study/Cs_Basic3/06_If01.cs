using System;

namespace _7_if
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 27;
            if (i % 3 == 0)
                Console.WriteLine("i는 홀수입니다.");
            else
                Console.WriteLine("i는 짝수입니다.");
        }
    }
}