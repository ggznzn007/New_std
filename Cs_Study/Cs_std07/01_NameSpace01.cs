﻿/* namespace :
프로그래밍을 할 때, 가장 큰 어려움은 클래스 이름, 함수 이름, 메소드 이름, 변수 이름을 정하는 것.
프로젝트의 규모가 크면 커질수록 각종 이름들의 충돌 발생 가능성이 점점 커짐.
이 문제점을 해결해 주는 방법이 네임스페이스!
네임스페이스는 클래스들의 묶음이고, 반대로 클래스들은 네임스페이스를 이용하여 분류할 수 있음*/

// 네임스페이스 선언
public class A { }
public class B { }
public class C { }

namespace group
{
    class A { };
    class B { };
    class C { };
}

// 네임스페이스는 중첩도 가능
namespace group
{
    namespace group2
    {
        class A { };
        class B { };
        class C { };
    }

}

//네임스페이스 참조
//전체 네임스페이스를 다 적어주는 방식
//UnityEngine.Debug.Log("네임스페이스 1");
//using 지시문을 사용 방식
/*using UnityEngine;  

public class NamespacesExample : MonoBehaviour
{
    void Start()
    {
        Debug.Log("네임스페이스 1");
    }
}*/