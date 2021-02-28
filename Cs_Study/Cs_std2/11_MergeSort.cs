using System;
using System.Linq;

namespace Cs_std2
{
    public class Program
    {
        public const int ITEMSIZE = 8;

        public static void Main(string[] args)
        {
            int[] array = new int[] { 14, 7, 3, 12, 9, 11, 6, 2 };
            mergeSort(array, 0, ITEMSIZE - 1);
            printArray(array);
        }

        public static void printArray(int[] array)
        {
            foreach (int element in array)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }

        public static void merge(int[] array, int start, int mid, int end)
        {
            int[] resultArray = new int[ITEMSIZE];
            int low = start;
            int high = mid + 1;
            int key = start;

            // 나눠서 비교해서 정렬
            while (low <= mid && high <= end)
            {
                if (array[low] < array[high])
                {
                    resultArray[key] = array[low];
                    low++;
                }
                else
                {
                    resultArray[key] = array[high];
                    high++;
                }
                key++;
            }
            // 오른쪽(high) array 값이 다 처리 안됫을 경우
            if (low > mid)
            {
                for (high = high; high <= end; high++)
                {
                    resultArray[key] = array[high];
                    key++;
                }
                // 왼쪽(low) array 값이 다 처리 안됫을 경우
            }
            else
            {
                for (low = low; low <= mid; low++)
                {
                    resultArray[key] = array[low];
                    key++;
                }
            }
            // 임시로 만든 array를 정렬할 array에 복사
            for (int i = start; i <= end; i++)
            {
                array[i] = resultArray[i];
            }
            printArray(array);
        }

        public static void mergeSort(int[] array, int start, int end)
        {
            if (start < end)
            {
                int mid = ((start + end) / 2);
                mergeSort(array, start, mid);
                mergeSort(array, mid + 1, end);
                merge(array, start, mid, end);
            }
        }

    }
}
