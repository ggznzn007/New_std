using System;

namespace Visitor

{

    abstract class Element

    {

        public string Name

        {

            get;

            private set;

        }

        public Element(string name)

        {

            Name = name;

        }

        public abstract void Accept(IVisit visitor);

    }

}