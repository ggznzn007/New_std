using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Our final product
class Vehicle
{
    public VehicleType vehicleType;
    private List<string> _parts = new List<string>();

    // Constructor method
    public Vehicle(VehicleType vehicleType)
    {
        this.vehicleType = vehicleType;
    }

    public void AddPart(string desc)
    {
        _parts.Add(desc);
    }

    public string GetPartsList()
    {
        string partsList = vehicleType.ToString() + " parts:\n\t";
        foreach (string part in _parts)
        {
            partsList += part + " ";
        }

        return partsList;
    }
}
