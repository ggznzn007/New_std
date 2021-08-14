using System;

class Arr01
{
    public static void Main()
    {
        int[] arr01 = new int[] { 10, 20, 30, 40, 50 };
        Console.WriteLine("Contents of the arr - ");

        for(int i = 0; i<5;i++)
        {
            Console.WriteLine("Value at arr[" + i + "]is" + arr01[i]);
        }
        Console.ReadKey(true);
    }
}