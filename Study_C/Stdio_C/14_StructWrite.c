#include <stdio.h>

typedef struct _Student
{
	char name[20];
	char address[30];
	int age;
	float height;
	float weight;
}Student;

void main()
{
	FILE* fp = NULL;
	fopen_s(&fp, "student.bin", "wb");

	Student st = { "홍길동", "지리산 청학동", 21, 180.3f, 75.2f };

	fwrite(&st, sizeof(st), 1, fp);

	fclose(fp);
}