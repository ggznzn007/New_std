using System;
namespace Mediator
{
    class Shutter : InnerModule
    {
        int speed;
        public Shutter(IChange mediator, int mid) : base(mediator, mid)
        {
            speed = 10;
        }
        public override void SetValue(int value)
        {
            this.speed = value;
            Console.WriteLine("셔터 스피드:{0}", speed);
            Changed(speed);
        }
        public override void AlramChanged(int iris)
        {
            Console.WriteLine("조리개 F값 변경 통보 받음");
            SetValue(10 - iris);
        }
    }
}