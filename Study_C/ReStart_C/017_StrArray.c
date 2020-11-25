/* 3명의 학생의 이름과 국어점수를 입력받고
총점과 평균
각 학생 이름과 점수를 출력하세요
*/
#include "turboc.h"

#define ST_NUM 100

void main()
{
	int kor[ST_NUM] = { 0 };
	int sum = 0;
	double avg = 0.0;
	// char[20]배열이 3개 있다1
	char names[3][20] = { 0 };
	printf("\n");
	for (int i = 0; i < 3; i++)
	{
		printf("이름 입력 >> ");
		fgets(names[i], sizeof(names[i]) - 1, stdin);
		names[i][strlen(names[i]) - 1] = 0;
	}
	printf("\n");
	for (int i = 0; i < 3; i++)
	{
		printf("%d번째 학생 이름은 %s 입니다\n", i + 1, names[i]);
	}
	printf("\n");
	for (int i = 0; i < sizeof(kor) / sizeof(kor[0]); i++)
	{
		kor[i] = (100);
		sum += kor[i];

		printf("%02d\t", kor[i]);
		if (i % 10 == 9)
			printf("\n");
	}
	printf("\n");

	avg = (double)sum / ST_NUM;

	printf("총합은 %d입니다\n", sum);
	printf("평균은 %.2lf입니다\n", avg);
}