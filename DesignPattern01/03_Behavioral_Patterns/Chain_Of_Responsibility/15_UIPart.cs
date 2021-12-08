using System.Collections.Generic;
namespace ChainofResponsibility
{
    class UIPart
    {
        ChangeHandler head = null;
        ChangeHandler tail = null;
        public void AddChangeHandler(ChangeHandler handler)
        {
            if (head != null)
            {
                tail.Successor = handler;
                tail = handler;
            }
            else
            {
                head = tail = handler;
            }
        }
        public string ChangeRequest(List<int> mode, string subject)
        {
            if (head != null)
            {
                return head.ChangeRequest(mode, subject);
            }
            return subject;
        }
    }
}