using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUse : MonoBehaviour
{
    void Start()
    {
        // Instantiate the director and builders
        Engineer engineer = new Engineer();
        //CarBuilder carBuilder = new CarBuilder();
        CarBuilder carBuilder = GetComponent<CarBuilder>();
        carBuilder.makeVehicle();
        //MotorCycleBuilder motorCycleBuilder = new MotorCycleBuilder();
        MotorCycleBuilder motorCycleBuilder = GetComponent<MotorCycleBuilder>();
        motorCycleBuilder.makeVehicle();

        // 빌더를 통해 구성해야 할 제품을 입력받아 제품을 구성한다.
        engineer.Construct(carBuilder);
        engineer.Construct(motorCycleBuilder);

        // 최종 생산된 제품을 반환받는다.
        Vehicle car = carBuilder.getVehicle();
        Debug.Log(car.GetPartsList());

        Vehicle motorCycle = motorCycleBuilder.getVehicle();
        Debug.Log(motorCycle.GetPartsList());

        // 최종 생산된 제품의 위치를 지정한다.
        car.transform.position = new Vector3(-3f, 0, 0);
        motorCycle.transform.position = new Vector3(3f, 0, 0);
    }
}