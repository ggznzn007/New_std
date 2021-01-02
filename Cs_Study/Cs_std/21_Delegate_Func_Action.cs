using System;

namespace Func_Action
{
    class Program
    {
        //delegate bool MemberTest(int x); //*** Func를 사용하므로 필요없음

        static void Main(string[] args)
        {
            var arr = new[] { 3, 34, 6, 34, 7, 8, 24, 3, 675, 8, 23 };
            //이름없는 무명 델리게이트로 정의
            int n = Count(arr, delegate (int x) { return x % 2 == 0; });//배열과 델리게이트가 매개변수
            Console.WriteLine("짝수의 개수: " + n);
        }

        private static int Count(int[] arr, Func<int, bool>testMethod)
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