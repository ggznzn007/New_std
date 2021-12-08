// 대상 인터페이스
// : 옵저버 관리, 활용에 관한 타입 정의
public interface ISubject
{
    void AddObserver(Observer o);
    void RemoveObserver(Observer o);
    void Notify();
}