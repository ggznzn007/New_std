using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std
{
    class DelClass
    {
        // 델리게이트 선언
        private delegate void RunDelegate(int i);

        private void RunThis(int val)
        {
            // 콘솔출력 : 1024
            Console.WriteLine("{0}", val);
        }

        private void RunThat(int value)
        {
            // 콘솔출력 : 0x400
            Console.WriteLine("0x{0:X}", value);
        }

        public void Perform()
        {
            // 2. 델리게이트 인스턴스 생성
            RunDelegate run = new RunDelegate(RunThis);
            // 3. 델리게이트 실행
            run(1024);

            // run = new RunDelegate(RunThat); 을 줄여서 아래와같이 사용가능
            run = RunThat;
            run(1024);
        }


        class Pro
        {
            static void Main(string[] args)
            {
                DelClass dc = new DelClass();
                dc.Perform();
            }
        }
    }
}
