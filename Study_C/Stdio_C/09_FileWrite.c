/* 파일 입출력 3단계
1. 스트림 생성
2. 읽기 쓰기  동작
3. 스트림 닫기
운영체제가 생성해줌
*/

#include <stdio.h>	

void main()
{
	//1)스트림 생성
	FILE* fp = NULL;
	fopen_s(&fp, "test.txt", "wt");

	//2-1) 문자저장
	fputc('A', fp); fputc('\n', fp);
	fputc('B', fp); fputc('\n', fp);
	//2-2) 문자열 저장
	fputs("I Love You!\n", fp);
	fputs("호기도\n", fp);
	fputs("이꺼저\n", fp);
	fputs("자기사\n", fp);




	//3)스트림종료
	fclose(fp);
}