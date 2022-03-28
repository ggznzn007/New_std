using UnityEngine;

public class BuilderUse : MonoBehaviour
{
    void Start()
    {
        // Instantiate the director and builders
        Engineer engineer = new Engineer();  // ⭐ 엔지니어 만들기
        CarBuilder carBuilder = new CarBuilder(); // 빈 Car 만들기
        MotorCycleBuilder motorCycleBuilder = new MotorCycleBuilder();  // 빈 MotorCycle 만들기

        // 빌더를 통해 구성해야 할 제품을 입력받아 제품을 구성한다.
        // 부품들을 붙인다.
        engineer.Construct(carBuilder);
        engineer.Construct(motorCycleBuilder);

        // 최종 생산된 완성된 제품을 반환받는다.
        Vehicle car = carBuilder.getVehicle();
        Debug.Log(car.GetPartsList());
        Vehicle motorCycle = motorCycleBuilder.getVehicle();
        Debug.Log(motorCycle.GetPartsList());
    }

}
