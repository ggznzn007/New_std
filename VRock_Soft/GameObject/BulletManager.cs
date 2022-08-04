using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR;



public class BulletManager :Poolable  //MonoBehaviour                                   // �Ѿ� ��ũ��Ʈ 
{
    public float speed;
    Transform tr;
    Rigidbody rb;


    public ParticleSystem exploreEffet;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

    }
    private void OnEnable()
    {

        rb.AddForce(transform.forward * speed);
        //rb.AddForce(transform.forward * speed);
        /*if(GameObject.FindGameObjectWithTag("FirePoint"))
        {
            rb.AddForce(GameObject.FindGameObjectWithTag("FirePoint").transform.position * speed);
        }*/
     }

    
    private void OnBecameInvisible()
    {
        // BulletPool.BulletPooling.ReturnBullet();
        // Push();
        Enqueue();
    }
    private void OnDisable()
    {
        tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        rb.Sleep();
        
    }
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    IEnumerator DeactiveBullet()
    {
        yield return new WaitForSeconds(0.02f);
        //BulletPool.BulletPooling.ReturnBullet();
        Enqueue();
    }


    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube"))
        {            
            ShowEffect(collision);
            
            //StartCoroutine(DeactiveBullet());

            Enqueue();

            // BulletPool.BulletPooling.ReturnBullet();
            //StartCoroutine(ExploreEffect(collision));            
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 
            Debug.Log("�ݶ��̴� �±׵�");
        }

       
    }

   

    /* IEnumerator ExploreEffect(Collision coll)
     {
         // �浹������ ������ ����
         ContactPoint contact = coll.contacts[0];

         // ���� ��Ÿ�� �̷�� ȸ������ ����
         Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

         var effect = BulletEffectPool.EffectPooling.GetBulletEffect();
         effect.transform.SetPositionAndRotation(contact.point, rot);
         Invoke(nameof(DeactiveEffect), 1f); 
        yield return null;        
     }*/

    public void ShowEffect(Collision coll)
    {
        // �浹������ ������ ����
        ContactPoint contact = coll.contacts[0];

        // ���� ��Ÿ�� �̷�� ȸ������ ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ���� ȿ�� ����
        Instantiate(exploreEffet, contact.point, rot);
       
    }

}

