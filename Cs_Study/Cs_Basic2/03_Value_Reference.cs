using System;

namespace _Val_Ref
{
    class Program
    {// C언어의 포인터와 비슷한 개념 
        // 값(value) 형식, 참조(reference) 형식
        static void Main(string[] args)
        {
            string s = "before passing";
            Console.WriteLine(s);

            Test(s);
            Console.WriteLine(s);

            Test(ref s);
            Console.WriteLine(s);
        }

        public static void Test(string s)
        {
            s = "after passing";
        }

        public static void Test(ref string s)
        {
            s = "after passing";
        }
    }
}