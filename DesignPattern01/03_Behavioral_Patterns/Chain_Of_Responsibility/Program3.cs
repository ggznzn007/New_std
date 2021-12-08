using System;
using System.Collections.Generic;
namespace ChainofResponsibility
{
    class Program
    {
        static void Main(string[] args)
        {
            string picture = string.Empty;
            List<int> mode = new List<int>();
            ChangeHandler[] handlers = new ChangeHandler[3];
            handlers[0] = new GrayChangeHandler(0);
            handlers[1] = new SoftChangeHanler(1);
            handlers[2] = new RedEyeChangeHandler(2);
            UIPart uipart = new UIPart();
            uipart.AddChangeHandler(handlers[0]);
            uipart.AddChangeHandler(handlers[1]);
            uipart.AddChangeHandler(handlers[2]);
            picture = uipart.ChangeRequest(mode, "칼라 빨간눈 날카로운 몸매");
            Console.WriteLine(picture);
            mode.Add(0);
            picture = uipart.ChangeRequest(mode, "칼라 빨간눈 날카로운 몸매");
            Console.WriteLine(picture);
            mode.Add(2);
            picture = uipart.ChangeRequest(mode, "칼라 빨간눈 날카로운 몸매");
            Console.WriteLine(picture);
            mode.Add(1);
            picture = uipart.ChangeRequest(mode, "칼라 빨간눈 날카로운 몸매");
            Console.WriteLine(picture);
        }
    }
}