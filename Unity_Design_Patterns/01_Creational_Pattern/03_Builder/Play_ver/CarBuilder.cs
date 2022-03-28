using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CarBuilder : MonoBehaviour, IVehicleBuilder
{
    public GameObject ParentOfVehicle;
    public GameObject frame;
    public GameObject engine;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;

    private Vehicle _vehicle;

    public Vehicle getVehicle()
    {
        return _vehicle;
    }

    public void makeVehicle()
    {
        //_vehicle = new Vehicle(VehicleType.Car);
        GameObject obj = Instantiate(ParentOfVehicle) as GameObject;
        _vehicle = obj.GetComponent<Vehicle>();
        _vehicle.setVehicleType(VehicleType.Car);
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
        _vehicle.AddPart(wheel1, new Vector3(0.75f, -0.5f, 0.5f));
        _vehicle.AddPart(wheel2, new Vector3(-0.75f, -0.5f, 0.5f));
        _vehicle.AddPart(wheel3, new Vector3(-0.75f, -0.5f, -0.5f));
        _vehicle.AddPart(wheel4, new Vector3(0.75f, -0.5f, -0.5f));
    }
}
