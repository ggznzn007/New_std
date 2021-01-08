using System;

namespace _SplitMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            /*// 01 Spilt Method
            Console.WriteLine("더하고자 하는 숫자들을 입력하세요: ");
            string s = Console.ReadLine();
            Console.WriteLine(s);

            int sum = 0;
            string[] v = s.Split();
            foreach(var i in v)
            {
                sum += int.Parse(i);
            }
            Console.WriteLine("결과는 {0}", sum);*/

            // 02 String Concat
            string userName = "bikang";
            string date = DateTime.Today.ToShortDateString();

            string strPlus = "Hello " + userName + ". Today is " + date + ".";
            Console.WriteLine(strPlus);

            string strFormat = String.Format("Hello {0}. Today is {1}.",
                userName, date);
            Console.WriteLine(strFormat);

            string strInterpolation = $"Hello {userName}. Today is {date}.";
            Console.WriteLine(strInterpolation);



        }
    }
}