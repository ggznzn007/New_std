using System;
class CSTest
{
    static void Main()
    {
        string s = "Albert";
        switch(s)
        {
            case "one":
                Console.WriteLine(1);
                break;
            case "two":
                Console.WriteLine(2);
                break;
            case "three":
                Console.WriteLine(3);
                break;
            default:
                Console.WriteLine(7);
                break;
        }
    }
}