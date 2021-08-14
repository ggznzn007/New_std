using System;

public class NameAndPricePair
{
    public string Name { get; set; }
    public int Price { get; set; }
}

class Program
{
    private static void output(NameAndPricePair data)
    {
        Console.WriteLine("{0}은 {1}원입니다.", data.Name, data.Price);
    }
    static void Main(string[] args)
    {
        var data1 = new NameAndPricePair() { Name = "가방", Price = 1200 };
        var data2 = new NameAndPricePair() { Name = "지갑", Price = 200 };
        output(data1);
        output(data2);
    }
}