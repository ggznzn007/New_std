using System;

namespace Lamda_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new[] { 3, 34, 6, 34, 7, 8, 24, 3, 675, 8, 23 };

            int n = Count(arr, x => x % 2 == 0);//카운트 메소드는 배열과 람다식을 매개변수로 사용
            Console.WriteLine("짝수의 개수: " + n);

            n = Count(arr, x => x % 2 == 1);
            Console.WriteLine("홀수의 개수: " + n);
        }

        private static int Count(int[] arr, Func<int,bool> testMethod)
        {//카운트 메소드 정의
            //전달된 람다식은 Func<>로 받음
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