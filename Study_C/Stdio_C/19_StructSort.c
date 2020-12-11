#include <stdio.h>

// 전처리기한테 1byte단위로 정렬해
#pragma pack(push, 1)
typedef struct st1 
{
	char ch;
	char ch2;
	short s;
	int i;
}St1;
#pragma pack(pop)

#pragma pack(push, 1)
typedef struct st2 
{
	char ch;
	short s;
	int i;
	char ch2;
}St2;
#pragma pack(pop)

void main()
{
	printf("St1의 크기: %d\n", sizeof(St1));
	printf("St2의 크기: %d\n", sizeof(St2));
}