namespace Composite
{
    abstract class Tree
    {
        public string Name { get; private set; }
        public Tree Parent { get; set; }
        public abstract void View();
        public virtual void AddChild(Tree child) { } //복합 개체에서만 필요한 기능
        public virtual void RemoveChild(Tree child) { }//복합 개체에서만 필요한 기능

        public int Size
        {
            get
            {
                return Name.Length + Level * 2;
            }
        }

        public int Level
        {
            get
            {
                int level = 0;
                Tree ancestor = Parent;
                while (ancestor != null)
                {
                    level++;
                    ancestor = ancestor.Parent;
                }
                return level;
            }
        }

        public Tree(string name)
        {
            Name = name;
            Parent = null;
        }
    }
}