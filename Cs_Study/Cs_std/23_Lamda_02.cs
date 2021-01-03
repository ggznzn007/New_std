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


        }
    }
}