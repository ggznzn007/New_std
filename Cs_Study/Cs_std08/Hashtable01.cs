using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new ConcurrentDictionary<int, string>();

            Task t1 = Task.Factory.StartNew(() =>
            {
                int key = 1;
                while (key <= 100)
                {
                    if (dict.TryAdd(key, "Albert" + key))
                    {
                        key++;
                    }
                    Thread.Sleep(100);
                }
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                int key = 1;
                string val;
                while (key <= 100)
                {
                    if (dict.TryGetValue(key, out val))
                    {
                        Console.WriteLine("{0}, {1}", key, val);
                        key++;
                    }
                    Thread.Sleep(100);
                }
            });

            Task.WaitAll(t1, t2);
        }
    }
}
