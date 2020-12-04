/* 파일입출력
; 1) 파일 저장, 읽기
  2) 3단계
	 2-1) 스트림 생성
	 2-2) 쓰거나 읽거나
	 2-3) 스트림 닫기
*/
//#include <stdio.h>
//
//void main()
//{
//	// 1) test.txt파일에 스트림을 생성한다
//	// write, text 모드로 
//	FILE* fp = NULL;
//	fopen_s(&fp, "test.txt", "wt");
//
//	// 2) 파일 저장
//	fputs("안녕하세요\n반갑습니다", fp);
//
//	// 3) 스트림 닫기
//	fclose(fp);
//}

#include <stdio.h>

void main()
{
	// 1) test.txt파일에 스트림을 생성한다
	// append, text 모드로 
	FILE* fp = NULL;
	fopen_s(&fp, "test.txt", "at");

	// 2) 파일 저장
	fputs("안녕하세요.반갑습니다\n", fp);

	// 3) 스트림 닫기
	fclose(fp);
}