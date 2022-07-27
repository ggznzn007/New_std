using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour                  // �ѱ� ȿ�� 
{    
    private void OnEnable()
    {
        Invoke(nameof(DeactiveDelay), 0.5f);       
    }
    void OnDisable()
    {       
        ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ���
        CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� 
    }

    void DeactiveDelay() => gameObject.SetActive(false);
}
