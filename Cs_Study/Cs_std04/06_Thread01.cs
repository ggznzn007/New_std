using System;
using System.Threading;

public class ThreadOne
{
    public void Thread1()
    {
        for(int i = 0; i<5;i++)
        {
            Console.WriteLine("Thread One Runs {0} times", i);
        }
    }
}

public class ThreadTwo
{
    public void Thread2()
    {
        for(int i = 0; i<5; i++)
        {
            Console.WriteLine("Thread Two Runs {0} times", i);
        }
    }
}

public class ThreadDemo
{
    public static void Main()
    {
        Console.WriteLine("Main Thread starts");

        ThreadOne thr1 = new ThreadOne();
        ThreadTwo thr2 = new ThreadTwo();

        Thread tid1 = new Thread(new ThreadStart(thr1.Thread1));
        Thread tid2 = new Thread(new ThreadStart(thr2.Thread2));

        tid1.Start();
        tid2.Start();

        Console.WriteLine("Main Thread ends");
        Console.ReadKey(true);
    }
}