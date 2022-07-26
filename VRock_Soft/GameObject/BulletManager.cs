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
        // ������ ��� ����
        //transform.position += transform.forward * speed*Time.deltaTime;

    }


    private void OnTriggrtEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        ParticleSystem explo = Instantiate(exploreEffect);
        explo.transform.position = transform.position;
        //Destroy(explo, 2);
        // ������ �Ҹ� ���
        audioSource.Play();

        // �浹�ϸ� ��������

    }


}

