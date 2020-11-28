#include <stdio.h>

void main()
{

	printf("BMI를 입력해주세요 >> ");
	int bmi;
	scanf_s("%d", &bmi);

	if (bmi <= 10)
		printf("정상체중 입니다. \n");
	else if (bmi <= 20)
		printf("과체중입니다.\n");
	else
		printf("비만입니다.\n");

	return 0;
}