using UnityEngine;

public static class Utils
{
	/// <summary>
	/// 0 ~ maxCount까지의 숫자 중 겹치지 않는 n개의 난수가 필요할 때 사용
	/// </summary>
	public static int[] RandomNumbers(int maxCount, int n)
	{
		int[] defaults	= new int[maxCount];	// 0~maxCount까지 순서대로 저장하는 배열
		int[] results	= new int[n];			// 결과 값들을 저장하는 배열

		// 배열 전체에 0부터 maxCount의 값을 순서대로 저장
		for ( int i = 0; i < maxCount; ++ i )
		{
			defaults[i] = i;
		}

		// 우리가 필요한 n개의 난수 생성
		for ( int i = 0; i < n; ++ i )
		{
			int index = Random.Range(0, maxCount);

			results[i]		= defaults[index];
			defaults[index]	= defaults[maxCount-1];

			maxCount --;
		}

		return results;
	}
}

