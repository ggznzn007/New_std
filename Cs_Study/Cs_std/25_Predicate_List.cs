using System;
using System.Collections.Generic;

namespace ListAndLamda
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> myList = new List<String>
            { "mouse","cow","tiger","rabbit","dragon","snake"};

            bool n = myList.Exists(s => s.Contains("x"));
            Console.WriteLine("이름에 'x'를 포함하는 동물이 있나요: " + n);

            Console.WriteLine("이름이 3글자인 첫 번째 동물: ");
            string name = myList.Find(s => s.Length == 3);
            Console.WriteLine(name);

            Console.Write("이름이 6글자 이상의 동물들: ");
            List<string> longName = myList.FindAll(s => s.Length > 5);
            foreach(var item in longName)
            {

            }
        }
    }
}