#include <stdio.h>

void main()
{
	// 1) 스트림 생성
	FILE* fp = NULL;
	fopen_s(&fp, "test.txt", "rt");

	// 2) 파일 읽기
	int ch = fgetc(fp);  printf("%c", ch);
	ch = fgetc(fp);  printf("%c", ch);
	ch = fgetc(fp);  printf("%c", ch);
	ch = fgetc(fp);  printf("%c", ch);

	// 3) 스트림 종료
	fclose(fp);
}