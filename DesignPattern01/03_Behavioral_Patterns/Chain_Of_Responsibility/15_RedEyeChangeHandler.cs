using System.Collections.Generic;
namespace ChainofResponsibility
{
    class RedEyeChangeHandler : ChangeHandler
    {
        public RedEyeChangeHandler(int hid) : base(hid)
        {
        }
        public override string ChangeRequest(List<int> mode, string picture)
        {
            if (IncludeMode(mode))
            {
                picture = picture.Replace("빨간눈", "정상눈");
            }
            if (Successor != null)
            {
                picture = Successor.ChangeRequest(mode, picture);
            }
            return picture;
        }
    }
}