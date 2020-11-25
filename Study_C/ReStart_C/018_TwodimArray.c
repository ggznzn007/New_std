// 2명 학생의 3과목(국,영,수) 점수 입력받자
// 총점과 평균 출력

#include <stdio.h>

/*
int [3][4]가 2그룹으로 존재한다
int arr[2][3][4];
*/

void main()
{
	// int [3]개가 2그룹으로 존재한다
	// 2행 3열 
	int arr[2][3] = { {99,34,68},{98,79,45} };
	for (int i = 0; i < 2; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			printf("%d ", arr[i][j]);
		}
		printf("\n");
	}
}