using System;

class EO
{
    public static void Main()
    {
        int x;

        Console.Write("Enter an integer : ");
        x = Convert.ToInt32(Console.ReadLine());
        if(x%2==0)
        {
            Console.WriteLine("Number is even.");
        }
        else
        {
            Console.WriteLine("Number is odd.");
        }
        Console.ReadKey(true);
    }
}