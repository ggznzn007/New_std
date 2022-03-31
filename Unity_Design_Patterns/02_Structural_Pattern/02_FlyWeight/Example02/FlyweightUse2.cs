using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightUse2 : MonoBehaviour
{
    void Start()
    {
        CarbonDrinkFactory factory = GetComponent<CarbonDrinkFactory>();

        for (int i = 0; i < 10; i++)
        {
            factory.getCider("칠성사이다" + i);
        }

        GameObject cider1 = factory.getCider("칠성사이다");
        GameObject cider2 = factory.getCider("스프라이트");
        GameObject cider3 = factory.getCider("칠성사이다");

        if(cider1==cider3)
        {
            Debug.Log("name : " + cider1.GetComponent<Cider>().getName());
        }
    }


}
