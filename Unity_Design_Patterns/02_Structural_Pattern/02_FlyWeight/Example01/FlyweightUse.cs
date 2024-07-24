using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightUse : MonoBehaviour
{
    void Start()
    {
        Cola coke1 = DrinkFactory.getDrink("코카콜라");
        Cola coke2 = DrinkFactory.getDrink("펩시콜라");
        Cola coke3 = DrinkFactory.getDrink("코카콜라");

        Debug.Log(coke1 = coke2); // 둘이 다르다
        Debug.Log(coke1 = coke3); // 둘이 같다
        
        Debug.Log("name : " + coke1.getName()); 
    }
}
