using System;
using System.Globalization;

namespace _DateTimeFormat
{
    class Pro
    {
        static void Main(string[] args)
        {
            DateTime today = DateTime.Now;

            Console.WriteLine(today.ToString("yyyy년 MM월 dd일"));
            Console.WriteLine(string.Format("{0:yyyy년 MM월 dd일}", today));
            Console.WriteLine(today.ToString("MMMM dd, yyyy ddd",
                CultureInfo.CreateSpecificCulture("en-US")));

            // d : 축약된 날짜 형식
            Console.WriteLine("\td : " + today.ToString("d"));
            // D : 긴 날짜 형식
            Console.WriteLine("\tD : " + string.Format("{0:D}", today));
            // t : 축약된 시간
            Console.WriteLine("\tt : " + string.Format("{0:t}", today));
            // T : 긴 시간 형식
            Console.WriteLine("\tT : " + string.Format("{0:T}", today));
            // g : 일반 날짜 및 시간(초생략)
            Console.WriteLine("g : " + string.Format("{0:g}", today));
            // G : 일반 날짜 및 시간
            Console.WriteLine("G : " + string.Format("{0:G}", today));
            // f : Full 날짜 및 시간(초생략)
            Console.WriteLine("f : " + string.Format("{0:f}", today));
            // F : Full 날짜 및 시간
            Console.WriteLine("F : " + string.Format("{0:F}", today));
            // s : ISO 8601 표준(밀리초 생략)
            Console.WriteLine("s : " + string.Format("{0:s}", today));
            // o : ISO 8601 표준
            Console.WriteLine("o : " + string.Format("{0:o}", today));
            // r : UTC로 표시
            Console.WriteLine("r : " + string.Format("{0:r}", today));
            // u : UTC로 출력
            Console.WriteLine("u : " + string.Format("{0:u}", today));
        }
    }
}