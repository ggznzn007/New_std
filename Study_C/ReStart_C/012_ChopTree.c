//1. 숫자를 입력받고, 숫자만큼 "나무를 찍으세요".
//다 찍으면 "나무를 캐었다"	출력
//
//2. 1번을 함수로 만드록 함수에 숫자를 전달하면
//"나무를 찍는다"를 반복하세요.
//다 찍으면 "나무를 캐었다" 출력
//3. 함수를 만들어서 1번째 인자는 반복회수,
//2번째 인자는 해당번째 마다 "쿵"을 출력
//ex) chopTree(10, 3);
//"나무를 찍었다"
//"나무를 찍었다"
//"나무를 찍었다"
//"쿵"

#include <stdio.h>

void chopTree(int tryagain)
{
	int cnt = 0;
	int n;

	for (cnt = 1; cnt <= tryagain; ++cnt)
	{
		printf_s("\n나무를 몇 번 찍을까요?\n", cnt);
		scanf_s("%d", &cnt, &n);
		printf("\n");
		printf_s("나무를 찍었습니다. 쿵 !!! \n", cnt);
		printf("\n");

		if (cnt == tryagain)
		{
			printf("쿵 !!! 나무를 베었습니다!!!\n");
			printf("\n");

		}
		else if (cnt < tryagain)
		{
			printf("나무를 베지 못했습니다.\n");
		}
	}
}

void main()
{
	chopTree(16,3);
}