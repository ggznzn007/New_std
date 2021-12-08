using System.Collections.Generic;
namespace ChainofResponsibility
{
    class GrayChangeHandler : ChangeHandler
    {
        public GrayChangeHandler(int hid) : base(hid)
        {
        }
        public override string ChangeRequest(List<int> mode, string picture)
        {
            if (IncludeMode(mode))
            {
                picture = picture.Replace("칼라", "흑백");
            }
            if (Successor != null)
            {
                picture = Successor.ChangeRequest(mode, picture);
            }
            return picture;
        }
    }
}