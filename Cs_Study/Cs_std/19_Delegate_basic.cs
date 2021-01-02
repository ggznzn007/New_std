using System;

namespace Delegate_B
{
    class Program
    {
        delegate bool MemberTest(int a);//델리게이트 생성
        //매개변수로 정수 하나를 사용하고 리턴값이 부울인 메소드 사용가능
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