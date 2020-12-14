#include "turboc.h"
#include "addressBookDefine.h"
#include "addressBookModel.h"

// 다른 c파일에 존재하는 전역변수를
// 접근하려면 아래처럼 사용한다.
extern int g_nAddrCnt;
extern AddressBook addressBook[ADDR_NUM];

void saveAddressBook()
{
	FILE* fp = NULL;
	fopen_s(&fp, "addressBook.bin", "wb");

	/* 파일 저장시
	1. 4byte는 배열의 개수
	2. 뒤에 구조체 배열 저장
	*/
	fwrite(&g_nAddrCnt, sizeof(int), 1, fp);
	fwrite(addressBook, sizeof(AddressBook),
		g_nAddrCnt, fp);

	fclose(fp);
}
void loadAddressBook()
{
	FILE* fp = NULL;

	// 정상적으로 파일이 열린 경우
	if (fopen_s(&fp, "addressBook.bin", "rb") == 0)
	{
		fread(&g_nAddrCnt, sizeof(int), 1, fp);
		fread(addressBook, sizeof(AddressBook),
			g_nAddrCnt, fp);
		fclose(fp);
	}
}
