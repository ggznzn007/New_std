/* result는 비록 이름이 같은 변수이지만
 소속이 다르기 때문에 전혀 다른 변수이다
 서로한테 영향을 주지 않는다.

 c언어는 다른 함수와의 상호작용을
 매개변수와 리턴값으로 한다
 (이것이 공식적인 상호작용이다)
*/
#include <stdio.h>

int add(int num0, int num1)
{
	int result = 0;
	result = num0 + num1;
	return result;
}

void main()
{
	int result = 0;
	result = add(100, 200);
	printf("%d\n", result);
}