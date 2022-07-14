using UnityEngine;

public static class Utils
{
	/// <summary>
	/// 0 ~ maxCount������ ���� �� ��ġ�� �ʴ� n���� ������ �ʿ��� �� ���
	/// </summary>
	public static int[] RandomNumbers(int maxCount, int n)
	{
		int[] defaults	= new int[maxCount];	// 0~maxCount���� ������� �����ϴ� �迭
		int[] results	= new int[n];			// ��� ������ �����ϴ� �迭

		// �迭 ��ü�� 0���� maxCount�� ���� ������� ����
		for ( int i = 0; i < maxCount; ++ i )
		{
			defaults[i] = i;
		}

		// �츮�� �ʿ��� n���� ���� ����
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

