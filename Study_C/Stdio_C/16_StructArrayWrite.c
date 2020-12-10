#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define ST_NUM 3

typedef struct _Student
{
	char name[20];
	char address[30];
	int age;
	float height;
	float weight;
}Student;

void inputStudent(Student* pSt)
{
	printf("이름 입력:");
	fgets(pSt->name, sizeof(pSt->name), stdin);
	pSt->name[strlen(pSt->name) - 1] = '\0';//엔터값을 널로
	printf("주소 입력:");
	fgets(pSt->address, sizeof(pSt->address), stdin);
	pSt->address[strlen(pSt->address-1)] = '\0';//엔터값을 널로
	printf("나이 입력:");
	char age[4] = { 0 };
	fgets(age, sizeof(age), stdin);
	pSt->age = atoi(age);//문자를 숫자로
	printf("키 입력:");
	char height[10] = { 0 };
	fgets(height, sizeof(height), stdin);
	pSt->height = atof(height);//문자를 실수로
	printf("몸무게 입력:");
	char weight[10] = { 0 };
	fgets(weight, sizeof(weight), stdin);
	pSt->weight = atof(weight);
}

void main()
{
	FILE* fp = NULL;
	fopen_s(&fp, "student.bin", "wb");

	Student st[ST_NUM] = { 0 };
	for (int i = 0; i < ST_NUM; i++)
	{
		inputStudent(&st[i]);//= inputStudent(st+i);
	}
	fwrite(&st, sizeof(st), ST_NUM, fp);

	fclose(fp);
}