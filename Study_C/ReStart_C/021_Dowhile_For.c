//3. do~while문을 이용해서 1부터 10까지 합을 출력하세요
//for문을 이용해서 1부터 10까지 합을 출력하세요
#include <stdio.h>

void dowhile2(int i, int num)
{
	int sum = 0;
	do
	{
		sum += i++;
	} while (i <= num);
	printf("\n%d\n", sum);
}

void for2(int i, int num)
{
	int sum = 0;
	for (int i = 1; i <= num; i++)
		sum = sum + i;
	printf("\n%d\n", sum);
}

void main()
{
	dowhile2(1, 10);
	for2(1, 10);
}