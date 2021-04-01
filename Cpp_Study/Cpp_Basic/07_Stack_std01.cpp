#include <iostream>
#include <stack>

using namespace std;

int main() 
{
    stack<int> s;
    int inputNumber, n;
    cin >> inputNumber;

    for (int i = 0; i < inputNumber; i++)
    {
        cin >> n;
        s.push(n);
    }

    for (int i = 0; i < inputNumber; i++)
    {
        cout << s.top() << " ";
        s.pop();
    }
    return 0;
}