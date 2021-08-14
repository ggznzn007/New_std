using System;

namespace HeapSort01
{
    class Program
    {
        static void heapSort(int[] arr)
        {
            int i, temp, length = arr.Length;
            for (i = length / 2 - 1; i >= 0; i--) heapHeapify(arr, length, i);
            for (i = length - 1; i >= 0; i--)
            {
                temp = arr[0]; arr[0] = arr[i]; arr[i] = temp;
                heapHeapify(arr, i, 0);
            }
        }
        static void heapHeapify(int[] arr, int length, int i)
        {
            int left = 2 * i + 1, right = 2 * i + 2;
            int temp, largest = i;
            if (left < length && arr[left] > arr[largest]) largest = left;
            if (right < length && arr[right] > arr[largest]) largest = right;
            if (largest != i)
            {
                temp = arr[i]; arr[i] = arr[largest]; arr[largest] = temp;
                heapHeapify(arr, length, largest);
            }
        }
        static void Main(string[] args)
        {
            int[] arr = { 9, 1, 22, 4, 0, -1, 1, 22, 100, 10 };
            foreach ( int num in arr )
            Console.Write(" " + num);
            Console.WriteLine();

            heapSort(arr);
            foreach (int num in arr)
            Console.Write("," +num);
            // -1,0,1,1,4,9,10,22,22,100
        }
    }

}