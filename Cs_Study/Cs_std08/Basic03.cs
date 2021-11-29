using System; // 문자열 거꾸로 출력

namespace ReverseString
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("<<<<<<<< 로꾸꺼 월드 >>>>>>>>>>\n");
                Console.WriteLine("문자를 입력하시오 : \n");
                string a = Console.ReadLine();
                char[] stringArr = a.ToCharArray();
                Array.Reverse(stringArr);
                Console.WriteLine(new string(stringArr));
                Console.WriteLine();

                if(a==""||a=="Q"||a=="q")
                {
                    Environment.Exit(0);                    
                }
            }
        }
    }
}