using System;

namespace Predicate01
{
    class Program
    {
        static void Main(string[] args)
        {
            Predicate<int> isEven = n => n % 2 == 0;// 람다식으로 정의
            Console.WriteLine(isEven(6));//정수하나를 매개변수로 부울값 리턴

            Predicate<string> isLowerCase = s => s.Equals(s.ToLower());
            Console.WriteLine(isLowerCase("This is test"));
            /*isLowerCase는 string s를 매개변수로 보내고 s를 소문자로 바꾼 결과와 같은지를
              리턴값으로 보내준다. 맨 앞글자가 대문자 T이므로 false 출력*/
        }
    }
}