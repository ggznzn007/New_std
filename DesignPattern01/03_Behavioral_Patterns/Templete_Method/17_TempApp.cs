using System;

namespace TempMethod
{
    class TempApp
    {
        public void Do()
        {
            InitInstance();
            Run();
            ExitInstance();
        }
        protected void Run()
        {
            ConsoleKey key = ConsoleKey.Escape;
            while ((key = SelectMenu()) != ConsoleKey.Escape)
            {
                switch (key)
                {
                    case ConsoleKey.F1: About(); break;
                    default: KeyProc(key); break;
                }
                Console.WriteLine("아무키나 누르세요");
                Console.ReadKey();
            }
        }
        protected virtual void InitInstance() { }
        protected virtual void ExitInstance() { }
        protected virtual void About()
        {
            Console.WriteLine("EH Camera");
        }
        protected virtual void ViewMenu()
        {
            Console.WriteLine("ESC:프로그램 종료 F1:제품 정보");
        }
        protected virtual void KeyProc(ConsoleKey key)
        {
            Console.WriteLine("잘못된 메뉴를 선택하였습니다.");
        }
        private ConsoleKey SelectMenu()
        {
            ViewMenu();
            Console.WriteLine("메뉴를 선택하세요.");
            return Console.ReadKey().Key;
        }
    }
}