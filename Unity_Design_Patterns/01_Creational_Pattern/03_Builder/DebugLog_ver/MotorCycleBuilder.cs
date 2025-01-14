using UnityEngine;

class MotorCycleBuilder : IVehicleBuilder
{
    private Vehicle _vehicle;

    public Vehicle getVehicle() { return _vehicle; }

    public MotorCycleBuilder()
    {
        // ������ ������� �� �ƹ��͵� ���� ���ͽ���Ŭ�� �ϳ� �����.(��ǰ�� �ٱ� ��)
        _vehicle = new Vehicle(VehicleType.MotorCycle);
    }

    public void BuildFrame()  // MotorCycle�� ������ ����� 
    {
        _vehicle.AddPart("�˷�̴�");
    }

    public void BuildEngine() // MotorCycle�� ���� ����� 
    {
        _vehicle.AddPart("50����");
    }

    public void BuildWheels() // MotorCycle�� ���� ����� 
    {
        _vehicle.AddPart("�� ����");
        _vehicle.AddPart("�� ����");
    }
}