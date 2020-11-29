/*
 함수에서 배열의 총합을 구해서 리턴해라

 포인터의 2번째 용법
 - 함수에 배열을 전달할 때 사용한다
 포인터는 배열의 시작주소를 받을 수 있다.
 그러므로 함수의 매개변수로 포인터변수를
 사용하면 배열을 받을 수 있다.
 단, 배열의 시작위치 받기 때문에 배열의 길이도
 별도로 받아야 한다.
*/
#include <stdio.h>

int sum(int ptr[], int len)
{
	int result = 0;

	for (int i = 0; i < len; i++)
		result += ptr[i];

	return result;
}

void main()
{
	int arr[] = { 10, 20, 30, 40, 50 };
	int arr1[] = { 10, 20, 30, 40, 50, 60, 70 };
	int result = 0;
	result = sum(arr, sizeof(arr) / sizeof(arr[0]));
	printf("arr의 합: %d\n", result);
	result = sum(arr1, sizeof(arr1) / sizeof(arr1[0]));
	printf("arr1의 합: %d\n", result);
}