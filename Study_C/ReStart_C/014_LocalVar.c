// 지역변수(local variable)
// 1. 영역내에 존재하는 변수{}
// 2. 영역이 시작될 때 탄생
// 3. 영역이 끝날 때 소멸
#include <stdio.h>

void main()
{
	int result = 0;
	//int result = 0; // 동일영역 중복명칭 안됨
	for (int i = 1; i <= 10; i++)
	{
		int result = 0;// for문 소속
		result += i;
		printf("결과는 %d 입니다\n", result);
	}
	printf("결과는 %d 입니다\n", result);
}
// result 가 for문 영역에서만 유효하다