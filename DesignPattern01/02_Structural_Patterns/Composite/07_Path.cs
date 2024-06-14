using System;
namespace Composite
{
    class Path : Tree
    {
        public Path(string path) : base(path)
        {
        }

        public override void View()
        {
            Console.WriteLine("{0," + Size.ToString() + "}", Name);
        }
    }
}