using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private void OnEnable()
    {
       

    }
    void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ���
        CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� 
    }



    void DeactiveDelay() => gameObject.SetActive(false);
}
