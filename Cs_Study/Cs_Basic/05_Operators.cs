using System;

namespace _Operators
{
    class Program
    {
        static void Main(string[] args)
        {
            /*// 01 Operators
            Console.WriteLine(3 + 4 * 5);
            Console.WriteLine((3 + 4) * 5);
            Console.WriteLine(3 * 4 / 5);
            Console.WriteLine(4 / 5 * 3);

            int a = 10, b = 20, c;
            Console.WriteLine(c = a + b);*/

            /*// 02 ArithmeticOperators
            Console.WriteLine("정수의 계산");
            Console.WriteLine(123 + 45);
            Console.WriteLine(123 - 45);
            Console.WriteLine(123 * 45);
            Console.WriteLine(123 / 45);
            Console.WriteLine(123 % 45);

            Console.WriteLine("\n실수의 계산");
            Console.WriteLine(123.45 + 67.89);
            Console.WriteLine(123.45 - 67.89);
            Console.WriteLine(123.45 * 67.89);
            Console.WriteLine(123.45 / 67.89);
            Console.WriteLine(123.45 % 67.89);*/

            /*// 03 DivdeByZero, try ~ catch == 예외처리
            int x = 10, y = 0;

            try
            {
                Console.WriteLine(x / y);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }*/

            /*// 04 OverflowException_checked
            *//*Console.WriteLine("int.MaxValue = {0}", int.MaxValue);
            int x = int.MaxValue, y = 0;
            y = x + 10;
            Console.WriteLine("int.MaxValue +10 = {0}", y);*//*

            int x = int.MaxValue, y = 0;

            checked
            {
                try
                {
                    y = x + 10;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("int.MaxValue + 10 = {0}", y);*/

            /*// 05 Relational Operators
            bool result;
            int first = 10, second = 20;

            result = (first == second);
            Console.WriteLine("{0} == {1} : {2}", first, second, result);

            result = (first > second);
            Console.WriteLine("{0} > {1} : {2}", first, second, result);

            result = (first < second);
            Console.WriteLine("{0} < {1} : {2}", first, second, result);

            result = (first >= second);
            Console.WriteLine("{0} >= {1} : {2}", first, second, result);

            result = (first <= second);
            Console.WriteLine("{0} <= {1} : {2}", first, second, result);

            result = (first != second);
            Console.WriteLine("{0} != {1} : {2}", first, second, result);*/

            /*// 06 Logical Operators
            bool result;
            int first = 10, second = 20;

            result = (first == second) || (first > 5);
            Console.WriteLine("{0} || {1}:{2}", first == second, first > 5, result);

            result = (first == second) && (first > 5);
            Console.WriteLine("{0} && {1}:{2}", first == second, first > 5, result);

            result = true ^ false;
            Console.WriteLine("{0} ^ {1}:{2}", true, false, result);

            result = !(first > second);
            Console.WriteLine("!{0}:{1}", first > second, result);*/

            /*// 07 Bitwise Operators
            int x = 14, y = 11, result;

            result = x | y;
            Console.WriteLine("{0} | {1} = {2}", x, y, result);
            result = x & y;
            Console.WriteLine("{0} & {1} = {2}", x, y, result);
            result = x ^ y;
            Console.WriteLine("{0} ^ {1} = {2}", x, y, result);
            result = ~x;
            Console.WriteLine("~{0} = {1}", x, result);
            result = x<<2;
            Console.WriteLine("{0} << 2 = {1}", x, y, result);
            result = y>>1;
            Console.WriteLine("{0} >> 1 = {1}", y, result);*/

            // 08 Conditional Operators
            int input = Convert.ToInt32(Console.ReadLine());

            string result = (input > 0) ? "양수입니다." : "음수입니다.";
            Console.WriteLine("{0}는 {1}", input, result);
            Console.WriteLine("{0}는 {1}", input,
              (input % 2 == 0) ? "짝수입니다." : "홀수입니다.");

            for(int i = 1; i <=50;i++)
            {
                Console.WriteLine("{0,3}{1}", i, i % 10 != 0 ? "" : "\n");
            }


        }
    }
}