namespace Visitor

{

    class Initializer : IVisit

    {

        public void VisitPicture(Picture picture)

        {

            picture.SetInfo(0, 0, 0);

        }

        public void VisitDiary(Diary diary)

        {

            diary.SetContent("내용 없음");

        }

    }

}



