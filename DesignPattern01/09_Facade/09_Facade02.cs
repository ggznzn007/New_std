/*1.Facade Pattern 정의
  - 어떤 서브시스템의 일련의 인터페이스에 대한 통합된 인터페이스를 제공
  - 퍼사드에서 고수준 인터페이스를 정의하기 때문에 서브시스템을 더 쉽게 가능
  - 복잡한 내부처리 내용을 일괄적으로 처리, 외부에는 간단한 인터페이스만 노출됨
2.사용용도 및 장점
 - 하나의 시스템에서 하나의 작업을 처리하는데 여러개의 클래스의 상호 동작이 필요할때,
   이것을 묶어서 처리하며 외부에는 간단한 인터페이스로 노출
 - 컴퓨터 시스템을 예를들면, 컴퓨터를 켜면 컴퓨터에서는 자체적으로 CPU를 가동/바이오스 로드/램초기화/OS시동
   등을 일련의 작업을 하게된다. 사용자는 단순히 컴퓨터의 본체의 POWER만 켰을뿐인데 내부적으로는 여러가지의
   일들이 정해진 순서로 일어나게 된다. 또한 파워를 끌때도 일련의 종료작업 및 하드웨어 종료 작업이 순차적으로 진행
   되는데 이때도 사용자는 단지 컴퓨터 종료만 하면 모든게 자동으로 처리된다
 - 사용자는 복잡한 내부절차를 알지 못해도 단순한 인퍼페이스로 시스템을 사용이 가능
 3.스타크래프트에서는 유닛생성에 관련된 예
 - 테란진영을 선택한 게이머가 탱크를 생성할때 우선 미네랄이 탱크를 생성하기에 충분이 있는지 확인을 하며,
   그후에 가스가 충분한지를 확인한다. 그 이후에 밥집(서플라이디폿..유닛생성갯수를 늘려주는 건물)이 부족한지
   확인후에 유닛을 생성하게 된다. 이때 이런 일련의 작업들 UnitCreatable이라는 클래스에서 외부로 노출되는 
   Createable()이란 메소드를 노출시켜서 3가지의 작업의 결과를 한번에 처리하게 된다.
 - 유닛을 생성할때마다 꼭하는 작업이기 때문에 그때마다 이런 작업들을 각각 3개를 호출에서 처리하는 것 보다
   하나로 묶어서 처리되는 것이 효율적이다. 그리고 나중이라도 유닛생성에 관련된 기능이 업데이트 된다 하더라도
   UnitCreateable에서만 수정이 되면 다른곳에서는 작업이 필요없게 된다.*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade
{
    class Program    {

        static void Main(string[] args)
        {
            UnitCreateable unitCreateable = new UnitCreateable();
            if (unitCreateable.isUnitCreateable(50, 30, 2).Equals(true))
                Console.WriteLine("유닛 생성 가능!");
            else
                Console.WriteLine("유닛 생성 불가능!");

            if (unitCreateable.isUnitCreateable(50, 130, 2).Equals(true))
                Console.WriteLine("유닛 생성 가능!");
            else
                Console.WriteLine("유닛 생성 불가능!");

            if (unitCreateable.isUnitCreateable(50, 30, 5).Equals(true))
                Console.WriteLine("유닛 생성 가능!");
            else
                Console.WriteLine("유닛 생성 불가능!");

            Console.ReadKey();

        }

        public class UnitCreateable
        {
            Mineral mineral = new Mineral();
            Gas gas = new Gas();
            UnitLimit unitLimit = new UnitLimit();
            public bool isUnitCreateable(int unitMineralCost, int unitGasCost, int unitLimitCost)
            {
                if (!mineral.isEnoughMineal(unitMineralCost))
                {
                    return false;
                }
                else if (!gas.isEnoughGas(unitGasCost))
                {
                    return false;
                }
                else if (!unitLimit.isEnoughLimit(unitLimitCost))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        class Mineral
        {
            private int mineral = 100;
            public bool isEnoughMineal(int unitMineralCost)
            {
                if (unitMineralCost <= mineral)
                    return true;
                else
                    return false;
            }
        }

        class Gas
        {
            private int gas = 100;
            public bool isEnoughGas(int unitGasCost)
            {
                if (unitGasCost <= gas)
                    return true;
                else
                    return false;
            }
        }
        class UnitLimit
        {
            private int limit = 3;
            public bool isEnoughLimit(int unitLimitCost)
            {
                if (unitLimitCost <= limit)
                    return true;
                else
                    return false;
            }
        }
    }
}