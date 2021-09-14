using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Stack01
    {
        public static void Main()
        {
            string[] str;
            string[] keyword = new string[] { "push", "pop", "size", "empty", "top" };
            int[] stack = new int[10000];
            int index = 0;
            int num = int.Parse(Console.ReadLine());
            for (int i = 0; i < num; i++)
            {
                str = Console.ReadLine().Split();
                for (int x = 0; x < keyword.Length; x++)
                {
                    if (str[0].CompareTo(keyword[x]) == 0)
                    {
                        switch (x)
                        {
                            case 0:
                                stack[index++] = int.Parse(str[1]);
                                break;
                            case 1:
                                if (index == 0)
                                    Console.WriteLine("-1");
                                else
                                    Console.WriteLine(stack[--index]);
                                break;
                            case 2:
                                Console.WriteLine(index);
                                break;
                            case 3:
                                if (index == 0)
                                    Console.WriteLine("1");
                                else
                                    Console.WriteLine("0");
                                break;
                            case 4:
                                if (index == 0)
                                    Console.WriteLine("-1");
                                else
                                    Console.WriteLine(stack[index - 1]);
                                break;
                        }
                    }
                }
            }
        }
    }
}
