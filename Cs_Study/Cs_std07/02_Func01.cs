using System;
using System.Collections;
/*함수의 선언
관례적으로 함수의 이름은 대문자로 시작.
//함수 선언
[접근지정자] [리턴 타입] [함수 이름]([매개변수])
{
    //실행 코드
    return [리턴 결과];
}*/



public int Plus(int a, int b)
{
    int result = a + b;
    return result;
}
/*함수의 호출
함수의 이름을 호출하면, 함수 내 코드 실행.
*/
Plus(3,5);
> 8
