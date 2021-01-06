using System;

namespace CS_01
{
    class Program
    {
        static void Main(string[] args)
        {
            /*int i; // 01
            double x;

            i = 5;
            x = 3.141592;
            Console.WriteLine(" i == " + i + ", x == " + x);

            x = i;//암시적 형변환
            i = (int)x;
            Console.WriteLine(" i == " + i + ", x == " + x);*/

            /* bool b = true;// 02
             char c = 'A';
             decimal d = 1.234m;//m은 decimal 형의 접미사
             double e = 1.23456789;
             float f = 1.23456789f;//f는 float 형의 접미사
             int i = 1234;
             string s = "Hello";

             Console.WriteLine(b);
             Console.WriteLine(c);
             Console.WriteLine(d);
             Console.WriteLine(e);
             Console.WriteLine(f);
             Console.WriteLine(i);
             Console.WriteLine(s);*/

            /*// 03
            Console.WriteLine("10 이하의 소수: {0}, {1}, {2}, {3}", 2, 3, 5, 7);

            string primes;
            primes = String.Format("10 이하의 소수: {0}, {1}, {2}, {3}", 2, 3, 5, 7);
            Console.WriteLine(primes);*/

            /*int v1 = 100; // 04
            double v2 = 1.234;

            Console.WriteLine(v1.ToString() + ", " + v2.ToString());
            Console.WriteLine("v1 = " + v1 + ", v2 = " + v2);
            Console.WriteLine("v1={0}, v2 = {1}", v1, v2);
            Console.WriteLine($"v1 = {v1}, v2 = {v2}");*/

            /*//ConsoleFormat
            Console.Clear(); // 05

            Console.WriteLine("Standard Numeric Format Specifiers");
            Console.WriteLine(
                "(C) Currency:........{0:C}\n" +
                "(D) Decimal:.........{0:D}\n" +
                "(E) Scientific:......{0:E}\n" +
                "(F) Fixed point:.....{0:F}\n" +
                "(G) General:.........{0:G}\n" +
                "(N) Number:..........{0:N}\n" +
                "(P) Percent:.........{1:P}\n" +
                "(R) Round-trip:......{1:R}\n" +
                "(X) Hexademical:.....{0:X}\n",
                -12345678, -1234.5678f);*/

            // 06
            Console.WriteLine("{0:N2}", 1234.5678); // 출력: 1,234.57
            Console.WriteLine("{0:D8}", 1234);      // 출력: 00001234
            Console.WriteLine("{0:F3}", 1234.56);   // 출력: 1234.560
            Console.WriteLine("{0,8}", 1234);       // 출력: ____1234
            Console.WriteLine("{0:-8}", 1234);      // 출력: 1234____

            string s;
            s = string.Format("{0:N2}", 1234.5678);
            Console.WriteLine(s);
            s = string.Format("{0:D8}", 1234);
            Console.WriteLine(s);
            s = string.Format("{0:F3}", 1234.56);
            Console.WriteLine(s);

            Console.WriteLine(1234.5678.ToString("N2"));
            Console.WriteLine(1234.ToString("D8"));
            Console.WriteLine(1234.56.ToString("F3"));

            Console.WriteLine("{0:#.##}", 1234.5678);
            Console.WriteLine("{0:0,0.00}", 1234.5678);
            Console.WriteLine("{0:#,#.##}", 1234.5678);
            Console.WriteLine("{0:000000.00}", 1234.5678);

            Console.WriteLine("{0:#,#.##;(#,#.##);zero}", 1234.567);
            Console.WriteLine("{0:#,#.##;(#,#.##);zero}", -1234.567);
            Console.WriteLine("{0:#,#.##;(#,#.##);zero}", 0);
        }
    }
}