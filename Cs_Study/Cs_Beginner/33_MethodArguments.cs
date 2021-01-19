using System;

namespace _MethodAr
{
    class Pro // 메소드에 인수를 전달하는 3가지 방법
    {
        static void Main(string[] args)
        {
            /*1.값에 의한 호출 - 인수의 값을 메소드의 매개변수에 복사한다
              이 경우에는 메소드 안에서 매개변수의 값이 바뀌더라도 호출한 곳의 인수에 영향을 미치지 않는다.
              == C# 에서의 디폴트*/
            int a = 3;
            Sqr(a);
            Console.WriteLine("Value: {0}", a);// 3이 출력됩니다.

            /*2. 참조에 의한 호출 - ref 키워드로 인수를 메소드로 전달하면 실제로는 변수의 참조
              즉, 주소를 전달하기 때문에 호출된 메소드에서 그 주소에 있는 변수의 값을 바꿀수 있음*/
            int b = 3;
            Sqr(ref b);
            Console.WriteLine("ref: {0}", b);// 9가 출력됩니다.

            /*3. out 키워드를 사용 - 메소드에서 out을 사용한 변수는 호출한 곳으로 값을 내보내줄때만
             * 사용되고 out을 사용한 인수의 값은 메소드 내에서 사용되지 않는다.
             * 따라서 변수의 초기화가 필요없다, 메소드가 최대 1개의 리턴값만 보내줄 수 있기 때문에
             * out을 사용하면 메소드에서 여러개의 값을 리턴하는 효과가 있음*/
            string name;
            int id;
            GetName(out name, out id);
            Console.WriteLine("out: {0} {1}", name, id);
        }

        static void Sqr(int x)
        {
            x = x * x;
        }

        static void Sqr(ref int x)
        {
            x = x * x;
        }

        static void GetName(out string name, out int id)
        {
            Console.Write("Enter Name: ");
            name = Console.ReadLine();
            Console.Write("Enter Id: ");
            id = int.Parse(Console.ReadLine());
        }
    }
}