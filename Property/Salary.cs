using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salary : MonoBehaviour
{
    private int salary; // ����

    private int bonus = 10;

   /* private void SetSalary(int value) // �ش� �޼ҵ�� ���а��� ����
    {
        salary = value;
    }

    public int GetSalary() // ���� �����Ұ�, �б� ����
    {
        return salary;
    }
*/

    public int SalaryP 
    { 
        get 
        { 
            return salary + bonus;
        }

      private  set
        {
            if (value < 0)
                salary = 10;
            else
                salary = value;
        }
    }

    public int Bonus // ������ ��ɱ����� ������Ƽ
    {
        get;set;
    }

    private void Start()
    {
        SalaryP = 10;
    }
}
