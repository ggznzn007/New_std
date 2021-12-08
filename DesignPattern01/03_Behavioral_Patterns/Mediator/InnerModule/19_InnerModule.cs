namespace Mediator
{
    abstract class InnerModule
    {
        IChange mediator;
        public int Mid
        {
            get;
            private set;
        }
        public InnerModule(IChange mediator, int mid)
        {
            Mid = mid;
            this.mediator = mediator;
        }
        public void Changed(int value)
        {
            mediator.Change(Mid, value);
        }
        public abstract void SetValue(int value);
        public abstract void AlramChanged(int value);
    }
}