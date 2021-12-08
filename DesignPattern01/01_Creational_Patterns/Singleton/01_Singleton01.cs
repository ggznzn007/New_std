using System;

namespace Singleton01
{
    class Program
    {
        static void Main()
        {
            Singleton s1 = Singleton.Instance();
            Singleton s2 = Singleton.Instance();
            if (s1 == s2)
            {
                Console.WriteLine("Objects are the same instance");
            }
            // Wait for user
            Console.ReadKey();
        }
    }
    //싱글톤 클래스
    class Singleton
    {
        private static Singleton _instance;
        //  'protected' 로 생성자를 만듦
        protected Singleton()
        {
        }
        // 'static'으로 메서드를 생성
        public static Singleton Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            //다중 쓰레드 환경일 경우 Lock 필요
            //if (_instance == null)
            //{      //  lock(_instance)<br>
            //  {
            //     _instance = new Singleton();
            //  }
            //}
            return _instance;
        }
    }
}


