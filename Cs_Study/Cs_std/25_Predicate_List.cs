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
        }
    }
}