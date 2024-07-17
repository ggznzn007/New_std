using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUse : MonoBehaviour
{
    Factory factory = null;
    GameObject unit1 = null;
    GameObject unit2 = null;
    

    public void Start()
    {
        factory = GetComponent<Factory>();

        unit1 = factory.CreateUnit(UnitType.Marine);
        unit2 = factory.CreateUnit(UnitType.Firebat);        

        StartCoroutine("UnitAction");
    }

   public IEnumerator UnitAction()
    {
        yield return new WaitForSeconds(0.2f);

        // 추상클래스 Unit을 이용하여 Marine, Firebat의 구분없이
        // 접근하여 사용할 수 있다.
        unit1.GetComponent<Unit>().Move();
        unit2.GetComponent<Unit>().Move();
    }
}
