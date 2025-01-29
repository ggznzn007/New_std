using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cider : MonoBehaviour, CarbonatedDrink
{
    public string name;
    public int price;

    public void setName(string name) { this.name = name; }

    public string getName() { return name; }
}
