using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1000보다 작은 3또는 5의 배수의 합을 구하라
            int sum = 0;
            for (int i = 1; i < 1000; i++)
            {
                if (i % 3 == 0 || i % 5 == 0) //3의 배수또는 5의배수
                {
                    sum += i;
                }
            }
            Console.WriteLine(sum);
        }
    }
}
