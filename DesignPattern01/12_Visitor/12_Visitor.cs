namespace Visitor
{
    interface IVisit
    {
        void VisitPicture(Picture picture);
        void VisitDiary(Diary diary);
    }
}