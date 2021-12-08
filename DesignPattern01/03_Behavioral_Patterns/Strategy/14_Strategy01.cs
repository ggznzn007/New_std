/*Strategy Pattern 정의
 - 알고리즘군을 정의하고 각각을 캡슐화하여 교환해서 사용할 수 있도록 만든다.
   스트래티지를 활용하면 알고리즘을 사용하는 클라이언트와는 독립적으로 알고리즘을 변경할 수 있다.

UML
 - Context : Strategy의 구상객체를 소유하며 해당 구상객체의 메소드를 실행하는 메소드를 가지고 있다(ContextInterface)
 - Strategy : 알고리즘의 추상 객체 , 여러 타입의 구상객체들의 추상 객체가 된다.
 - ConcreteStrategyA~C : Context에 적용될 각각 다른 알고리즘으로 되있는 메소드들을 소유함
   
사용용도 및 장점
 - 상속으로 해결될 수 없는 코드 중복이나 객체의 실시간 알고리즘의 변경시에 유용하다.
 - Strategy 추상객체를 상속해서 신규 알고리즘을 추가하기가 용이하다. ( 확장성 )
 - 추후에 알고리즘이 변경시에도 해당 알고리즘만 변경되면 된다.(클라이언트와 독립적으로 변경 )*/

using System;

namespace DoFactory.GangOfFour.Strategy.Structural
{
   class MainApp
    {
       static void Main()
        {
            Context context;
            // Three contexts following different strategies
            context = new Context(new ConcreteStrategyA());
            context.ContextInterface();
            context = new Context(new ConcreteStrategyB());
            context.ContextInterface();
            context = new Context(new ConcreteStrategyC());
            context.ContextInterface();
            // Wait for user
            Console.ReadKey();
        }
    }
    /// <summary>
    /// The 'Strategy' abstract class
    /// </summary>
    abstract class Strategy
    {
        public abstract void AlgorithmInterface();
    }
    /// <summary>
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyA : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine(
              "Called ConcreteStrategyA.AlgorithmInterface()");
        }
    }
    /// <summary>
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyB : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine(
              "Called ConcreteStrategyB.AlgorithmInterface()");
        }
    }
    /// <summary>
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyC : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine(
              "Called ConcreteStrategyC.AlgorithmInterface()");
        }
    }
    /// <summary>
    /// The 'Context' class
    /// </summary>
    class Context
    {
        private Strategy _strategy;
        // Constructor
        public Context(Strategy strategy)
        {
            this._strategy = strategy;
        }
        public void ContextInterface()
        {
            _strategy.AlgorithmInterface();
        }
    }
}