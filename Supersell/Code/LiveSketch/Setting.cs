using UnityEngine;
public class Setting
{
    public static float delayTime = 3;
    public static WaitForSeconds delay;
    public static float speed = 7;
    public static float startWaitTime = 1.5f;
    public static float minX = -20f;
    public static float maxX = 20f;
    public static float minY = -20f;
    public static float maxY = 20f;
    public static float minZ = 0f;
    public static float maxZ = 10f;
    public static float scale = 2f;
    public static float distance = 1;
    public static string movingSpots = "MovingSpotGroup";
    public static float posVal = 2.5f;

    public static int[] RandomNumbers(int maxCount, int n)
    {
        int[] defaults = new int[maxCount]; // 0~maxCount���� ������� �����ϴ� �迭
        int[] results = new int[n];         // ��� ������ �����ϴ� �迭

        // �迭 ��ü�� 0���� maxCount�� ���� ������� ����
        for (int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }

        // �츮�� �ʿ��� n���� ���� ����
        for (int i = 0; i < n; ++i)
        {
            int index = Random.Range(1, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }
        return results;
    }
}
