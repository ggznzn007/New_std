
// Duck 객체가 모자라서 Turkey 객체를 대신 사용해야 하는 상황
// 인터페이스가 다르기 때문에 Turkey객체를 바로 사용할 수는 없다.
public class TurkeyAdapter : Duck
{
    Turkey turkey;

    public TurkeyAdapter(Turkey turkey)
    {
        this.turkey = turkey;
    }

    public void quack()
    {
        turkey.gobble();
    }

    public void fly()
    {
        turkey.fly();
    }
}