using System;
using System.Collections.Generic;

namespace Dictionary_T
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> colorTable = new Dictionary<string, string>();
            //제네릴 Dictionary 객체 생성
            colorTable.Add("Red", "빨간색");
            colorTable.Add("Green", "초록색");
            colorTable.Add("Blue", "파란색");
            //3개의 키, 값 쌍을 저장
            foreach (var v in colorTable)//foreach문으로 모든 자료 출력
                Console.WriteLine("colorTable[{0}] = {1}", v.Key, v.Value);
            //try catch문 안에서 레드 빨강으로 새로운값 저장 시
            // 이미 레드 키값이 존재하므로 ArgumentException이 발생
            try
            {
                colorTable.Add("Red", "빨강");
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            //try catch문 안에서 옐로우를 출력하려고 하면, 옐로우라는 키값이 없어서
            // KeyNotFoundException이 발생
            try
            {
                Console.WriteLine("Yellow => {0}", colorTable["Yellow"]);
            }
            catch(KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\n" + colorTable["Red"]);
            Console.WriteLine(colorTable["Green"]);
            Console.WriteLine(colorTable["Blue"]);
            //빨간,초록,파란 순서대로 값이 출력됩니다
        }
    }
}