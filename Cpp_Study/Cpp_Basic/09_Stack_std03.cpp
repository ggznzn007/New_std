#include <iostream>
#include <stack>
#include <vector>

using namespace std;

int main()
{
	int a; 
	char cs[1000];
	stack<char> st;
	vector<char> result;

	cin >> a;
	cin >> cs;
	int i = 0;
	while (cs[i] != '\0')
	{
		st.push(cs[i]);
		i++;
	}

	char tmp;
	int cnt = 0;
	while (!st.empty())
	{
		cnt++;
		if (cnt % 4 == 0)
		{
			result.push_back(',');
		}
		else
		{
			tmp = st.top();
			result.push_back(tmp);
			st.pop();
		}
	}

	vector<char>::reverse_iterator itr = result.rbegin();
	for (itr; itr != result.rend(); itr++)
	{
		cout << *itr;
	}

	return 0;
}