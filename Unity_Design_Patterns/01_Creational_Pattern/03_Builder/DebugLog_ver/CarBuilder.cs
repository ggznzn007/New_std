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
        // ������ ������� �� �ƹ��͵� ���� ���� �ϳ� �����. (��ǰ�� �ٱ� ��)
        _vehicle = new Vehicle(VehicleType.Car);
    }

    public void BuildFrame() // Car�� ������ �����
    {
        _vehicle.AddPart("��ö");
    }

    public void BuildEngine() // Car�� ���� �����
    {
        _vehicle.AddPart("100����");
    }

    public void BuildWheels()  // Car�� ���� �����
    {
        _vehicle.AddPart("��.���� ����");
        _vehicle.AddPart("��.������ ����");
        _vehicle.AddPart("��.���� ����");
        _vehicle.AddPart("��.������ ����");
    }
}