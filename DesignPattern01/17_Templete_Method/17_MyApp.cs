using System;

namespace TempMethod
{
    class MyApp : TempApp
    {
        protected override void InitInstance()
        {
            Console.WriteLine("사진 관리자 프로그램 V0.1");
            Console.WriteLine("아무키나 누르세요.");
        }
        protected override void ExitInstance()
        {
            Console.WriteLine("사진 관리자 프로그램을 종료합니다.");
        }
        protected override void About()
        {
            base.About();
            Console.WriteLine("응용 개발팀 2021. 12. 08");
        }
        protected override void ViewMenu()
        {
            base.ViewMenu();
            Console.WriteLine("F2: 사진 추가 F3: 사진 검색");
        }
        protected override void KeyProc(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.F2: AddPicture(); return;
                case ConsoleKey.F3: SearchPicture(); return;
            }
            base.KeyProc(key);
        }
        void AddPicture()
        {
            Console.WriteLine("사진 추가 기능을 선택하였습니다.");
        }
        void SearchPicture()
        {
            Console.WriteLine("사진 검색 기능을 선택하였습니다.");
        }
    }
}