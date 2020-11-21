#include "turboc.h"

void main()
{
	setcursortype(NOCURSOR);

	int x = 0;
	for (x = 0; x <= 80; x++)
	{
		clrscr();
		gotoxy(0, 0);
		puts("#");
		delay(100);
	}
	printf("감사합니다\n");
}