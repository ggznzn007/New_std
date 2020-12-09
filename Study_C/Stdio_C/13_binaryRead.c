#include <stdio.h>

void main()
{
	FILE* fp = NULL;
	fopen_s(&fp, "data.bin", "rb");

	int num = 0;
	double fnum = 0.0;
	char ch = 0;
	int arr[3] = { 0 };

	fread(&num, sizeof(num), 1, fp);
	fread(&fnum, sizeof(fnum), 1, fp);
	fread(&ch, sizeof(ch), 1, fp);
	fread(arr, sizeof(int), 3, fp);

	printf("num : %d\n", num);
	printf("fnum : %lf\n", fnum);
	printf("ch : %c\n", ch);
	printf("arr : %d %d %d\n",
		arr[0], arr[1], arr[2]);

	fclose(fp);
}