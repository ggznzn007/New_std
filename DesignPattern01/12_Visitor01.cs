/*Vistor(방문자) 패턴
    - 방문자패턴은 동작하는 객체로부터 알고리즘을 분리하는 패턴
구조
Visitor: Visitor 인터페이스는 인자로서 객체의 구조를 나타내는 세부 Element 를 취하는 방문자 메소드들을 선언한다.
         오버로딩을 지원하는 언어라면 같은 이름이면서 다른 타입을 취하는 메소드들을 정의한다.
Concrete Visitor: Concrete Element 클래스에 따라 같은 행동을 하는 여러 버전을 구현체 클래스들이다.
Element: Element 인터페이스는 방문자를 "accepting" 하기 위한 메소드를 정의하고 있다. 이 메소드는 방문자 인터페이스 타입의 파라미터를 가져야 한다.
Concrete Element: 각 Concrete Element 는 반드시 Element 의 "accepting" 메소드를 구현해야한다.
                  이 메소드의 목적은 적절한 방문자 메소드 호출을 해당하는 Element 클래스로 넘기는것이다.
                  비록 기본 Element 클래스가 이 메소드를 구현하고 있다고 해도, 모든 서브클래스들은 자신의 클래스에서 자신의 메소드를 오버라이딩 해야하고, 방문 객체의 적절한 메소드를 호출한다.
Client: 클라이언트는 대게 collection 이나 복잡 객체를 표현한다. 클라이언트는 추상 인터페이스를 통해 collection 의 객체들과
        교류하기 때문에 모든 세부 Element 클래스들을 알지 못한다.*/

using System;
using System.Collections.Generic;
interface IVisitor
{
    void Visit(Model1 model);
    void Visit(Model2 model);
    void Visit(Model3 model);
}
interface Model
{
    void Accept(IVisitor visitor);
}
class Model1 : Model
{
    public String Data { get; set; }
    public Model1()
    {
        Data = "Model1";
    }
    // 방문자 클래스가 Accept되면 방문자 클래스에 자기 자신을 등록한다.
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
class Model2 : Model
{
    public String Data { get; set; }
    public Model2()
    {
        Data = "Model2";
    }
    // 방문자 클래스가 Accept되면 방문자 클래스에 자기 자신을 등록한다.
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
class Model3 : Model
{
    public String Data { get; set; }
    public Model3()
    {
        Data = "Model3";
    }
    // 방문자 클래스가 Accept되면 방문자 클래스에 자기 자신을 등록한다.
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
class Controller : IVisitor
{
    // Model1 이 있을 때 호출
    public void Visit(Model1 model)
    {
        Console.WriteLine(model.Data);
    }
    // Model2 이 있을 때 호출
    public void Visit(Model2 model)
    {
        Console.WriteLine(model.Data);
    }
    // Model3 이 있을 때 호출
    public void Visit(Model3 model)
    {
        Console.WriteLine(model.Data);
    }
}

class Route : List<Model>
{
    public void Accept(IVisitor visitor)
    {
        foreach (var model in this)
        {
            model.Accept(visitor);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var route = new Route();
        route.Add(new Model1());
        route.Add(new Model2());
        route.Add(new Model3());

        // 결과 Model1 \n Model2 \n Model3
        route.Accept(new Controller());

        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }
}

