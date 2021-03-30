#include <iostream>

using namespace std;

int main()
{
	int num;
	cout << "Enter any non-zero Number : ";
	cin >> num;
	if (num > 0)
	{
		cout << "Number is positive";
	}
	else
	{
		cout << "Number is negative";
	}

	return 0;
}