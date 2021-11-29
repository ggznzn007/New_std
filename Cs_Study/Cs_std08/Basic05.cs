using System;

namespace pointManager
{
    class Program
    {
        public static void Main(string[] args)
        {
            while(true)
            {
              Console.WriteLine("\t***** 점수에 따른 학점알아보기 *****\n");
              int point = 0;
              Console.Write("점수를 입력하시오  :  ");
              point = int.Parse(Console.ReadLine());
              Console.WriteLine();
                if ((point >= 96) && (point <= 100)) { Console.WriteLine("와~우~ A+ 입니다.!!!"); }
              else if ((point >= 91) && (point <= 95)) { Console.WriteLine("축하합니다 A 입니다."); }
              else if ((point >= 86) && (point <= 90)) { Console.WriteLine("아쉽게 B+ 입니다."); }
              else if ((point >= 81) && (point <= 85)) { Console.WriteLine("아쉽게 B 입니다."); }
              else if ((point >= 76) && (point <= 80)) { Console.WriteLine("분발하세요 C+ 입니다."); }
              else if ((point >= 71) && (point <= 75)) { Console.WriteLine("분발하세요 C 입니다."); }
              else if ((point >= 66) && (point <= 70)) { Console.WriteLine("차라리 F받아라 D+ 입니다."); }
              else if ((point >= 61) && (point <= 65)) { Console.WriteLine("차라리 F받아라 D 입니다."); }
              else if ((point >= 0) && (point <= 60)) { Console.WriteLine("닥공!!!!! F 입니다."); }
              else { Console.WriteLine("범위 내에 존재하지 않습니다."); }
              Console.WriteLine();

             
            }
        }
    }
}