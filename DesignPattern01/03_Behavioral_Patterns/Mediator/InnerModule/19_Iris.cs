using System;
namespace Mediator
{
    class Iris : InnerModule
    {
        int iris;
        public Iris(IChange mediator, int mid) : base(mediator, mid)
        {
            iris = 0;
        }
        public override void SetValue(int value)
        {
            this.iris = value;
            Console.WriteLine("조리개 F값:{0}", iris);
            Changed(iris);
        }
        public override void AlramChanged(int speed)
        {
            Console.WriteLine("셔터 스피드 변경 통보 받음");
            SetValue(10 - speed);
        }
    }
}