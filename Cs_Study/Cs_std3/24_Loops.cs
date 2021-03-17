using System;

namespace loops
{
    /*class For
    {
        static void Main()
        {
            for (int i = 1; i <= 10; i++)
                Console.Write(" " + i);
        }
    }*/

    /*class While
    {
        static void Main()
        {
            int i = 1;
            while(i<=10)
            {
                Console.Write(" " + i);
                i++;
            }
        }
    }*/

    /*class Dowhile
    {
        static void Main()
        {
            int i = 1;
            do
            {
                Console.Write(" " + i);
                i++;
            } while (i <= 10);
        }
    }*/

    /*class Foreach
    {
        static void Main()
        {
            int[] arr = new int[10];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = i + 1;

            foreach(int variable in arr)
            {
                Console.Write(" " + variable);
            }
        }
    }*/

    /*class BreakStatement
    {
        static void Main()
        {
            for(int i = 1; i<=10;i++)
            {
                if (i == 6)
                    break;
                Console.Write(" " + i);
            }
            Console.WriteLine("\nLoop is ended.");
        }
    }*/

    /*class Continue
    {
        static void Main()
        {
            for(int i = 1; i<=10;i++)
            {
                if(i==5)
                {
                    Console.Write(" _");
                    continue;
                }
                Console.Write(" " + i);
            }
        }
    }*/

    /*class Infinite
    {
        static void Main()
        {
            for(; ; )
            {
                Console.WriteLine("This loop is infinite.");
            }

           *//* while(true)
            {
                Console.WriteLine("This loop is infinite.");
            }*//*
        }
    }*/

    /*class Embedded_Cycle
    {
        static void Main()
        {
            for(int i = 0; i<10;i++)
            {
                for(int j = 0; j<10;j++)
                {
                    Console.Write(" " + i + "" + j);
                }
                Console.WriteLine("");
            }
        }
    }*/

    /*class MultiTable
    {
        static void Main()
        {
            for(int i =1; i<=10;i++)
            {
                Console.Write(i + ": ");
                for(int j = 1; j<=10;j++)
                {
                    Console.Write((i * j) + " ");
                }
                Console.WriteLine();
            }
        }
    }*/

    class ForWithoutBody
    {
        static void Main()
        {
            int sum = 0;
            for (int i = 0; i < 5; sum += i++) ;

            Console.WriteLine(sum);
        }
    }
}