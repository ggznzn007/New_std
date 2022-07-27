using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour                  // 총구 효과 
{    
    private void OnEnable()
    {
        Invoke(nameof(DeactiveDelay), 0.5f);       
    }
    void OnDisable()
    {       
        ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }

    void DeactiveDelay() => gameObject.SetActive(false);
}
