// 국어점수가 50이상 ~ 100이하 범위로 난수 생성
#include "turboc.h"

#define ST_NUM 100

void main()
{
	int kor[ST_NUM] = { 0 };
	int sum = 0;
	double avg = 0.0;

	randomize();//난수 초기화
	for (int i = 0; i < sizeof(kor) / sizeof(kor[0]); i++)
	{
		//printf("%d번째 학생 국어 점수 입력: ", i + 1);
		kor[i] = 50 + random(51);
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