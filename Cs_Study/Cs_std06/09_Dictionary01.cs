using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Dictionary<자료형,자료형>  변수명 = new Dictionary<자료형,자료형>();
// <자료형,자료형> -> <Key, Value>값을 나타낸다
// 키와 값이 한쌍으로 이루어져 있는 자료구조

namespace Dictionary01
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();

            // 객체에 사용자가 원하는 데이터 저장
            dic.Add("내가하면10분컷", 56);
            dic.Add("척척박사", 55);
            dic.Add("20년개발자", 57);

            Console.WriteLine("=====================" +
                "foreach문=========================");
            // Dictionary에 저장되어 있는 객체 출력
            // 1. foreach 반복문을 통하여 출력하는 방법
            foreach (KeyValuePair<string, int> pair in dic)
            {
                Console.WriteLine("Key : {0} , Value : {1}", pair.Key, pair.Value);
            }

            foreach (var pair in dic)
            {
                Console.WriteLine("Key : {0} , Value : {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("========================" +
                "for문==================================");

            // 2.for문을 통하여 출력하는 방법
            // var형식의 변수 target에 dit.ToList 형태 저장
            var target = dic.ToList();

            for (int idx = 0; idx <= dic.Count; idx++)
            {
                Console.WriteLine("Key : {0} , Value : {1}", target[idx].Key, target[idx].Value);
            }
        }
    }
}