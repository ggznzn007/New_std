#include "turboc.h"

#define WIDTH	80
#define HEIGHT	25

#define TRUE	1
#define FALSE	0

#define UP_KEY 72
#define DOWN_KEY 80
#define LEFT_KEY 75
#define RIGHT_KEY 77


void drawXLine(int sX, int eX, int yPos, const char* icon)
{
	int x = 0;
	for (x = sX; x <= eX; x++)
	{
		gotoxy(x, yPos);
		puts(icon);
	}
}

void drawYLine(int sY, int eY, int xPos, const char* icon)
{
	int y = 0;
	for (y = sY; y <= eY; y++)
	{
		gotoxy(xPos, y);
		puts(icon);
	}
}

void drawBar(int x, int y, char* bar)
{
	gotoxy(x, y);
	puts(bar);
}

void main()
{
	char* bar = "▣▣▣▣";
	int preX, preY;
	int newX, newY;
	preX = newX = WIDTH / 2 - strlen(bar) / 2;
	preY = newY = HEIGHT / 5 * 4;

	drawXLine(0, (WIDTH + 2) / 2, HEIGHT + 2, "▥");
	drawYLine(0, HEIGHT + 2, WIDTH + 2, "▤");
	drawBar(newX, newY, bar);

	while (TRUE)
	{
		preX = newX;
		preY = newY;

		if (GetAsyncKeyState(VK_UP) & 0x8000)
			newY--;
		if (GetAsyncKeyState(VK_DOWN) & 0x8000)
			newY++;
		if (GetAsyncKeyState(VK_LEFT) & 0x8000)
			newX--;
		if (GetAsyncKeyState(VK_RIGHT) & 0x8000)
			newX++;

		// 좌표의 변화가 있을 때만 다시 그리자
		if (newX != preX || newY != preY)
		{
			drawBar(preX, preY, "        ");
			drawBar(newX, newY, bar);
		}

		delay(50);
	}
}