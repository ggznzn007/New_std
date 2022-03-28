using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Engineer
{
    public void Construct(IVehicleBuilder vehicleBuilder)
    {
        vehicleBuilder.BuildFrame();
        vehicleBuilder.BuildEngine();
        vehicleBuilder.BuildWheels();
    }
}