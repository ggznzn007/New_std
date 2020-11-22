#include "turboc.h"

void drawWall(int gaolVal, int any, int pt, const char* icon)
{
	int i = 0;
	for (i = 0; i <= gaolVal; i++)
	{
		if (any == 0)
			gotoxy(i, pt);

		else if (any == 1)
			gotoxy(pt, i);

		puts(icon);
	}
}

void main()
{
	clrscr();
	setcursortype(NOCURSOR);

	drawWall(84, 0, 27, "■");//가로축
	drawWall(27, 1, 83, "■");//세로축

	int x = 0, y = 0;
	int dirX = 1, dirY = 1;
	while (1)
	{
		gotoxy(x, y);
		puts("#");
		delay(30);
		gotoxy(x, y);
		puts(" ");
		x += dirX;
		y += dirY;
		if (x == 80 || x == 0)
			dirX *= -1;
		if (y == 25 || y == 0)
			dirY *= -1;
	}
}