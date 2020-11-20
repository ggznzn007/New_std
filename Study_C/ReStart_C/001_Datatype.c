#include<stdio.h>

int main(void)
{
	int num = 100;
	char ch = 'A';
	double fnum = 3.14;
	char str[] = "korea";

	printf_s("int %d\n", num);
	printf_s("char %c, %d\n", ch, ch);
	printf_s("float %lf\n", fnum);
	printf_s("chars %s\n", str);

	return 0;

}