/*옵저버 패턴
  -  한 객체의 상태가 바뀌면 그 객체에 의존하는 다른 객체들한테 연락이 가서 자동으로 내용이 갱신되는 방식.
  -  1:N 의존성을 정의

플로우
1.Subject 객체가 bCheck 값 변경이 일어나면 Observer 객체들에게 이를 알린다.
2.각각의 Observer 객체들의 update() 함수에서 이를 감지하고 이에 따른 동작들을 수행한다.
3.Subject 가 Observer 에 대해서 아는 것은 Observer 가 특정 인터페이스를 구현한다는 것 뿐
  예를 들어 Observer 객체들은 모두 update() 함수를 가지며 이에서 변화를 감지한다.
4.Observer 는 언제든지 새로 추가할 수 있다.
5.Subject 는 Observer 인터페이스를 구현하는 객체 목록에만 의존하기 때문.
6.새로운 형식의 Observer를 추가하려 해도 Subject 를 전혀 변경할 필요가 없다.
7.새로운 클래스에서 Observer 인터페이스만 구현해주면 된다.
8.Subject 나 Observer 가 바뀌더라도 서로에게 전혀 영향을 주지 않는다. 그래서 Subject 와 Observer 는 서로 독립적으로 재사용할 수 있다.
9.유니티, C# 에서의 델리게이트와 같다. 델리게이트가 곧 이 옵저버 패턴인 것이다. 델리게이트 사용하면 옵저버 객체들을 리스트로 관리할 필요가 없다.*/

// 옵저버 추상클래스
// : 옵저버들이 구현해야 할 인터페이스 메서드
public abstract class Observer
{
    // 상태 update 메서드
    public abstract void OnNotify();
}