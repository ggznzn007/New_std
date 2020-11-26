/*몫과 나머지를 함수를 통해 결과를 전달받고 싶다.
  그런데, C함수는 반드시 1개의 결과만 리턴할 수 있다
  그래서 우리는 구조체를 사용해서 몫과 나머지를
  묶어서 전달하겠다
*/

#include <stdio.h>

typedef struct _DivValue
{
	int quotient;  //몫
	int remainder; //나머지
}DivValue;

DivValue div(int num0, int num1)
{
	DivValue dv;
	dv.quotient = num0 / num1;
	dv.remainder = num0 % num1;
	return dv;
}

void main()
{
	DivValue dv = div(10, 3);
	printf("10/3의 몫은 %d입니다. \n", dv.quotient);
	printf("10/3의 나머지는 %d입니다. \n", dv.remainder);
}