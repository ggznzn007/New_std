using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPay : MonoBehaviour
{
    void Start()
    {
        // for PayX : ���� �̷��� ����� ...

        PayImpl myPay = new PayImpl();
        myPay.setCreditCardNum("12345");
        string myCardNum = myPay.getCreditCardNum();
        // Debug.Log("PayX : " + myCardNum);

        // for PayY : PayX �� ���� �޼������ ���
        //            �׷��Ƿ� �ڵ带 �ٲ� �ʿ䰡 ����.
        Debug.Log("PayY : " + myCardNum);
    }
}
