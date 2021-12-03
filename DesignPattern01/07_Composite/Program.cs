namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Category("사진트리");
            Tree date_tree = MakeDemoDateTree();
            Tree location_tree = MakeDemoLocationTree();
            tree.AddChild(date_tree);
            tree.AddChild(location_tree);
            tree.View();
        }
        static Tree MakeDemoLocationTree()
        {
            Tree tree = new Category("지역");
            Tree sub1 = new Category("제주");
            Tree sub2 = new Category("천안");
            Tree sub3 = new Category("서울");
            Tree sub4 = new Path("C:\\사진\\외도");
            Tree sub2_1 = new Path("C:\\사진\\현충사의봄");
            sub2.AddChild(sub2_1);
            tree.AddChild(sub1);
            tree.AddChild(sub2);
            tree.AddChild(sub3);
            tree.AddChild(sub4);
            return tree;
        }
        static Tree MakeDemoDateTree()
        {
            Tree tree = new Category("날짜");
            Tree sub1 = new Category("20120429");
            Tree sub2 = new Category("20120407");
            Tree sub3 = new Category("20120507");
            Tree sub1_1 = new Path("C:\\사진\\외도");
            sub1.AddChild(sub1_1);
            Tree sub2_1 = new Path("C:\\사진\\현충사의봄");
            sub2.AddChild(sub2_1);
            Tree sub3_1 = new Path("C:\\사진\\일산고구마모종");
            sub3.AddChild(sub3_1);
            tree.AddChild(sub1);
            tree.AddChild(sub2);
            tree.AddChild(sub3);
            return tree;
        }
    }
}