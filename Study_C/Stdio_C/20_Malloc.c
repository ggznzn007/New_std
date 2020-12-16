/* 메모리 동적할당
포인터의 3번째 용법
- 힙영역에 할당
- 필요할 때 할당/삭제 마음대로 할 수 있음
- 큰 메모리가 필요할 때
*/
#include <stdio.h>

char* getName()
{
	char name[20];
	printf("이름 입력: ");
	fgets(name, sizeof(name) - 1, stdin);
	printf("이름 : %s\n", name);
	printf("name : %p\n", name);
	return name;
}
void main()
{
	char* pName = getName();
	printf("pName : %p\n", pName);
	printf("이름: %s\n", pName);
}