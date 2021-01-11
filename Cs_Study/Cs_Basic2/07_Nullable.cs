using System;

namespace _Nullable
{
    class Program
    {
        static void Main(string[] args)
        {
            Nullable<int> i = null;
            Console.WriteLine(i.GetValueOrDefault());
        }
    }
}