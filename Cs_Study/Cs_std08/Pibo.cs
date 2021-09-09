using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Pibo
    {
        static void Main()
        {
            // 피보나치 수열 중 4백만을 초과하지 않는 짝수 값의 합을 구하라
            int[] idx = new int[50];
            for (int i = 0; i < idx.Length; i++)
            {
                idx[i] = i;
            }

            int sum = 0;
            for (int a = 0; a < idx.Length - 2; a++)
            {
                idx[a + 2] = idx[a] + idx[a + 1];
                if (idx[a + 2] % 2 == 0)
                {
                    Console.WriteLine(idx[a + 2]);
                    sum += idx[a + 2];
                }
                if (idx[a + 2] > 4000000)
                    break;
            }
            Console.WriteLine(sum);
            Console.ReadLine();

        }

    }
}
