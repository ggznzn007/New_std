using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MotorCycleBuilder : MonoBehaviour, IVehicleBuilder
{
    public GameObject ParentOfVehicle;
    public GameObject frame;
    public GameObject engine;
    public GameObject wheel1;
    public GameObject wheel2;

    private Vehicle _vehicle;

    public Vehicle getVehicle()
    {
        return _vehicle;
    }

    public void makeVehicle()
    {
        //_vehicle = new Vehicle(VehicleType.MotorCycle);
        GameObject obj = Instantiate(ParentOfVehicle) as GameObject;
        _vehicle = obj.GetComponent<Vehicle>();
        _vehicle.setVehicleType(VehicleType.MotorCycle);
    }

    public void BuildFrame()
    {
        _vehicle.AddPart(frame, Vector3.zero);
    }

    public void BuildEngine()
    {
        _vehicle.AddPart(engine, new Vector3(0, 0.5f, 0));
    }

    public void BuildWheels()
    {
        _vehicle.AddPart(wheel1, new Vector3(1.5f, 0, 0));
        _vehicle.AddPart(wheel2, new Vector3(-1.5f, 0, 0));
    }
}
