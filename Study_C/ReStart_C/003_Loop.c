#include <stdio.h>
#include <conio.h>
void main()
{
	printf_s("1.Insert\n");
	printf_s("2.Search\n");
	printf_s("3.Update\n");
	printf_s("4.Delete\n");
	printf_s("5.Exit\n");

	printf_s("Please select Number >>\n");
	char sel = _getch();
	printf_s("%c you got it\n", sel);

	return 0;
}