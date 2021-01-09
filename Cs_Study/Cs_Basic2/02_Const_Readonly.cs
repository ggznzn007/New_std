using System;

namespace _Const_Readonly
{
    class ConstEx
    {
        public const int number = 3;
    }

    class ReadonlyEx
    {
        public readonly int number = 10;
        public ReadonlyEx()
        {
            number = 20;
        }
        public ReadonlyEx(int n)
        {
            number = n;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ConstEx.number); // const사용


        }
    }
}