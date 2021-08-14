using System;
using System.Collections.Generic;

namespace ListAndLamda
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> myList = new List<String>//6개의 동물이름 List로 정의
            { "mouse","cow","tiger","rabbit","dragon","snake"};

            bool n = myList.Exists(s => s.Contains("x"));//동물이름 중 x자를 포함여부 체크
            Console.WriteLine("이름에 'x'를 포함하는 동물이 있나요: " + n);

            Console.WriteLine("이름이 3글자인 첫 번째 동물: ");
            string name = myList.Find(s => s.Length == 3);//이름이 3글자인 첫번째 동물 탐색
            Console.WriteLine(name);//s=>s.Length==3이 람다식 Predicate입니다

            Console.Write("이름이 6글자 이상의 동물들: ");
            List<string> longName = myList.FindAll(s => s.Length > 5);
            foreach(var item in longName)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            Console.Write("대문자로 변환: ");
            List<string> capList = myList.ConvertAll(s => s.ToUpper());
            foreach (var item in capList)
                Console.Write(item + " ");
            Console.WriteLine();
        }
    }
}