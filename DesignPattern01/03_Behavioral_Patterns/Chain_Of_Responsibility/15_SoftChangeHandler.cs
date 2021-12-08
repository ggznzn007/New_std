using System.Collections.Generic;
namespace ChainofResponsibility
{
    class SoftChangeHanler : ChangeHandler
    {
        public SoftChangeHanler(int hid)
            : base(hid)
        {
        }
        public override string ChangeRequest(List<int> mode, string picture)
        {
            if (IncludeMode(mode))
            {
                picture = picture.Replace("날카로운", "부드러운");
            }
            if (Successor != null)
            {
                picture = Successor.ChangeRequest(mode, picture);
            }
            return picture;
        }
    }
}