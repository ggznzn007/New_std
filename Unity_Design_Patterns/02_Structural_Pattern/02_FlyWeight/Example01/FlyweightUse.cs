using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightUse : MonoBehaviour
{
    void Start()
    {
        Cola coke1 = DrinkFactory.getDrink("��ī�ݶ�");
        Cola coke2 = DrinkFactory.getDrink("����ݶ�");
        Cola coke3 = DrinkFactory.getDrink("��ī�ݶ�");

        Debug.Log(coke1 = coke2); // ���� �ٸ���
        Debug.Log(coke1 = coke3); // ���� ����
        
        Debug.Log("name : " + coke1.getName()); 
    }
}
