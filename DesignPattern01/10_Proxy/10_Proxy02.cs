using System;

namespace CSharp_DesignPattern
{
    abstract class Subject
    {
        public abstract void Request();
    }
    class RealSubject : Subject
    {
        public override void Request()
        {
            Console.WriteLine("Cakked RealSubject.Request");
        }
    }
    class Proxy : Subject
    {
        private RealSubject _realSubject;

        public override void Request()
        {
            if (_realSubject == null)
            {
                _realSubject = new RealSubject();
            }
            _realSubject.Request();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Proxy proxy = new Proxy();
            proxy.Request();

            Console.ReadKey();
        }
    }
}