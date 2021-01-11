using System;

namespace _Grading
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
            Console.Write("해당학생의 점수를 입력하세요:  ");
            int score = int.Parse(Console.ReadLine());
            string grade = null;

            if (score >= 90)
                grade = "A+";
            else if (score >= 80)
                grade = "B+";
            else if (score >= 70)
                grade = "C";
            else if (score >= 60)
                grade = "D";
            else 
                grade = "F";

            Console.WriteLine("학점은 {0}", grade);

            switch (score/10)
            {
                case 10:
                case 9:
                    grade = "A+";
                    break;
                case 8:
                    grade = "B+";
                    break;
                case 7:
                    grade = "C";
                    break;
                case 6:
                    grade = "D";
                    break;
                default:
                    grade = "F";
                    break;
            }
            Console.WriteLine("학점은 {0}", grade);
            }
        }
    }
}