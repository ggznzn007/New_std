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

    class Continue
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
    }
}