using System;

namespace _ArrayClass
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = { 5, 25, 75, 35, 15 }; // 배열을 선언 후 초기화
            PrintArray(a);

            int[] b;
            b = (int[])a.Clone(); // 배열 복사 방법 1
            PrintArray(b);

            int[] c = new int[10];
            Array.Copy(a, 0, c, 1, 3); // 배열 복사 방법 2
            PrintArray(c);
            
            
        }
        private static void PrintArray(int[] a)
        {
            foreach (var i in a)
                Console.Write("{0,5}", i);
            Console.WriteLine();
        }
    }
}