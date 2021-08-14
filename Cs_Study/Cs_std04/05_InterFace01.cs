using System;

class StudentDetail
{
    public int Roll_no;
    public string Name;

    public void getDetail(int x,string s)
    {
        Roll_no = x;
        Name = s;
    }

    public void showDetail()
    {
        Console.WriteLine("Roll No. :- " + Roll_no);
        Console.WriteLine("Name of Student :- " + Name);
    }

    interface Sports
    {
        void getsport(string s);
        void showSport();
    }

    class FullInfo:StudentDetail,Sports
    {
        string SportsName;
        public virtual void getsport(string s)
        {
            SportsName = s;
        }
        public virtual void showSport()
        {
            Console.WriteLine("Name of Sport:- " + SportsName);
        }

        public void ViewAll()
        {
            showDetail();
            showSport();
        }
    }

    class Demo_interface
    {
        public static void Main()
        {
            FullInfo f = new FullInfo();
            f.getDetail(1004, "Albert");
            f.getsport("Baseball");
            f.ViewAll();
            Console.ReadKey(true);
        }
    }
}