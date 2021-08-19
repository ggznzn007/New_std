using System;

class CSTest
{
    static void Main()
    {
        string snum;
        int num;
        do
        {
            Console.Write("Please Input any Number : ");
            snum = Console.ReadLine();
            num = Convert.ToInt32(snum);
            Console.WriteLine("That is {0}.", num);
        } while (num != 0);
    }
}