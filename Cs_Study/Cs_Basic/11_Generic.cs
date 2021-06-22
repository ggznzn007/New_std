using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _11_Generic
{
    class Wrapper<T>
    {
        T Value;
        public Wrapper()
        { Value = default(T); }
        public Wrapper(T aValue)
        { Value = aValue; }
        public T Data
        {
            get { return Value; }
            set { Value = value; }
        }
        public void OutValue()
        {
            Console.WriteLine(Value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Wrapper<int> gi = 
                new Wrapper<int>(1234);
            gi.OutValue();
            Wrapper<string> gs =
                new Wrapper<string>("문자열");
            gs.OutValue();
        }
    }
}
