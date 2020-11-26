/*
데니스 리치가 별도의 포인터 자료형을 만들지
않은 이유는
해당 변수의 시작주소뿐만 아니라 해석정보까지
필요하기 때문이다.
포인터 변수에서 가리키는 대상의 시작주소값이
들어있다
int* ptr로 선언하면 ptr에는 시작주소가 들어있고
ptr을 통해 값을 접근할 때는 int로 해석하라는
의미도 담겨있다.
*/
#include <stdio.h>

void main()
{
	int num = 100;
	char ch = 'A';
	double pi = 3.14;

	int* iptr = &num;
	char* cptr = &ch;
	double* dptr = &pi;

	printf("%p\n", iptr);
	printf("%p\n", cptr);
	printf("%p\n", dptr);
	printf("\n");

	printf("%d\n", *iptr);
	printf("%c\n", *cptr);
	printf("%lf\n", *dptr);
	printf("\n");
}