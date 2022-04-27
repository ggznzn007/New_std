using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salary : MonoBehaviour
{
    private int salary; // 은닉

    private int bonus = 10;

   /* private void SetSalary(int value) // 해당 메소드로 은닉값을 조정
    {
        salary = value;
    }

    public int GetSalary() // 값을 수정불가, 읽기 가능
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

    public int Bonus // 간단한 기능구현의 프로퍼티
    {
        get;set;
    }

    private void Start()
    {
        SalaryP = 10;
    }
}
