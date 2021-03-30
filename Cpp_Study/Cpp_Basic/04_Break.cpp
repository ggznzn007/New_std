#include <iostream>

using namespace std;

int main()
{
	int num = 10;

	do
	{
		cout << "value of number: " << num << endl;
		num = num + 1;
		if (num > 15)
		{
			break;
		}

	} 	while (num < 20);
	return 0;
}