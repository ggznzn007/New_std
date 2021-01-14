using System;

namespace _RandomClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Console.Write("{0,-16}", "Random Bytes");
            Byte[] b = new byte[5];
            r.NextBytes(b); // 한번 호출로 배열을 랜덤값으로 채움

            foreach (var x in b)
                Console.Write("{0,12}", x);// 12자리로 출력
            Console.WriteLine();


        }
    }
}