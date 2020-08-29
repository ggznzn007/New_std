#include <iostream>

using namespace std;

int main()
{
	int age;

	cout << "여러분의 나이를 입력해주세요 : ";
	cin >> age;
	if (age <= 0)
	{
		cout << "유효하지 않은 수입니다 다시입력해주세요.";
	}
	else
		cout << "당신은" << age << "세 이군요." << endl;
	return 0;
}