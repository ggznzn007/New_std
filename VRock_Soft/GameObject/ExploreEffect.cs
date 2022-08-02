using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreEffect : MonoBehaviour                      // 총알 임팩트 효과
{
   /* private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
       
    }
    private void OnEnable()
    {
        audioSource.Play();                                     // 폭발 소리 재생
        
        Invoke(nameof(DeactiveDelay), 0.5f);
    }
    void OnDisable()
    {        
        ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }

    

    void DeactiveDelay() => gameObject.SetActive(false);*/
}
