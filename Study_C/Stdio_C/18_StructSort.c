/* cpu는 변수 접근 속도를 향상시키기 위해
  int의 크기는 4byte단위로 접근을 한다

  구조체는 동일한 멤버라 할지라도 순서에 따라
  크기가 달라지게 된다

  그래서 구조체 변수를 파일로 저장하게 되면
  St1과 St2의 파일 크기가 달라지게 된다.

  이럴 경우 2가지 문제가 있을 수 있다
  1) 임베디드 프로그래밍(소형 전자장비)에서는
	메모리의 크기가 비용과 연결된다
	그래서 최대한 사용하는 메모리를 줄일 필요가 있다
  2) 네트워크 통신시 문제가 생길 수 있다
	PC      : cpu 4byte 단위 정렬
	소형장비 : cpu 1byte 단위 정렬
	pc에서 구조체를 send(write) : St2를 전송 12byte
	소형장비 구조체를 recv(read) : St2를 수신 8byte
	데이터의 손실이 생길 수 있다.

	그래서 이럴 때는 이렇게 하는 경우가 많다
	#pragma pack
*/

#include <stdio.h>

typedef struct st1
{
	char ch;
	char ch2;
	short s;
	int i;
}St1;

typedef struct st2
{
	char ch;
	short s;
	int i;
	char ch2;
}St2;

void main()
{
	printf("St1의 크기: %d\n", sizeof(St1));
	printf("St2의 크기: %d\n", sizeof(St2));
}