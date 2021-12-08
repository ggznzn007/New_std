/*State Pattern 정의 - 클래스의 상태에 따라 처리되는 결과 값이 다른 형태로 만드는 것
                  - 객체의 내부 상태가 바뀜에 따라서 객체의 행동을 바꿀 수 있는 패턴
                  - 마치 객체의 클래스가 바뀌는 것과 같은 결과를 얻을 수 있는 패턴
사용용도 및 장점
  - 객체의 상태에 따라서, 동일한 동작이라도 다른 처리를 해야할때 사용
  - 하나의 객체에 여러가지의 상태가 존재할때 보통 IF문이나 SWITCH 문으로 분기를 해서 결과를 처리
    그러나 신규 상태가 존재할때마다 기존의 IF문이나 SWITCH이 다시 수정되어야 한다
  - 객체의 상태를 클래스화해서 그것을 참조하게 하는식으로 소스의 변화를 최소화 가능*/

using System;

namespace State
{
    class Program
    {
        static void Main(string[] args)
        {
            SiegeTank tank1 = new SiegeTank(new NormalState());

            tank1.Attack();
            tank1.Move();

            Console.WriteLine("\n");

            tank1.Attacked("Medic Blind");

            tank1.Attack();
            tank1.Move();

            Console.WriteLine("\n");

            tank1.Attacked("Ghost LockDown");
            tank1.Attack();
            tank1.Move();

            Console.ReadKey();
        }
        abstract class UnitState
        {
            public abstract void Attack();
            public abstract void Move();
        }
        class NormalState : UnitState
        {
            public override void Attack()
            {
                Console.WriteLine("공격 가능");
            }
            public override void Move()
            {
                Console.WriteLine("이동 가능 시야 10");
            }
        }
        class Blind : UnitState
        {
            public override void Attack()
            {
                Console.WriteLine("공격 가능");
            }
            public override void Move()
            {
                Console.WriteLine("이동 가능 시야 1");
            }
        }
        class LockDown : UnitState
        {
            public override void Attack()
            {
                Console.WriteLine("공격 불가!");
            }
            public override void Move()
            {
                Console.WriteLine("이동 불가!");
            }
        }
        class SiegeTank
        {
            private UnitState unitState;
            public SiegeTank(UnitState _unitState)
            {
                this.unitState = _unitState;
            }
            public void Attacked(string _StateAttack)
            {
                switch (_StateAttack)
                {
                    case "Medic Blind":
                        Console.WriteLine("메딕에게 블라인드 상태이상 공격을 받음");
                        this.unitState = new Blind();
                        break;
                    case "Ghost LockDown":
                        Console.WriteLine("고스트에게 락다운 상태이상 공격을 받음");
                        this.unitState = new LockDown();
                        break;
                    default:
                        break;
                }
            }
            public void Attack()
            {
                unitState.Attack();
            }
            public void Move()
            {
                unitState.Move();
            }
        }
    }
}