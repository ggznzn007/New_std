using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Log : MonoBehaviour
{
    [SerializeField] int age; // private 생략하면 자동으로 프라이빗으로 설정됨

    /*public int GetAge() { return age; }

    public void SetAge(int num) { age = num; }*/

    public int Age 
    { 
        get 
        { 
            return age;
        }

        set
        {
            age = value;
            OnAgeChanged(value);
        } 
    }

    void OnAgeChanged(int _age)
    {
        print($"{_age} 나이 변화");
    }
}
