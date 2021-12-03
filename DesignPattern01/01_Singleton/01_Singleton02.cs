using System;

namespace Singleton02
{
    public class Singleton2
    {
        private static Singleton2 staticSingleton;
        
        public static Singleton2 Intance()
        {
            if(staticSingleton==null)
            {
                staticSingleton = new Singleton2();
            }
            return staticSingleton;
        }

        static void Main(string[] args)
        {
            var ObjectA = Singleton2.Intance();
            var ObjectB = Singleton2.Intance();
            var ObjectC = Singleton2.Intance();

            Console.WriteLine(ObjectA);
            Console.WriteLine(ObjectB);
            Console.WriteLine(ObjectC);
        }
    }
}