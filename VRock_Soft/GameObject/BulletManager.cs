using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletManager : MonoBehaviour                                        // 총알 스크립트
{
    [SerializeField] GameObject bullet;  // 총알 프리팹

    AudioSource audioSource;             // 총알 발사 소리
    public float speed = 40f;            // 총알 속도
    public int tagCount = 0;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        audioSource.Play();                                                                     // 총알 발사 소리 재생
        rb.velocity = GameObject.FindGameObjectWithTag("FirePoint").transform.forward * speed;
        

        Invoke(nameof(DeactiveDelay), 1);
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 터지는 이펙트 보여지고
        if (collision.gameObject.CompareTag("Cube"))
        {
            var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect", this.transform.position);
            DeactiveDelay();

            
           
            Debug.Log("태그됨");
        }
    }


    void DeactiveDelay() => gameObject.SetActive(false);

   
}

