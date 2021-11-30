using System;

// Controller 인터페이스
public interface Controller
{
    // 추상 함수
    void Execute(Model model);
}
// Model 인터페이스
public interface Model
{
    // 추상 함수
    String GetData();
}
// MVC 형태의 프레임워크에서 Model 클래스(파라미터)
public class ParameterModel : Model
{
    // 맴버 변수
    private string data;
    // 생성자
    public ParameterModel(string data)
    {
        // 맴버 변수 데이터에 넣는다.
        this.data = data;
    }
    // 함수 재정의, 데이터 취득 함수
    public String GetData()
    {
        // data 리턴
        return this.data;
    }
}
// MVC 형태의 프레임워크에서 Controller 클래스
public class MainController : Controller
{
    // 함수 재정의, model를 받는다.
    public void Execute(Model model)
    {
        // 콘솔 출력, model의 데이터를 취득
        Console.WriteLine("Execute - " + model.GetData());
    }
}
class Program
{
    // 실행 함수
    static void Main(string[] args)
    {
        // Controller 인스턴스
        var controller = new MainController();
        // 웹 요청시의 파라미터 model 인스턴스
        var model = new ParameterModel("Hello world");
        // Controller 실행
        controller.Execute(model);
        // 아무 키나 누르시면 종료합니다.
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}