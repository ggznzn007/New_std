using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// delegate 반환형 델리게이트명(매개변수..); 대신 일을 해주는 녀석

namespace Delegate01
{
    delegate int PDelegate(int a, int b);

    class Program
    { 
        static int Plus(int a, int b)
        {
            return a + b;
        } 
        static void Main(string[] args) 
        { 
            PDelegate pd1 = Plus;
            PDelegate pd2 = delegate (int a, int b)
            {
                return a / b;
            };
            Console.WriteLine(pd1(5, 10));
            Console.WriteLine(pd2(10, 5));
        } 
    }    
}

/*코드를 보시면 9행에 PDelegate라는 델리게이트가 보이죠?
매개변수 부분에는 int형 매개변수 a, b를 명시해주었습니다.
13~16행에서는 Plus란 메소드가 정의되었습니다. 이제부터 자세히 보셔야 합니다.
20행을 먼저 봅시다. Plus 메소드 자체를 델리게이트에 집어넣고 있는것 같죠?
Plus 메소드와 연결하여 대리자를 인스턴스화 합니다.
이제부터 델리게이트 pd1은 Plus 메소드를 참조하게 됩니다.
26행을 보시면 Plus 메소드를 쓰듯, a와 b를 더해서 값을 반환합니다.
21행을 보시면 아무 이름이 없는 메소드를 델리게이트에 집어넣습니다.
별도로 메소드를 만들지 않았죠? 이런 무명 메소드의 사용은 한번 사용하면
불필요해지는 메소드를 만들때도 사용되는 등, 매우 유용합니다.
27행도 26행과 마찬가지로 pd1처럼 사용할 수 있습니다.*/

