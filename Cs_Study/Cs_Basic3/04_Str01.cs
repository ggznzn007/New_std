using System;

namespace _10_StudentStruct
{
    struct Student
    {
        public string Name;
        public int StNum;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Student b;
            b.Name = "정푸른";
            b.StNum = 880706;
            Console.WriteLine("{0}, {1}", b.Name, b.StNum);
        }
    }
}