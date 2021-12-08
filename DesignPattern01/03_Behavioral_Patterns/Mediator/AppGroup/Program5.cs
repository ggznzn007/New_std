using System;
namespace Mediator
{
    class Program
    {
        static void Main(string[] args)
        {
            Camera camera = new Camera();
            camera.ChangeMode(0);
            Console.WriteLine("1");
            camera.SetShutterSpeed(3);
            Console.WriteLine("2");
            camera.SetIris(3);
            camera.ChangeMode(1);
            Console.WriteLine("3");
            camera.SetShutterSpeed(4);
            Console.WriteLine("4");
            camera.SetIris(4);
        }
    }
}