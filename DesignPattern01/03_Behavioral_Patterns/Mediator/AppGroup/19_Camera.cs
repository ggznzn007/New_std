using System;
namespace Mediator
{
    class Camera
    {
        InnerMediator mediator = new InnerMediator();
        InnerModule[] modules = new InnerModule[2];
        public Camera()
        {
            modules[0] = new Iris(mediator, 0);
            modules[1] = new Shutter(mediator, 1);
            mediator.SetInnerModule(0, modules[0]);
            mediator.SetInnerModule(1, modules[1]);
            mediator.ChangeMode(0);
        }
        public void ChangeMode(int mode)
        {
            if ((mode != 0) && (mode != 1))
            {
                return;
            }
            if (mode == 0)
            {
                Console.WriteLine("***   조리개 우선 모드  ***");
            }
            else
            {
                Console.WriteLine("***   셔터 스피드 우선 모드  ***");
            }
            mediator.ChangeMode(mode);
        }
        public void SetIris(int iris)
        {
            modules[0].SetValue(iris);
        }
        public void SetShutterSpeed(int speed)
        {
            modules[1].SetValue(speed);
        }
    }
}