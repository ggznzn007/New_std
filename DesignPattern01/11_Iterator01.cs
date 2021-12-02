/*이터레이터(반복자) 패턴
    - 객체 집합의 내용을 노출시키지 않고 집합의 원소들을 모든 항목에 순차적으로 접근할 수 있는 방법을 제공
    - 객체 내부의 표현 방식을 알 필요없이 집합의 각 원소들에 접근하고 싶을 때 사용
    - 서로 다른 구조의 객체 집합을 동일한 방식으로 순차 접근하고 싶을 때 사용
장점
    - 집합체 내에서 어떤 식으로 일이 처리되는지 대해서 전혀 모르는 상태에서 그 안에
      있는 모든 항목들에 대해서 반복작업을 수행가능
    - 클래스가 데이터를 저장함에 있어서 배열, ArrayList, 해쉬테이블 등등의 데이터 구조를 사용한다 할지라도 이터레이터
      인터페이스를 구현하게 함으로써 외부에서는 동일한 인터페이스로 각각의 항목에 대해서 순차적으로 접근이 가능*/
using System;
using System.Collections.Generic;

namespace Iterator
{
    class Program
    {
        static void Main(string[] args)
        {
            HotKey HotKey1 = new HotKey();
            HotKey1.addUnit(new Unit("마린1"));
            HotKey1.addUnit(new Unit("마린2"));
            HotKey1.addUnit(new Unit("마린3"));
            HotKey1.addUnit(new Unit("메딕1"));
            HotKey1.addUnit(new Unit("메딕2"));
            HotKey1.addUnit(new Unit("파이어뱃1"));

            UnitIterator Iterator = HotKey1.CreateIterator();

            while (Iterator.hasNext())
            {
                Console.WriteLine("핫키에 등록된 유닛:{0}", Iterator.Next().m_strName);
            }

            Console.WriteLine(Environment.NewLine);

            Observer Observer1 = new Observer();
            Observer1.addUnit(new Unit("적 다크템플러1"));
            Observer1.addUnit(new Unit("적 다크템플러2"));
            Observer1.addUnit(new Unit("적 다크템플러3"));
            Observer1.addUnit(new Unit("적 레이쓰1"));
            Observer1.addUnit(new Unit("적 레이쓰2"));
            Observer1.addUnit(new Unit("적 고스트1"));

            Iterator = Observer1.CreateIterator();

            while (Iterator.hasNext())
            {
                Console.WriteLine("옵저버1이 밝혀진 클락킹된 유닛:{0}", Iterator.Next().m_strName);
            }

            Console.ReadKey();
        }
        /// <summary>
        /// 유닛 클래스 생성
        /// </summary>
        class Unit
        {
            public string m_strName { get; set; }
            public Unit(string _strName)
            {
                m_strName = _strName;
            }
        }
        /// <summary>
        /// 반복자 추상 클래스 생성
        /// </summary>
        abstract class UnitIterator
        {
            public abstract Unit Next();
            public abstract bool hasNext();
        }
        /// <summary>
        /// 핫키를 관리하는 반복자 클래스 생성
        /// </summary>
        class HotkeyIterator : UnitIterator
        {
            private HotKey m_HotKey;
            private int m_intCurrent;
            public HotkeyIterator(HotKey _Hotkey)
            {
                m_HotKey = _Hotkey;
                m_intCurrent = 0;
            }
            public override Unit Next()
            {
                return m_HotKey[m_intCurrent++];
            }
            public override bool hasNext()
            {
                return m_intCurrent < m_HotKey.Count();
            }
        }
        class ObserverIterator : UnitIterator
        {
            private Observer m_Observer;
            private int m_intCurrent;

            public ObserverIterator(Observer _Observer)
            {
                this.m_Observer = _Observer;
                m_intCurrent = 0;
            }
            public override Unit Next()
            {
                return m_Observer[m_intCurrent++];
            }
            public override bool hasNext()
            {
                return m_intCurrent < m_Observer.Count();
            }
        }
        /// <summary>
        /// 유닛을 관리하는 추상 클래스 생성
        /// </summary>
        abstract class UnitContainer
        {
            public abstract UnitIterator CreateIterator();
        }
        /// <summary>
        /// 유닛을 소유하고 있는 핫키 클래스 생성
        /// </summary>
        class HotKey : UnitContainer
        {
            private Unit[] m_HotKeyUnit = new Unit[10];
            private int m_Count = 0;
            public override UnitIterator CreateIterator()
            {
                return new HotkeyIterator(this);
            }
            /// <summary>
            /// 유닛 추가 (배열)
            /// </summary>
            /// <param name="_Unit"></param>
            public void addUnit(Unit _Unit)
            {
                if (m_Count < 10)
                {
                    this.m_HotKeyUnit[m_Count] = _Unit;
                    m_Count++;
                }
            }
            public int Count()
            {
                return m_Count;
            }
            public Unit this[int _index]
            {
                get { return m_HotKeyUnit[_index]; }
            }
        }
        /// <summary>
        /// 클라킹된 유닛을 소유하는 옵져버 클래스 생성
        /// </summary>
        class Observer : UnitContainer
        {
            private List<Unit> m_ObserverUnit = new List<Unit>();
            public override UnitIterator CreateIterator()
            {
                return new ObserverIterator(this);
            }
            public void addUnit(Unit _Unit)
            {
                this.m_ObserverUnit.Add(_Unit);
            }
            public int Count()
            {
                return m_ObserverUnit.Count;
            }
            public Unit this[int _index]
            {
                get { return m_ObserverUnit[_index]; }
            }
        }
    }
}