using System;
using System.Collections.Generic;

public class ListD
{
    public static void Main()
    {
        var countries = new List<string>() { "Korea", "America" };

        countries.Add("UK");
        countries.Add("India");

        foreach(var country in countries)
        {
            Console.WriteLine(country);
        }
    }
}