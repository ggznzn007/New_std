using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Adapter
{
// 보통 개발을 하다보면 여러회사의 콤포넌트를 연동을 필요가 생기게 될 때에 사용
    class Program
    {
        static void Main(string[] args)
        {
            List<NewUnit> ltUnit = new List<NewUnit>();
            ltUnit.Add(new NewUnit()); //신규 유닛
            ltUnit.Add(new Unit()); //기존 유닛
            foreach (NewUnit newUnit in ltUnit)
            {
                //같은 소스로 제어가능
                newUnit.Move();
                newUnit.Stop();
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 타겟 클래스
        /// </summary>
        class NewUnit
        {
            public virtual void Move()
            {
                Console.WriteLine("New Unit Moved !");
            }
            public virtual void Stop()
            {
                Console.WriteLine("New Unit Stopped !");
            }
        }

        /// <summary>
        /// 어뎁터클래스
        /// </summary>
        class Unit : NewUnit
        {
            private OldUnit _adaptee = new OldUnit();
            public override void Move()
            {
                _adaptee.MoveToPoint();
            }
            public override void Stop()
            {
                _adaptee.StopMove();
            }
        }

        /// <summary>
        /// 어뎁티 클래스
        /// </summary>
        class OldUnit
        {
            public void MoveToPoint()
            {
                Console.WriteLine("Old Unit Moved !");
            }
            public void StopMove()
            {
                Console.WriteLine("Old Unit Stopped !");
            }
        }
    }
}