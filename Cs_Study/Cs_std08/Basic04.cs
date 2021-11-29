using System;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            int errorCode = 1200;

            string s = ErrorDescription(errorCode);

            Console.WriteLine(s);
        }

        static string ErrorDescription(int errorCode)
        {
            string desc;

            switch (errorCode)
            {
                case 1100:
                    desc = "입력 데이타가 없습니다";
                    break;
                case 1200:
                    desc = "잘못된 입력값입니다";
                    break;
                case 1300:
                    desc = "입력 범위를 초과하였습니다";
                    break;
                default:
                    desc = "Unknown Error";
                    break;
            }

            return desc;
        }
    }
}



  /*  namespace Day5
{
    class Program2
    {
        static void Main(string[] args)
        {
            // 배열을 foreach 를 사용하여 출력
            string[] s = { "Jim", "Sam", "Kim", "Park" };

            foreach (string x in s)
            {
                Console.WriteLine(x);
            }

            // while문으로 1부터 100까지 합계 구하기
            int sum = 0;
            int i = 1;

            while (i <= 100)
            {
                sum = sum + i;
                i++;
            }
            Console.WriteLine(sum);

            // do...while 을 사용한 반복문
            // 아래는 while 문이 거짓이 되지만
            // Hello를 한번은 출력함
            int j = 0;
            do
            {
                Console.WriteLine("Hello");
                j++;
            } while (j < 0);
        }
    }
}*/



  /*  namespace Day5
{
    class Program3
    {
        static void Main(string[] args)
        {
            int a = 100;
            int b = 0;

            try
            {
                int c = a / b;
                Console.WriteLine(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}*/