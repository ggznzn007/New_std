using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPay : MonoBehaviour
{
    void Start()
    {
        // for PayX : 원래 이렇게 사용중 ...

        PayImpl myPay = new PayImpl();
        myPay.setCreditCardNum("12345");
        string myCardNum = myPay.getCreditCardNum();
        // Debug.Log("PayX : " + myCardNum);

        // for PayY : PayX 와 같은 메서드명을 사용
        //            그러므로 코드를 바꿀 필요가 없다.
        Debug.Log("PayY : " + myCardNum);
    }
}
