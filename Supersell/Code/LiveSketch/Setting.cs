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
        int[] defaults = new int[maxCount]; // 0~maxCount까지 순서대로 저장하는 배열
        int[] results = new int[n];         // 결과 값들을 저장하는 배열

        // 배열 전체에 0부터 maxCount의 값을 순서대로 저장
        for (int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }

        // 우리가 필요한 n개의 난수 생성
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
