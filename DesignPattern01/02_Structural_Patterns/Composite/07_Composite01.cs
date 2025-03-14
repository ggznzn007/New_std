﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 /* 컴포지드 패턴 ---> 객체들의 관계를 트리 구조로 구성하여 부분-전체 계층을 표현하는 패턴
                     사용자가 단일 객체와 복합 객체 모두 동일하게 처리함
 사용방법
 - 객체의 구성 및 상하위 체계를 파악
 - 파악된 객체들을 트리 구조로 설계
 - 객체와 복합체는 공통으로 사용할 수 있는 메소드가 정의된 인터페이스/추상클래스를 구현과 상속 */
namespace CSharp_CompositePattern
{
    abstract class Component
    {
        protected string name;
        public Component(string name)
        {
            this.name = name;
        }
        public abstract void Add(Component c);
        public abstract void Remove(Component c);
        public abstract void Display(int depth);
    }

    class Composite : Component
    {
        private List<Component> _children = new List<Component>();
        public Composite(string name) : base(name)
        {
        }
        public override void Add(Component component)
        {
            _children.Add(component);
        }
        public override void Remove(Component component)
        {
            _children.Remove(component);
        }
        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);

            foreach (Component component in _children)
            {
                component.Display(depth + 2);
            }
        }
    }

    class Leaf : Component
    {
        public Leaf(string name) : base(name)
        {
        }
        public override void Add(Component c)
        {
            Console.WriteLine("Cannot add to a leaf");
        }
        public override void Remove(Component c)
        {
            Console.WriteLine("Cannot remove from a leaf");
        }
        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Composite root = new Composite("root");
            root.Add(new Leaf("Leaf A"));
            root.Add(new Leaf("Leaf B"));

            Composite comp = new Composite("Composite X");
            comp.Add(new Leaf("Leaf XA"));
            comp.Add(new Leaf("Leaf XB"));

            root.Add(comp);
            root.Add(new Leaf("Leaf C"));

            Leaf leaf = new Leaf("Leaf D");
            root.Add(leaf);
            root.Remove(leaf);

            root.Display(1);

            Console.ReadKey();
        }
    }
}
