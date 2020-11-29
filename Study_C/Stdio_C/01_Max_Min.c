//1. 10개 정수 입력 후 최대 최소 총합 평균을 구하세요.

#include <stdio.h>

void main()
{
	int i;
	int a[10];
	int sum = 0;
	int max;
	int min;
	printf("\n10개의 정수를 입력하세요>> \n\n");
	for (i = 0; i <= 9; i++)
	{
		scanf_s("%d", &a[i]);
		sum += a[i];

	}
	for (max = a[0], min = a[0], i = 1; i <= 9; i++)
	{
		if (max < a[i])
			max = a[i];

		if (min > a[i])
			min = a[i];
	}

	printf("\n최대값 = %d\n\n최소값 = %d\n\n총합 = %d\n\n평균 = %.1f\n\n",
		max, min, sum, sum / 10.f);
}