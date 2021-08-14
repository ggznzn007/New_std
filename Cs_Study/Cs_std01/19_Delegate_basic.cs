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
            //카운트 메소드는 배열과 델리게이트 메소드를 매개변수로 사용
            Console.WriteLine("짝수의 개수: " + Count(arr, IsEven));
            Console.WriteLine("홀수의 개수: " + Count(arr, IsOdd));
        }

        static int Count(int[] a, MemberTest testMethod)
        {//메소드 정의
            int cnt = 0;//배열의 각 요소에 대해 인수로 전달받은 델리게이트의 메소드를 호출하여
            foreach(var n in a)
            {
                if (testMethod(n) == true)//리턴값이 트루면 cnt를 하나씩 증가시켜 리턴
                    cnt++;
            }
            return cnt;//Main에서 출력
        }

        static public bool IsOdd(int n) { return n % 2 != 0; }
        static public bool IsEven(int n) { return n % 2 == 0; }
        //정수를 매개변수로 해서 부울값을 리턴
    }
}