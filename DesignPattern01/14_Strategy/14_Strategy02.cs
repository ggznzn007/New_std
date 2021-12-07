using System;

namespace Strategy
{
    class Program
    {
        static void Main(string[] args)
        {
            //마린
            Unit unit = new Marine(new MoveLand(), new Attack());
            unit.Move();
            unit.Attack();
            //메딕
            unit = new Medic(new MoveLand(), new NoAttack());
            unit.Move();
            unit.Attack();
            //레이쓰
            unit = new Wrath(new MoveSky(), new Attack());
            unit.Move();
            unit.Attack();
            //메딕에게 스페샬!공격 기능으로 변경!!
            unit = new Medic(new MoveLand(), new SpecialAttack());
            unit.Move();
            unit.Attack();

            Console.ReadKey();
        }
        abstract class Moveable
        {
            public abstract void Move();
        }
        class MoveLand : Moveable
        {
            public override void Move()
            {
                Console.WriteLine(
                  "Move Land");
            }
        }
        class MoveSky : Moveable
        {
            public override void Move()
            {
                Console.WriteLine(
                  "Move Sky");
            }
        }
        abstract class Attackable
        {
            public abstract void AttackEnemy();
        }
        class Attack : Attackable
        {
            public override void AttackEnemy()
            {
                Console.WriteLine(
                  "Attack Enemy!");
            }
        }
        class NoAttack : Attackable
        {
            public override void AttackEnemy()
            {
                Console.WriteLine(
                  "Can not Attack");
            }
        }
        /// <summary>
        /// 메딕에게 신규 공격기술 추가
        /// </summary>
        class SpecialAttack : Attackable
        {
            public override void AttackEnemy()
            {
                Console.WriteLine(
                  "Medic can Special Attack!!");
            }
        }
        class Unit
        {
            private Moveable moveAble;
            private Attackable attackAble;

            // Constructor
            public Unit(Moveable moveable, Attackable attackable)
            {
                this.moveAble = moveable;
                this.attackAble = attackable;
            }
            public void Attack()
            {
                attackAble.AttackEnemy();
            }
            public void Move()
            {
                moveAble.Move();
            }
            public Moveable MoveAble
            {
                set { this.moveAble = value; }
            }
        }
        class Marine : Unit
        {
            public Marine(Moveable moveable, Attackable attackable)
                : base(moveable, attackable)
            {
            }
        }
        class Medic : Unit
        {
            public Medic(Moveable moveable, Attackable attackable)
                : base(moveable, attackable)
            {
            }
        }
        class Wrath : Unit
        {
            public Wrath(Moveable moveable, Attackable attackable)
                : base(moveable, attackable)
            {
            }
        }
    }
}