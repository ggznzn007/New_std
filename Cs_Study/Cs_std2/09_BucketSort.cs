using System;
using System.Collections.Generic;
using System.Linq;




namespace Bucket_Sort01
{
	class Pro
	{
		static void Main(string[] args)
        {
			int[] array = { 0, 5, 1, 6, 8, 2, 3 };
			foreach (int num in array)
				Console.Write(" " + num);
			Console.WriteLine();

			BucketSort(array);
			foreach (int num in array)
				Console.Write(" " + num);
        }
		public static void BucketSort(int[] data)
		{
			int minValue = data[0];
			int maxValue = data[0];

			for (int i = 1; i < data.Length; i++)
			{
				if (data[i] > maxValue)
					maxValue = data[i];
				if (data[i] < minValue)
					minValue = data[i];
			}

			List<int>[] bucket = new List<int>[maxValue - minValue + 1];

			for (int i = 0; i < bucket.Length; i++)
			{
				bucket[i] = new List<int>();
			}

			for (int i = 0; i < data.Length; i++)
			{
				bucket[data[i] - minValue].Add(data[i]);
			}

			int k = 0;
			for (int i = 0; i < bucket.Length; i++)
			{
				if (bucket[i].Count > 0)
				{
					for (int j = 0; j < bucket[i].Count; j++)
					{
						data[k] = bucket[i][j];
						k++;
					}
				}
			}
		}
	}
}