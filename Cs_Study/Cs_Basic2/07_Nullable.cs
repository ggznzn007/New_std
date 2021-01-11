using System;

namespace _Nullable
{
    class Program
    {
        static void Main(string[] args)
        {
            Nullable<int> i = null;
            Console.WriteLine(i.GetValueOrDefault());

            if (i.HasValue)
                Console.WriteLine(i.Value);
            else
                Console.WriteLine("Null");

            int? x = null;
            int j = x ?? 0;
            Console.WriteLine("x = {0}, j = {1}", x, j);


            
        }
    }
}