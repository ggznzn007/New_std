using System;

namespace Delegate_Anonymous
{
    class Program
    {
        delegate bool MemberTest(int x);//델리게이트 생성
        //매개변수로 정수 하나를 사용하고 부울값을 리턴하는 메소드 사용가능
        static void Main(string[] args)
        {
            var arr = new[] { 3, 34, 6, 34, 7, 8, 24, 3, 675, 8, 23 };

            int n = Count(arr, delegate (int x) { return x % 2 == 0; });
            Console.WriteLine("짝수의 개수: " + n);

            n = Count(arr, delegate (int x) { return x % 2 != 0; });
            Console.WriteLine("홀수의 개수: " + n);
        }

        private static int Count(int[] arr, MemberTest testMethod)
        {
            int cnt = 0;
            foreach(var n in arr)
            {
                if (testMethod(n))
                    cnt++;
            }
            return cnt;
        }
    }
}