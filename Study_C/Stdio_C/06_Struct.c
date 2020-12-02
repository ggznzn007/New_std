#include <stdio.h>

// 구조체 => 사용자 정의 자료형
struct Student
{
    // 구조체의 멤버변수들
    char name[20];
    int age;
    char address[30];
    float height;
    float weight;
};
// struct Student 자료형의 별명을
// Stud로 짓겠다.
typedef struct Student Stud;

void showStudent(Stud* pSt)
{
    printf("이름 : %s\n", pSt->name);
    printf("나이 : %d\n", pSt->age);
    printf("주소 : %s\n", pSt->address);
    printf("키 : %.2f\n", pSt->height);
    printf("몸무게 : %.2f\n", pSt->weight);
}

void main()
{
    Stud st;

    strcpy_s(st.name, sizeof(st.name), "대조영");
    st.age = 34;
    strcpy_s(st.address, sizeof(st.address), "한라산");
    st.height = 180.6f;
    st.weight = 78.3f;

    showStudent(&st);
}