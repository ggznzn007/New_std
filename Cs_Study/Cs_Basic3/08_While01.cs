using System;

class CSTest
{
    static void Main()
    {
        int i = 1;
        int sum = 0;
        while(i<100)
        {
            sum = sum + i;
            i++;
        }
        Console.WriteLine("1 ~ 99의 합 = {0}", sum);
    }
}