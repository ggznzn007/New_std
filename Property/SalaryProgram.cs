using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaryProgram : MonoBehaviour
{
    Salary mySalary = new Salary();

    private void Start()
    {
        print(mySalary.SalaryP);
    }
}
