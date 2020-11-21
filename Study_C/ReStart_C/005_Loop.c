#include <stdio.h>	
#include <conio.h>

void main()
{
	while (1)
	{
		printf("1.주소 입력\n");
		printf("2.주소 검색\n");
		printf("3.주소 수정\n");
		printf("4.주소 삭제\n");
		printf("5.프로그램 종료\n");

		printf("번호를 선택하세요 >>\n");
		char sel = _getch();
		printf("%c 번 선택하셨습니다.\n", sel);
		if (sel == '5')
		{
			printf("안녕히 계세요^^\n");
			break;
		}
		else
		{
			printf("업무를 처리하였습니다~\n");
			printf("************************\n\n");
		}

	}
	return 0;
}