using System;
using System.Collections.Generic;

namespace Stack_T
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("계산할 수식을 Polish 표기법으로 입력하세요: ");
            string[] token = Console.ReadLine().Split();

            foreach (var i in token)
                Console.Write(" {0}", i);
            Console.Write(" = ");

            Stack<double> nStack = new Stack<double>();
            
        }
    }
}