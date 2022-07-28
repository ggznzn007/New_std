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
        ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }



    void DeactiveDelay() => gameObject.SetActive(false);
}
