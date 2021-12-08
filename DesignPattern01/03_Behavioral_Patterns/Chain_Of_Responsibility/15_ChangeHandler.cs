using System.Collections.Generic;

namespace ChainofResponsibility
{
    abstract class ChangeHandler
    {
        public ChangeHandler Successor
        {
            get;
            set;
        }
        readonly int hid;
        public ChangeHandler(int hid)
        {
            this.hid = hid;
            Successor = null;
        }
        public abstract string ChangeRequest(List<int> mode, string picture);
        protected bool IncludeMode(List<int> mode)
        {
            foreach (int m in mode)
            {
                if (m == hid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
