using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashTable02
{
    class Program
    {
        static void Main(string[] args)
        {
            // 객체 선언
            Hashtable ht = new Hashtable();

            // 선언한 객체에 데이터 저장하기
            ht.Add("하나", "one");
            ht.Add("둘", "two");
            ht.Add("셋", "three");
            ht.Add("넷", "four");
            ht.Add("다섯", "five");
            ht.Add("여섯", "six");

            foreach (var value in ht.Keys)
            {
                Console.WriteLine("{0} : {1}", value, ht[value]);
            }
        }
    }
}