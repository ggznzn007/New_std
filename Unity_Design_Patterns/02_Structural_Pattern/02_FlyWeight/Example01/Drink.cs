using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Drink { string getName(); }

public class Cola : Drink
{
    string name;
    int price;

    public Cola(string name) { this.name = name; }

    public string getName() // 오버라이딩
    {
        return name;
    }
}