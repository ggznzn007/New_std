using System;
using System.Collections.Generic;

namespace Composite
{
    class Category : Tree
    {
        List<Tree> children = new List<Tree>();
        public Category(string name) : base(name)
        {
        }
        public override void View()
        {
            Console.WriteLine("{0," + Size.ToString() + "}-C", Name);
            foreach (Tree child in children)
            {
                child.View();
            }
        }
        public override void AddChild(Tree child) //Category에만 필요한 기능
        {
            child.Parent = this;
            children.Add(child);
        }
        public override void RemoveChild(Tree child) //Category에만 필요한 기능
        {
            child.Parent = null;
            children.Remove(child);
        }
    }
}