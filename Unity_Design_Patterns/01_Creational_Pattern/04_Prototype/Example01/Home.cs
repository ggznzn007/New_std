using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IPrototype interface
public interface IHome
{
    // method for cloning
    IHome Clone();
}

// 'ConcretePrototype1' class implements IPrototype interface
public class LivingRoom : IHome
{
    public int Couch { get; set; }
    public int Light { get; set; }

    // Implement shallow cloning method
    public IHome Clone()
    {
        // Shallow Copy
        return this.MemberwiseClone() as IHome;

        // Deep Copy
        // Implement Memberwise clone method for every reference type object 
        // return ..
    }
}

public class InnerRoom : IHome
{
    public int Bed { get; set; }
    public int Light { get; set; }

    // Implement shallow cloning method
    public IHome Clone()
    {
        // Shallow Copy
        return this.MemberwiseClone() as IHome;        
    }
}


