namespace Mediator
{
    class InnerMediator : IChange
    {
        InnerModule[] modules = new InnerModule[2];
        int mode;
        public void SetInnerModule(int mid, InnerModule module)
        {
            modules[mid] = module;
        }
        public void ChangeMode(int mode)
        {
            this.mode = mode;
        }
        public void Change(int mid, int value)
        {
            if (mode != mid)
            {
                return;
            }
            if (modules[0].Mid == mid)
            {
                modules[1].AlramChanged(value);
            }
            else
            {
                modules[0].AlramChanged(value);
            }
        }
    }
}