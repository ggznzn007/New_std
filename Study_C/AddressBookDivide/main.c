#include "addressBookDefine.h"

AddressBook addressBook[ADDR_NUM];	// 주소록 정보 저장 배열
int g_nAddrCnt = 0;	   // 저장 인원수/다음 저장 위치

void main()
{
	int selNum = 0;
	int isRun = 1;	// 1이면 반복, 0이면 종료
	loadAddressBook();
	while (isRun)
	{
		viewMenu();						// 1. 메뉴 보여주기
		selNum = getSelNum();			// 2. 사용자 입력
		isRun = processWork(selNum);	// 3. 처리
		laterProcess();					// 4. 후처리
	}
}