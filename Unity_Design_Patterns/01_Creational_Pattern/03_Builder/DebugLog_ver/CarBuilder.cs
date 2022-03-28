using UnityEngine;

class CarBuilder : IVehicleBuilder
{
    private Vehicle _vehicle;

    public Vehicle getVehicle()
    {
        return _vehicle;
    }

    public CarBuilder()
    {
        // 빌더가 만들어질 때 아무것도 없는 차도 하나 만든다. (부품이 붙기 전)
        _vehicle = new Vehicle(VehicleType.Car);
    }

    public void BuildFrame() // Car의 프레임 만들기
    {
        _vehicle.AddPart("강철");
    }

    public void BuildEngine() // Car의 엔진 만들기
    {
        _vehicle.AddPart("100마력");
    }

    public void BuildWheels()  // Car의 바퀴 만들기
    {
        _vehicle.AddPart("앞.왼쪽 바퀴");
        _vehicle.AddPart("앞.오른쪽 바퀴");
        _vehicle.AddPart("뒤.왼쪽 바퀴");
        _vehicle.AddPart("뒤.오른쪽 바퀴");
    }
}