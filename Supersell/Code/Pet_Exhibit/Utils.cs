using UnityEngine;

public class Utils:MonoBehaviour
{   
    public static float move_Speed = 7f;
    public static float scroll_Speed = 3000.0f;

    public static float minPosX = -22f; 
    public static float maxPosX = 22f; 
    public static float minPosZ = -4f; 
    public static float maxPosZ = 4f; 

    public static float maxZoom = 87.5f; 
    public static float minZoom = 30f;

    public static float wait = 1f;
    public static float minOrth = 8f;                           //2d 카메라 줌인아웃
    public static float maxOrth = 24f;
    public static float minOrth_x = -7f;
    public static float maxOrth_x = 7f;
    public static float minOrth_z = -4f;
    public static float maxOrth_z = 4f;

    /*public float Move_Speed() { return move_Speed; }
    public float Scroll_Speed() { return scroll_Speed; }
    public float Min_PosX() { return minPosX; }
    public float Max_PosX() { return maxPosX; }
    public float Min_PosZ() { return minPosZ; }
    public float Max_PosZ() { return maxPosZ; }
    public float Max_Zoom() { return maxZoom; }
    public float Min_Zoom() { return minZoom; }*/

    /// <summary>
    /// 0 ~ maxCount까지의 숫자 중 겹치지 않는 n개의 난수가 필요할 때 사용
    /// </summary>
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
            int index = Random.Range(0, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
    }
}
