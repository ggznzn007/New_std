using UnityEngine;

// 우리 회사에서 PayY 인터페이스 구현
public class PayImpl : PayY, PayX // 다중 상속  = 어댑터가 됨
{
    // for PayY
    private string customerCardNum;

    public string getCustomerCardNum()
    {
        Debug.Log("PayY (Get)");
        return customerCardNum;
    }

    public void setCustomerCardNum(string customerCardNum)
    {
        Debug.Log("PayY (Set)");
        this.customerCardNum = customerCardNum;
    }
    // ------------------------------------------------------

    // for PayX Method
    public string getCreditCardNum() { return getCustomerCardNum(); }

    public void setCreditCardNum(string creditCardNum) { setCustomerCardNum(creditCardNum); }
}