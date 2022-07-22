using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletManager : MonoBehaviour
{

    [SerializeField] ParticleSystem exploreEffect;
    AudioSource audioSource;
    public float speed = 30f;

    private void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;


    }

    private void Update()
    {
        // 앞으로 계속 나감
        //transform.position += transform.forward * speed*Time.deltaTime;

    }


    private void OnTriggrtEnter(Collision collision)
    {
        // 터지는 이펙트 보여지고
        ParticleSystem explo = Instantiate(exploreEffect);
        explo.transform.position = transform.position;
        //Destroy(explo, 2);
        // 터지는 소리 재생
        audioSource.Play();

        // 충돌하면 없어지게

    }


}

