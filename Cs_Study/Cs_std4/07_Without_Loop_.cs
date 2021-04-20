using System;

class WithoutLoops
{
    static int cnt = 1;

    static void printNums(int n)
    {
        Console.WriteLine(cnt);
        cnt++;

        if(cnt==n+1)
        { return; }
        printNums(n);
    }
    
    public static void Main()
    {
        int num;

        Console.WriteLine("Enter any Number : ");

        num = Convert.ToInt32(Console.ReadLine());

        printNums(num);

        Console.ReadKey(true);
    }

}