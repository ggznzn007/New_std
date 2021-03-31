#include <iostream>

using namespace std;

int main()
{
	double pob = 1;
	int cnt = 0;

	while (cnt < 0.01)
	{
		cnt++;
		pob = (pob * 0.5);
	}

	cout << pob; // 아직 확실히 풀지 못한 문제
}