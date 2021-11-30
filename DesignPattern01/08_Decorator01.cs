using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*데코레이터 패턴
    - 상황 및 용도에 따라 객체에 필요한 요소들을 동적으로 추가한다.
    - 기능 확장이 필요할 경우 클래스를 상속하는 대신 유연하게 사용할 수 있다.
    - 하나의 객체에 여러 개의 데코레이터를 추가할 수 있다.
    - 장점 = 기존 코드를 수정하지 않고 확장할 수 있다.
    - 단점 = 클래스의 종류가 많아지고 객체를 파악하기 어려워진다.*/
namespace CSharp_DecoratorPattern
{
    abstract class Component
    {
        public abstract void Operation();
    }
    class ConcreteComponent : Component
    {
        public override void Operation()
        {
            Console.WriteLine("ConcreteComponent.Operation()");
        }
    }
    abstract class Decorator : Component
    {
        protected Component component;
        public void SetComponent(Component component)
        {
            this.component = component;
        }
        public override void Operation()
        {
            if (component != null)
            {
                component.Operation();
            }
        }
    }
    class ConcreteDecoratorA : Decorator
    {
        public override void Operation()
        {
            base.Operation();
            Console.WriteLine("ConcreteDecoratorA.Operation()");
        }
    }

    class ConcreteDecoratorB : Decorator
    {
        public override void Operation()
        {
            base.Operation();
            AddedBehavior();
            Console.WriteLine("ConcreteDecoratorB.Operation()");
        }
        void AddedBehavior()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConcreteComponent c = new ConcreteComponent();
            ConcreteDecoratorA d1 = new ConcreteDecoratorA();
            ConcreteDecoratorB d2 = new ConcreteDecoratorB();

            d1.SetComponent(c);
            d2.SetComponent(d1);

            d2.Operation();

            Console.ReadKey();
        }
    }
}