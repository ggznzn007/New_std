using System;

namespace SelectionSort01
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] nArr = new int[] { 1, 4, 3, 5, 9, 6, 2, 7, 8, 10 };
            int TargetIndex, temp;

            //배열의 마지막 값은 자연스럽게 정렬이 되므로 배열크기 -1 만큼 반복
            for (int i = 0; i < nArr.Length - 1; i++)
            {
                //정렬방식에 따라 교환이 이루어질 인덱스
                TargetIndex = i;

                //비교대상 인덱스는 시작인덱스 +1 에서 배열의 마지막까지 반복
                for (int j = i + 1; j < nArr.Length; j++)
                {
                    //정렬방식 < : 오름차순, > : 내림차순
                    if (nArr[j] < nArr[TargetIndex])
                        TargetIndex = j;
                }

                if (i != TargetIndex)
                {
                    //검색을 시작한 가장 앞쪽 인덱스와 교환
                    temp = nArr[i];
                    nArr[i] = nArr[TargetIndex];
                    nArr[TargetIndex] = temp;
                }
            }

            for (int i = 0; i < nArr.Length; i++)
                Console.Write(nArr[i] + "\n");
        }
    }
}