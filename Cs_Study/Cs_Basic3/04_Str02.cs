using System;

namespace _11_StructorCtor
{
    class Program
    {
        static void Main(string[] args)
        {
            Student b = new Student("정푸른", 880706);
            b.OutInfo();
        }
    }

    struct Student
    {
        public string Name;
        public int StNum;
        public Student(string aName, int aStNum)
        {
            Name = aName;
            StNum = aStNum;
        }
        public void OutInfo()
        {
            Console.WriteLine("{0}, {1}", Name, StNum);
        }
    }
}