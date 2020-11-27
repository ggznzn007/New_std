/* int* ptr에서
   ptr변수는 2가지의 주소를 담는 것이 가능하다
   1) int 변수의 주소
   2) int 배열의 주소
*/
/* 포인터 변수는 배열의 시작주소를 가리키게
  할 수 있다.
  포인터 변수에 정수를 더하는 연산은
  일반적인 값의 산술연산이 아니라
  위치이동에 해당하는 연산이 된다.
  int* ptr = arr;
  ptr+1	 => int 크기만큼 다음위치로 이동
  ptr+2  => int*2 크기만큼 다음위치로 이동
  ptr-1  => int 크기만큼 이전위치로 이동
*/
#include <stdio.h>

void main()
{
	int num = 10;
	int arr[3] = { 10,20,30 };
	int* ptr = NULL;

	ptr = &num;
	printf("%d\n", *ptr);
	printf("배열의 이름: %p\n", arr);
	ptr = arr;
	printf("ptr의 값: %p\n", ptr);
	printf("%d\n", *ptr);
	printf("%d\n", *(ptr+0));
	printf("%d\n", *(ptr+1));
	printf("%d\n", *(ptr+2));
	printf("%d\n", ptr[0]);
	printf("%d\n", ptr[1]);
	printf("%d\n", ptr[2]);
}