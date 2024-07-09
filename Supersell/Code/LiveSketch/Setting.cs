using Unity.VisualScripting;
using UnityEngine;
public class Setting
{
    public static float delayTime = 3;
    public static float DelTime = 10;
    public static WaitForSeconds delay;
    public static float speed = 1.5f;
    public static float speed_Baro = 1.2f;
    public static float speed_Brac = 1.2f;
    public static float startWaitTime = 1.2f;
    public static float minX = -118f;
    public static float maxX = 118f;
    public static float minY = -17f;
    public static float maxY = 0f;
    public static float minZ = 0f;
    public static float maxZ = 100f;
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
