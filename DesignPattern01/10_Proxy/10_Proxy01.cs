/*프록시 패턴
    - 대리자로써 작업 수행을 요청하면 처리 후 그 결과를 알려주는 패턴
    - 클래스안에 처리과정이 복잡하고 리소스를 많이 차지할 경우 클래스를 생성하여 처리
    - 접근 제한을 둘 수 있기 때문에 원격제어 프로그램 작성 시에도 유용
    - 네트워크 연결, 메모리의 객체,복제할 수 없거나 자주 사용되는 리소스 등의 인터페이스 등에 사용
종류
    - 가상, 원격, 보호
장점
    1. 사이즈가 큰 객체(ex : 이미지)가 로딩되기 전에도 프록시를 통해 참조를 할 수 있다.
    2. 실제 객체의 public, protected 메소드들을 숨기고 인터페이스를 통해 노출시킬 수 있다. 
    3. 로컬에 있지 않고 떨어져 있는 객체를 사용할 수 있다. 
    4. 원래 객체의 접근에 대해서 사전처리를 할 수 있다.
단점
    1. 객체를 생성할때 한단계를 거치게 되므로, 빈번한 객체 생성이 필요한 경우 성능이 저하될 수 있다.
    2. 프록시 내부에서 객체 생성을 위해 스레드가 생성, 동기화가 구현되야 하는 경우 성능이 저하될 수 있다.
    3. 로직이 난해해져 가독성이 떨어질 수 있다.*/
using System;
// 인터페이스
interface INode
{
    // 추상 메소드
    void Print();
}
// Proxy 패턴 클래스
class NodeProxy : INode
{
    // INode를 상속받은 Node1클래스
    private class Node1 : INode
    {
        // 함수 재정의
        public void Print()
        {
            // 콘솔 출력
            Console.WriteLine("Node1 class");
        }
    }
    // INode를 상속받은 Node2클래스
    private class Node2 : INode
    {
        // 함수 재정의
        public void Print()
        {
            // 콘솔 출력
            Console.WriteLine("Node2 class");
        }
    }
    // 맴버 변수
    private INode node;
    private bool check;
    // 생성자
    public NodeProxy(bool check = true)
    {
        this.check = check;
    }
    // 함수 재정의
    public void Print()
    {
        if (node == null)
        {
            // 파라미터 check에 의해 생성되는 인스턴스가 다르다.
            if (check)
            {
                // Node1 인스턴스 생성
                node = new Node1();
            }
            else
            {
                // Node2 인스턴스 생성
                node = new Node2();
            }
        }
        // node 인스턴스의 print 함수 실행
        node.Print();
    }
}

// 실행 클래스
class Program
{
    // 실행 함수
    static void Main(String[] args)
    {
        // 파라미터가 없는 NodeProxy 인스턴스 생성
        INode node = new NodeProxy();
        // print 함수 호출
        node.Print();
        // 파라미터가 false인 NodeProxy 인스턴스 생성
        node = new NodeProxy(false);
        // print 함수 호출
        node.Print();
        // 아무 키나 누르시면 종료합니다.
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}

