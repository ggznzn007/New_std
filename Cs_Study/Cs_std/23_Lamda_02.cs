using System;
using System.Linq;

namespace Lamda_02
{
    class Program
    {
        delegate double CalcMethod(double a, double b);
        delegate bool IsTeenager(Student student);
        delegate bool IsAdult(Student student);

        static void Main(string[] args)
        {
            Func<int, int> square = x => x * x;
            Console.WriteLine(square(5));

            int[] numbers = { 2, 3, 4, 5 };
            var squaredNumbers = numbers.Select(x => x * x);
            Console.WriteLine(string.Join(" ", squaredNumbers));

            Action line = () => Console.WriteLine();
            line();

            CalcMethod add = (a, b) => a + b;
            CalcMethod sub = (a, b) => a - b;

            Console.WriteLine(add(10, 20));
            Console.WriteLine(sub(10.5, 20));

            IsTeenager isTeen
                = delegate (Student s) { return s.Age > 12 && s.Age < 20; };

            Student s1 = new Student() { Name = "John", Age = 18 };
            Console.WriteLine("{0}은 {1}.",
                s1.Name, isTeen(s1) ? "청소년입니다" : "청소년이 아닙니다");

            IsAdult isAdult = (s) =>
            {
                int adultAge = 18;
                return s.Age >= adultAge;
            };

            Student s2 = new Student() { Name = "Robin", Age = 20 };
            Console.WriteLine("{0}은 {1}.",
                s2.Name, isAdult(s2) ? "성인입니다" : "성인이 아닙니다");
        }

        public class Student
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}