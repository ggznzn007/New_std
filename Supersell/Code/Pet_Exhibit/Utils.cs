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
    public static float minOrth = 8f;                           //2d ī�޶� ���ξƿ�
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
    /// 0 ~ maxCount������ ���� �� ��ġ�� �ʴ� n���� ������ �ʿ��� �� ���
    /// </summary>
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
            int index = Random.Range(0, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
    }
}
