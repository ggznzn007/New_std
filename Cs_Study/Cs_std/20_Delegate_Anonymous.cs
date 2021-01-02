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

            int n = Count(arr, delegate (int x) { return x % 2 == 0; });//배열과 델리게이트가 매개변수
            Console.WriteLine("짝수의 개수: " + n);
            //델리게이트를 이름없이 인라인으로 직접 정의 가능 == 무명 델리게이트
            //따라서 따로 짝수 홀수 메소드를 정의할 필요없음
            n = Count(arr, delegate (int x) { return x % 2 != 0; });
            Console.WriteLine("홀수의 개수: " + n);
        }

        private static int Count(int[] arr, MemberTest testMethod)
        {//메소드 정의
            int cnt = 0;//배열의 각 요소에 대해 인수로 전달받은 델리게이트의 메소드를 호출하여
            foreach(var n in arr)
            {
                if (testMethod(n))
                    cnt++;//리턴값이 트루면 cnt를 하나씩 증가시켜 리턴
            }
            return cnt;//Main에서 출력
        }
    }
}