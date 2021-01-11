using System;

namespace _BMI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("키를 입력하세요(cm) :  ");
            double height = double.Parse(Console.ReadLine());
            height /= 100; // m 단위

            Console.Write("체중을 입력하세요(kg) : ");
            double weight = double.Parse(Console.ReadLine());
            double bmi = weight / (height * height);

            string comment = null;
            if (bmi < 20)
                comment = "저체중입니다. 많이 드세요.";
            else if (bmi < 25)
                comment = "정상체중입니다. 계속 유지하세요.";
            else if (bmi < 30)
                comment = "경도비만입니다. 적당한 운동이 필요해요.";
            else if (bmi < 40)
                comment = "비만입니다. 운동을 열심히 하세요!!!";
            else 
                comment = "고도비만입니다. 그만 좀 쳐먹어!!! 운동 좀 해라!!!";

            Console.WriteLine("BMI = {0:F1}, \"{1}", bmi, comment);
        }
    }
}