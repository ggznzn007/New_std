using System;
using System.ComponentModel;

namespace UsingDele
{
    class DelSort
    {
        // 델리게이트 CompareDelegate 선언
        public delegate int CompareDelegate(int i1, int i2);

        public static void Sort(int[] arr, CompareDelegate comp)
        {
            if (arr.Length < 2) return;
            Console.WriteLine("Method Prototype: " + comp.Method);

            int ret;
            for(int i =0; i<arr.Length-1;i++)
            {
                for(int j= i+1;j<arr.Length;j++)
                {
                    ret = comp(arr[i], arr[j]);
                    if(ret ==-1)
                    {
                        // 교환
                        int tmp = arr[i];
                        arr[j] = arr[i];
                        arr[i] = tmp;
                    }
                }
            }
            Display(arr);
        }
        static void Display(int[] arr)
        {
            foreach (var i in arr) Console.Write(i + " ");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            (new Program()).Run();
        }

        void Run()
        {
            int[] a = { 5, 53, 3, 7, 1 };

            // 오름차순 정렬
            DelSort.CompareDelegate compareDelegate = AscendingCompare;
            DelSort.Sort(a, compareDelegate);

            // 내림차순 정렬
            compareDelegate = DescendingCompare;
            DelSort.Sort(a, compareDelegate);
        }

        // CompareDelegate 델리게이트와 동일한 Prototype
        int AscendingCompare(int i1, int i2)
        {
            if (i1 == i2) return 0;
            return (i2 - i1) > 0 ? 1 : -1;
        }

        // CompareDelegate 델리게이트와 동일한 Prototype
        int DescendingCompare(int i1, int i2)
        {
            if (i1 == i2) return 0;
            return (i1 - i2) > 0 ? 1 : -1;
        }
    }
}