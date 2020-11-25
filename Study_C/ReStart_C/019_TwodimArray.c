/*
-주소 전용 변수 : 포인터 변수
int* ptr
1) int변수의 주소를 담을 수 있다
   ; int를 가리킬 수 있다
2) int 배열의 시작주소를 담을 수 있다
*/

#include <stdio.h>

void main()
{
	int arr[2][3] = { {99,88,77},{98,89,76} };
	int* ptr = &arr[0][0];
	for (int i = 0; i < sizeof(arr) / sizeof(arr[0][0]); i++)
	{
		// 2차원 배열을 1차원 배열처럼 접근
		printf("%d ", ptr[i]);
	}
}