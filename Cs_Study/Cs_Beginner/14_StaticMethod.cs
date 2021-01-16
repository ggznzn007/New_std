using System;

namespace _StaticMethd
{
    class Methods
    {
        static void Main(string[] args)
        {
            int a = 10, b = 30, c = 20;
            //Methods x = new Methods();
            //Console.WriteLine("가장 큰 수는{0}", (x.Larger(a,b),c));
            Console.WriteLine("가장 큰 수는{0}", Larger(Larger(a,b),c));
        }

        private static int Larger(int a, int b) // static입니다.
        {
            return (a >= b) ? a : b;
        }

        /*private static int Larger(int a, int b) // static이 아닙니다.
        {
            return (a >= b) ? a : b;
        }*/
    }
}