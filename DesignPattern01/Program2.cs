namespace Visitor

{

    class Program

    {

        static void Main(string[] args)

        {

            MyTour mt = new MyTour();

            Picture e = new Picture("A", 1, 1, 1);

            Diary d = new Diary("2012.6.1", "언제나 휴일");

            mt.AddElement(e);

            mt.AddElement(d);

            mt.ViewAll();

            mt.Reset();

            mt.ViewAll();

        }

    }

}


