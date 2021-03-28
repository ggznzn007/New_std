#include <iostream>
#include <conio.h>

using namespace std;

class Distance
{
private:
	int feet;
	float inches;
public:
	Distance()
	{
		feet = 0;
		inches = 0.0;
	}
	Distance(int ft, float in)
	{
		feet = ft;
		inches = in;
	}
	void getDist();
	void showDist();
	void addDist(Distance, Distance);
};

void Distance::getDist()
{
	cout << "Enter Feet & Inches : ";
	cin >> feet >> inches;
}

void Distance::showDist()
{
	cout << feet << "\'-" << inches << "\"";

}

void addDist(Distance dd1, Distance dd2)
{
	feet = dd1.feet + dd2.feet;
	inches = dd1.inches + dd2.inches;
	if (inches >= 12.0)
	{
		inches -= 12.0;
		feet++;
	}
}

void main()
{
	
	Distance d1;
	Distance d2(10, 6.0);
	Distance d3;
	d1.getDist();

	d1.showDist();
	cout << "+";
	d2.showDist();
	cout << "=";

	d3.addDist(d1, d2);
	d3.showDist();

	getch();
}
