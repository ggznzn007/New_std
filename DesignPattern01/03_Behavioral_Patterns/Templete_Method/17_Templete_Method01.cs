/*템플릿 메서드 패턴 - 단계별로 수행하는 행위에 대한 약속을 기반 클래스에서 정의를 하고
                  각 단계별로 수행하는 구체적 구현은 파생 클래스에서 재정의하는 형태의 패턴
                - 비슷한 형태의 프로그램 구현에 필요한 뼈대를 일반화하여 제공을 하는 것은 실무에서 자주 사용하는 기법
                - 세부 기능의 흐름을 약속과 세부적인 기능의 구체적 구현을 분리하는 패턴
특징 및 장점
    일반화 과정을 통해 작성된 뼈대가 마련이 되어 있다면 실제 프로그래밍을 할 때에는 이를 기반으로 
    세부적인 기능에 대한 재정의를 통해 효과적인 프로그래밍을 할 수 있습니다.
    설계 단계에서 프로그램 뼈대를 고민하는 비용을 줄일 수 있게 되고 기술 노하우를 결집 시킬 수 있게 해 줍니다.*/
using System;
abstract class AbstractController
{
    // 추상 클래스에서는 사용하진 않지만 상속받은 클래스에서 override해야한다.
    protected abstract void Init();
    protected abstract String Result();
    // 추상 메서드를 사용한다.
    public void Run()
    {
        Init();
        var ret = Result();
        Console.WriteLine(ret);
    }
}

class Controller : AbstractController
{
    private String data;
    // Run 매소드가 호출되면 사용되는 함수
    protected override void Init()
    {
        this.data = "hello world";
    }
    // Run 메서드가 호출되면 사용되는 함수
    protected override string Result()
    {
        return this.data;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var controller = new Controller();
        // controller에서 Init과 Result는 protected로 되어 있어서 사용할 수 없다.
        // 결과는 hello world
        controller.Run();

        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}