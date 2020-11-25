//4. do~while문을 이용해서 1부터 10까지 중에 짝수만 출력하세요
//do~while문내에 조건문을 넣어주면 됨
//짝수 조건 if (num % 2 == 0)
#include <stdio.h>

void dowhile3(int num, int goal)
{
	do
	{
		num++;
		if (num % 2 == 0)
			printf("\n%d\n", num);
		if (num == goal)
			break;
	} while (1);
}

void main()
{
	dowhile3(1, 10);
}