using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* C++ => Template
 * C#, Java => Generic
 * Generic Programming
 * 논리는 동일하고 자료형이 다른 여러 기능을 수행할 때
 * => 1번만 정의하고 자료형만 다르게 적용하면
 *   컴파일러에 의해 코드가 재생산된다.
 * => 우리 눈에 보이는 코드는 1개지만
 *   컴파일러는 자료형에 따라 여러 개의 코드를 자동으로
 *   생성한다.
 */
namespace _10_ClassGeneric
{
    class WrapperInt
    {
        int Value;
        public WrapperInt() { Value = 0; }
        public WrapperInt(int aValue)
        { Value = aValue; }
        public int Data
        {
            get { return Value; }
            set { Value = value; }
        }
        public void OutValue()
        {
            Console.WriteLine(Value);
        }
    }

    class WrapperString
    {
        string Value;
        public WrapperString() { Value = null; }
        public WrapperString(string aValue)
        { Value = aValue; }
        public string Data
        {
            get { return Value; }
            set { Value = value; }
        }
        public void OutValue()
        {
            Console.WriteLine(Value);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WrapperInt gi = new WrapperInt(1234);
            gi.OutValue();
            WrapperString gs = new WrapperString("문자열");
            gs.OutValue();
        }
    }
}
