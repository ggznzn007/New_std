using UnityEngine;

// �츮 ȸ�翡�� PayY �������̽� ����
public class PayImpl : PayY, PayX // ���� ���  = ����Ͱ� ��
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