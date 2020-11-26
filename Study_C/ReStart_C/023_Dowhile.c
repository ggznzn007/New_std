//5. do~while문을 이용해서 양의 정수를 입력받고 그 수만큼 "감사합니다"를 출력하세요
//for문을 이용해서 양의 정수를 입력받고 그 수만큼 "감사합니다"를 출력하세요

#include <stdio.h>



int for4()
{
	while (1)
	{
		printf("\n");
		int i, num;
		printf("양의 정수를 입력하세요 >> ");
		scanf_s("%d", &num);
		for (i = 1; i <= num; i++)
		{
			printf("\n사랑합니다.\n");
		}

	}
	return 0;
}

void main()
{
	for4();
}