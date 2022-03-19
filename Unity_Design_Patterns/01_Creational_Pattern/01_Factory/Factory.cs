using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Marine,
    Firebat
}

public class Factory : MonoBehaviour
{
    public GameObject marine = null;
    public GameObject firebat = null;

    public GameObject CreateUnit(UnitType type)
    {
        //Unit unit = null;
        GameObject unit = null;

        float x = (float)Random.Range(0, 6);
        float z = (float)Random.Range(0, 6);

        switch (type)
        {
            case UnitType.Marine:
                //unit = new Marine();
                unit = Instantiate(marine, new Vector3(x, 1.0f, z), Quaternion.identity);
                break;
            case UnitType.Firebat:
                //unit = new Firebat();
                unit = Instantiate(firebat, new Vector3(x, 0.5f, z), Quaternion.identity);
                break;
        }

        return unit;
    }
}