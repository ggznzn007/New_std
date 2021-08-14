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

            int[] numbers = { 2, 3, 4, 5 };//정수배열의 각 요소를 제곱하여 squaredNumbers에 할당
            var squaredNumbers = numbers.Select(x => x * x);//string.Join메소드로 빈칸을 사용
            Console.WriteLine(string.Join(" ", squaredNumbers));

            Action line = () => Console.WriteLine();
            line();//Action델리게이트로 매개변수없고 빈줄 하나를 출력

            CalcMethod add = (a, b) => a + b;//CalcMethod 델리게이트
            CalcMethod sub = (a, b) => a - b;//CalcMethod 델리게이트
            //두 개의 더블 값을 매개변수로 하여 두수를 연산함
            Console.WriteLine(add(10, 20));
            Console.WriteLine(sub(10.5, 20));

            IsTeenager isTeen//Student 객체를 매개변수로 하여 12보다크고 20보다 작으면 트루 리턴
                = delegate (Student s) { return s.Age > 12 && s.Age < 20; };

            Student s1 = new Student() { Name = "John", Age = 18 };// s1객체 생성
            Console.WriteLine("{0}은 {1}.",
                s1.Name, isTeen(s1) ? "청소년입니다" : "청소년이 아닙니다");//isTeen값 리턴

            IsAdult isAdult = (s) =>// 문 람다로 정의 18세이상이면 트루를 리턴
            {
                int adultAge = 18;
                return s.Age >= adultAge;
            };

            Student s2 = new Student() { Name = "Robin", Age = 20 };// s2객체 생성
            Console.WriteLine("{0}은 {1}.",
                s2.Name, isAdult(s2) ? "성인입니다" : "성인이 아닙니다");//isAdult값 리턴
        }

        public class Student//Student 클래스 정의 이름과 나이를 속성으로 가진다
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}