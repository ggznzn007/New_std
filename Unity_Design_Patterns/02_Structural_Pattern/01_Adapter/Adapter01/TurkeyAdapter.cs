
// Duck ��ü�� ���ڶ� Turkey ��ü�� ��� ����ؾ� �ϴ� ��Ȳ
// �������̽��� �ٸ��� ������ Turkey��ü�� �ٷ� ����� ���� ����.
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