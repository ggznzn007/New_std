using System;

namespace Float_Double_Decimal
{
    class Program
    {
        static void Main(string[] args)
        {
            /*float flt = 1F / 3; // 01
            double dbl = 1D / 3;
            decimal dcm = 1M / 3;

            Console.WriteLine("float : {0}\ndouble : {1}\ndecimal : {2}", flt, dbl, dcm);
            Console.WriteLine("float : {0} bytes\ndouble : {1} bytes\ndecimal : {2} bytes",
                sizeof(float), sizeof(double), sizeof(decimal));
            Console.WriteLine("float : {0}~{1}", float.MinValue, float.MaxValue);
            Console.WriteLine("double : {0}~{1}", double.MinValue, double.MaxValue);
            Console.WriteLine("decimal : {0}~{1}", decimal.MinValue, decimal.MaxValue);*/

            /*// 02 TypeConversion
            int num = 2147483647;
            long bigNum = num;// 암시적 형변환
            Console.WriteLine(bigNum);

            double x = 1234.5;
            int a;

            a = (int)x; // 명시적 형변환
            Console.WriteLine(a);*/

            /*// 03 String to Number
            string input;
            int value;

            Console.WriteLine("1. int로 변환할 문자열을 입력하세요: ");
            input = Console.ReadLine();
            bool result = Int32.TryParse(input, out value);

            if (!result)
                Console.WriteLine("'{0}'는 int로 변환될 수 없습니다.\n", input);
            else
                Console.WriteLine("int '{0}' 으로 변환되었습니다.\n", value);

            Console.WriteLine("2. double로 변환할 문자열을 입력하세요: ");
            input = Console.ReadLine();
            try
            {
                double m = Double.Parse(input);
                //double m = Convert.ToDouble(input);
                Console.WriteLine("double '{0}'으로 변환되었습니다.\n", m);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }*/

            // 04 Convert
            int x, y;

            Console.WriteLine("첫번째 숫자를 입력하세요: ");
            x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("두번째 숫자를 입력하세요: ");
            y = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("{0} + {1} = {2}", x, y, x + y);

            // 2진수, 8진수, 10진수, 16진수로 출력하기
            short value = short.MaxValue; // Int16.MaxValue
            Console.WriteLine("\n2진수, 8진수, 10진수, 16진수로 출력하기");

            int baseNum = 2;
            string s = Convert.ToString(value, baseNum);
            int i = Convert.ToInt32(s, baseNum);
            Console.WriteLine("i = {0}, {1,2}진수= {2,16}", i, baseNum, s);


        }
    }
}