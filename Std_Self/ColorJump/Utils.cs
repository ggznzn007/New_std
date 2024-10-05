using UnityEngine;

public static class Utils
{   
   public static int[] RandomNumberics(int maxCount, int n)
    {
        // 0 ~ maxCount까지의 숫자 중 겹치지 않는 n개의 난수가 필요할 때 사용
        int[] defaults = new int[maxCount];            // 0 ~ maxCount까지 순서대로 저장하는 배열
        int[] result = new int[n];                     // 결과 값들을 저장하는 배열

        // 배열 전체에 0 ~ maxCount까지의 값을 순서대로 저장
        for (int i = 0; i < maxCount; i++)
        {
            defaults[i] = i;
        }

        for (int i = 0;i < n;i++)
        {
            int index = Random.Range(0, maxCount);    // 임의의 숫자를 하나 선택

            result[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }
        return result;
    }
}
