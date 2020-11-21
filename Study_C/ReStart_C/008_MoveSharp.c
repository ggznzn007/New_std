#include "turboc.h"

//void main() 1번
//{
//	setcursortype(NOCURSOR);
//	int x = 0;
//	int y = 0;
//	for (x = 0; x <= 80; x++)
//	{
//		for (y = 0; y <= 80; y++)
//		{
//			clrscr();
//			gotoxy(x, 2);
//			gotoxy(y, 12);
//			puts("#");
//			delay(1);
//			puts(" ");
//		}
//	}
//} 

void main()
{
	setcursortype(NOCURSOR);
	int x = 0;
	int y = 0;
	while(1)
		for (x = 0; x <= 80; x++)
		{
			if(x==80)
				for (y = 80; y > 0; y--)
				{
					clrscr();//화면 청소
					gotoxy(y, 12);
					puts("#");
					delay(1);
					puts(" ");
				}
			clrscr();
			gotoxy(x, 12);
			puts("#");
			delay(1);
			puts(" ");

		}
}