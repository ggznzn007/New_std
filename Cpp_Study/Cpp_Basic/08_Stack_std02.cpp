#include <iostream>

using namespace std;

int main() 
{
    int inputNumber;
    cin >> inputNumber;
    do 
    {
        cout << inputNumber % 10;
        inputNumber /= 10;
        if (inputNumber == 0) 
        {
            break;
        }
    } while (1);

    return 0;
}
