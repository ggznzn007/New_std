using System;

namespace Delegate_B
{
    class Program
    {
        delegate bool MemberTest(int a);

        static void Main(string[] args)
        {
            int[] arr = new int[] { 3, 5, 4, 2, 6, 4, 6, 8, 54, 23, 4, 6, 4 };

            Console.WriteLine("짝수의 개수: " + Count(arr, IsEven));
            Console.WriteLine("홀수의 개수: " + Count(arr, IsOdd));
        }

        static int Count(int[] a, MemberTest testMethod)
        {
            int cnt = 0;
            foreach(var n in a)
            {
                if (testMethod(n) == true)
                    cnt++;
            }
            return cnt;
        }

        static public bool IsOdd(int n) { return n % 2 != 0; }
        static public bool IsEven(int n) { return n % 2 == 0; }
    }
}