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
            factory.getCider("ĥ�����̴�" + i);
        }

        GameObject cider1 = factory.getCider("ĥ�����̴�");
        GameObject cider2 = factory.getCider("��������Ʈ");
        GameObject cider3 = factory.getCider("ĥ�����̴�");

        if(cider1==cider3)
        {
            Debug.Log("name : " + cider1.GetComponent<Cider>().getName());
        }
    }


}
