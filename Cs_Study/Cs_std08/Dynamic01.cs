using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_std08
{
    class Dynamic01
    {
        public void M1()
        {
            // ExpandoObject에서 dynamic 타입 생성
            dynamic person = new ExpandoObject();

            // 속성 지정
            person.Name = "Kim";
            person.Age = 10;

            // 메서드 지정
            person.Display = (Action)(() =>
            {
                Console.WriteLine("{0} {1}", person.Name, person.Age);
            });

            person.ChangeAge = (Action<int>)((age) => {
                person.Age = age;
                if (person.AgeChanged != null)
                {
                    person.AgeChanged(this, EventArgs.Empty);
                }
            });

            // 이벤트 초기화
            person.AgeChanged = null; //dynamic 이벤트는 먼저 null 초기화함

            // 이벤트핸들러 지정
            person.AgeChanged += new EventHandler(OnAgeChanged);

            // 타 메서드에 파라미터로 전달
            M2(person);
        }

        private void OnAgeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Age changed");
        }

        // dynamic 파라미터 전달받음
        public void M2(dynamic d)
        {
            // dynamic 타입 메서드 호출
            d.Display();
            d.ChangeAge(20);
            d.Display();
        }
    }
}
