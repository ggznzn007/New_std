using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IVehicleBuilder
{
    Vehicle getVehicle();

    void BuildFrame(); // 프레임 만들기
    void BuildEngine(); // 엔진 만들기
    void BuildWheels(); // 바퀴 만들기
}

public enum VehicleType
{
    Car,
    MotorCycle
}
