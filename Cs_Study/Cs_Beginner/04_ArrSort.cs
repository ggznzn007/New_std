using System;

namespace _ArrSort
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] EngName = {"Mouse","Cow","Tiger","Rabbit","Dragon","Snake","Horse",
                "Sheep","Monkey","Chiken","Dog","Pig"};
            string[] KorName = {"똘기","떵이","호치","새촘이","드라고","요롱이","마초",
            "미미","뭉치","키키","강다리","찡찡이"};
            PrintArray("Before Sort: ", EngName);
            PrintArray("Before Sort: ", KorName);

            Array.Reverse(EngName);
            Array.Reverse(KorName);
            PrintArray("After Reverse: ", EngName);
            PrintArray("After Reverse: ", KorName);

            Array.Sort(EngName);
            Array.Sort(KorName);
            PrintArray("After Sort: ", EngName);
            PrintArray("After Sort: ", KorName);

            Array.Reverse(EngName);
            Array.Reverse(KorName);
            PrintArray("After Reverse: ", EngName);
            PrintArray("After Reverse: ", KorName);
        }

        private static void PrintArray(string s, string[] name)
        {
            Console.WriteLine(s);
            foreach (var n in name)
                Console.Write("{0} ", n);
            Console.WriteLine();
        }
    }
}