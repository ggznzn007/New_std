#include "turboc.h"	

enum
{
	INPUT_ADDR = 1,
	SEARCH_ADDR,
	UPDATE_ADDR,
	DELETE_ADDR,
	PRINTALL_ADDR,
	EXIT_ADDR
};

void viewMenu()
{
	printf("\t<주소록 관리 프로그램>\n");
	printf("1. 입력\n");
	printf("2. 검색\n");
	printf("3. 수정\n");
	printf("4. 삭제\n");
	printf("5. 전체출력\n");
	printf("6. 종료\n");
}

int getSelNum()
{
	int selNum = 0;
	printf("\n번호를 선택하세요 >> ");
	scanf_s("%d", &selNum);

	return selNum;
}


void inputAddress()
{
	printf("입력처리완료!!!\n");
}

void searchAddress()
{
	printf("검색완료!!!\n");
}

void updateAddress()
{
	printf("업데이트완료!!!\n");
}

void deleteAddress()
{
	printf("삭제완료!!!\n	");
}

void printAllAddress()
{
	printf("전체출력합니다!!!\n");
}

void exitApp()
{
	printf("프로그램을 종료합니다\n");
	exit(0);
}

int processWork(int selNum)
{
	int ret = 1;

	switch (selNum)
	{
	case INPUT_ADDR:
		inputAddress();
		break;
	case SEARCH_ADDR:
		searchAddress();
		break;
	case UPDATE_ADDR:
		updateAddress();
		break;
	case DELETE_ADDR:
		deleteAddress();
		break;
	case PRINTALL_ADDR:
		printAllAddress();
		break;
	case EXIT_ADDR:
		ret = 0;
		break;
	default:
		printf("잘못 입력했습니다. 다시 입력해주세요!\n");
		break;
	}
	return ret;
}

void laterProcess()
{
	_getch();// scanf_s에서 입력된 값을 처리
	_getch();// 아무 입력(화면 잠시 멈춤)
	clrscr();// 화면 청소
}

void main()
{
	int selNum = 0;
	int isRun = 1;
	while (isRun)
	{
		viewMenu();// 메뉴보여주기
		selNum = getSelNum(); //사용자 입력
		isRun = processWork(selNum); //처리
		laterProcess(); //후처리
	}
}