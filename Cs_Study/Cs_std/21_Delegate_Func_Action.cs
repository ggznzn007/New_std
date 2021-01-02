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

        private static int Count(int[] arr, Func<int, bool>testMethod)//Count() 메소드 정의
        {//델리게이트 대신에 Func<> 사용 매개변수로 int와 리턴값으로 부울값을 가진다
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