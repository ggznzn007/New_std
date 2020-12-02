/*
 배열 : 동일한 성격의 변수가 여러 개 필요
     ex) 3명 학생의 국어점수
 구조체 : 하나의 사물/업무에 필요한 정보가 여러 개
     ex) 학생의 신상정보
        이름, 나이, 주소, 키, 몸무게
       - 미리 학생 자료형을 제공할 수 없으니까
        필요할 때 학생관련정보를 묶어서
        학생 자료형을 만들어 사용해라.
       - 학생 자료형으로 만든 변수는
        학생에 관련된 정보를 저장할 수 있다.
*/
/* 프로그래밍의 그룹
1) 로직을 그루핑하고 싶으면 => 함수
2) 변수들을 그루핑하고 싶으면 => 구조체
*/
#include <stdio.h>

struct Student
{
    char name[20];
    int age;
    char address[30];
    float height;
    float weight;
};

void showStudent(struct Student st)
{
    printf("이름 : %s\n", st.name);
    printf("나이 : %d\n", st.age);
    printf("주소 : %s\n", st.address);
    printf("키 : %.2f\n", st.height);
    printf("몸무게 : %.2f\n", st.weight);
}

void main()
{
    struct Student st;

    strcpy_s(st.name, sizeof(st.name), "홍길동");
    st.age = 34;
    strcpy_s(st.address, sizeof(st.address), "백두산");
    st.height = 180.8f;
    st.weight = 77.4f;

    showStudent(st);
}