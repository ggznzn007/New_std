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
            //수식을 입력 받아 Split 메소드로 빈칸을 나누어 token배열에 저장하고 출력
            Stack<double> nStack = new Stack<double>();//제네릭 Stack 객체 생성
            foreach (var s in token)
            {//token 배열의 내용을 하나씩 읽어와서 연산자 여부 체크,
             //연산자이면 if문 실행, 아니면, else문에 의해 더블로 바꾸어 푸쉬
                if (isOperator(s))
                {//연산자일 경우 switch문에서 연산을 수행
                    switch (s)
                    {
                        case "+":
                            nStack.Push(nStack.Pop() + nStack.Pop()); break;
                        case "-":
                            nStack.Push(-(nStack.Pop() - nStack.Pop())); break;
                        case "*":
                            nStack.Push(nStack.Pop() * nStack.Pop()); break;
                        case "/":
                            nStack.Push(1.0 / (nStack.Pop() / nStack.Pop())); break;
                    }
                }
                else
                {
                    nStack.Push(double.Parse(s));
                }
            }
            Console.WriteLine(nStack.Pop());
            //모든 토큰이 처리된 후 nStack의 맨 아래에 수식의 결과값 저장 후 Pop으로 출력
        }

        private static bool isOperator(string s)
        {//매개변수가 연산자인지 체크 후 연산자이면 true, 아니면 false 리턴하는 메소드 
            if (s == "+" || s == "-" || s == "*" || s == "/")
                return true;
            else
                return false;
        }
    }
}