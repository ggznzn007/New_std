// 함수의 모양
//리턴타입 함수명(매개변수)
//{영역의 시작
//return 리턴타입변수;
//}영역의 끝
//<함수를 설계할 때 정해줘야 하는 것>
//1. 함수명		- 무조건 존재
//2. 프로세스	- 무조건 존재
//3. 매개변수	- 변화 = 지역변수
//4. 리턴값		- 변화

#include <stdio.h>

int getNum()
{
	int num = 0;
	printf("숫자 입력 : ");
	scanf_s("%d", &num);
	return num;
}

int add(int num0, int num1)
{
	return num0 + num1;
}

void viewResult(int result)
{
	
	printf("계산 결과는 %d 입니다.\n", result);
}

void viewTitle()
{
	printf("********덧셈 연산입니다 ************\n");
}

void main()
{
	viewTitle();
	int num0 = getNum();
	int num1 = getNum();
	
}