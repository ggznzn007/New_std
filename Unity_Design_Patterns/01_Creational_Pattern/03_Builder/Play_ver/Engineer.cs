using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Our 'Director' class.
class Engineer
{
    public void Construct(IVehicleBuilder vehicleBuilder)
    {
        vehicleBuilder.BuildFrame();
        vehicleBuilder.BuildEngine();
        vehicleBuilder.BuildWheels();
    }
}