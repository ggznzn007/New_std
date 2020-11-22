#include "turboc.h"	

void main()
{
	setcursortype(NOCURSOR);
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
		//x의 경계선에 닿으면 x의 변화
		if (x == 80 || x == 0)
			dirX *= -1;
		if (y == 25 || y == 0)
			dirY *= -1;
	}
}