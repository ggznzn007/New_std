#include "addressBookDefine.h"
#include "turboc.h"
#include "addressBookView.h"

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
	getchar();		// '\n'을 버퍼에서 제거

	return selNum;
}

void userInputAddress(AddressBook* pAddr)
{
	printf("이름 입력: ");
	fgets(pAddr->name, sizeof(pAddr->name), stdin);
	pAddr->name[strlen(pAddr->name) - 1] = '\0';	// '\n' -> '\0'
	printf("주소 입력: ");
	fgets(pAddr->address, sizeof(pAddr->address), stdin);
	pAddr->address[strlen(pAddr->address) - 1] = '\0';
	printf("나이 입력: ");
	char age[4] = { 0 };
	fgets(age, sizeof(age), stdin);	// "24"
	pAddr->age = atoi(age);		// "24" -> 24
	printf("키 입력: ");
	char height[10] = { 0 };
	fgets(height, sizeof(height), stdin); // "174.5"
	pAddr->height = (float)atof(height);	   //"174.5" -> 174.5
	printf("몸무게 입력: ");
	char weight[10] = { 0 };
	fgets(weight, sizeof(weight), stdin);	// "75.5"
	pAddr->weight = (float)atof(weight);	   // "75.5" -> 75.5
}

void laterProcess()
{
	getchar();		// scanf_s에서 입력된 \n을 처리
	getchar();		// 아무 입력(화면 잠시 멈춤)
	clrscr();		// 화면 청소
}

void showAddress(AddressBook* pAddr)
{
	printf("이름 : %s\n", pAddr->name);
	printf("주소 : %s\n", pAddr->address);
	printf("나이 : %d\n", pAddr->age);
	printf("키 : %.1f\n", pAddr->height);
	printf("몸무게 : %.1f\n", pAddr->weight);
	printf("----------------------\n\n");
}