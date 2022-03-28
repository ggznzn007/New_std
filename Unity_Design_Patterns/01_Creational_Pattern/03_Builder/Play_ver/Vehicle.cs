using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Our final product
public class Vehicle : MonoBehaviour
{
    private VehicleType vehicleType;

    public void setVehicleType(VehicleType vehicleType)
    {
        this.vehicleType = vehicleType;
    }

    public void AddPart(GameObject part, Vector3 localPosition)
    {
        GameObject obj = Instantiate(part) as GameObject;
        obj.transform.localPosition = localPosition;
        obj.transform.SetParent(this.transform);
    }

    public string GetPartsList()
    {
        string partsList = vehicleType.ToString() + " parts:\n\t";

        Transform[] trs = GetComponentsInChildren<Transform>();
        for (int i = 1; i < trs.Length; i++)
        {
            partsList += trs[i].gameObject.name + " ";
        }

        return partsList;
    }
}
