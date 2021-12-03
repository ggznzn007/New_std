using System;
namespace Visitor
{
    class Viewer : IVisit
    {
        public void VisitPicture(Picture picture)
        {
            Console.WriteLine("사진 제목:{0}", picture.Name);
            picture.ViewInfo();
        }
        public void VisitDiary(Diary diary)
        {
            Console.WriteLine(diary.Name);
            diary.ViewContent();
        }
    }
}