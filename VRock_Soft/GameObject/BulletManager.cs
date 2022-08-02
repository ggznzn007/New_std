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



public class BulletManager : MonoBehaviour                                        // �Ѿ� ��ũ��Ʈ
{
    public float speed;   
    Transform tr;
    Rigidbody rb;


    public GameObject exploreEffet;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        
    }
    private void OnEnable()
    {
        
        rb.AddForce(transform.forward * speed);
        /*if(GameObject.FindGameObjectWithTag("FirePoint"))
        {
            rb.AddForce(GameObject.FindGameObjectWithTag("FirePoint").transform.position * speed);
        }*/
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

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube"))
        {
            //var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect");
            ShowEffect(collision);
            gameObject.SetActive(false);
          
          
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 
            Debug.Log("�±׵�");
        }
    }


    private void ShowEffect(Collision coll)
    {
        // �浹������ ������ ����
        ContactPoint contact = coll.contacts[0];

        // ���� ��Ÿ�� �̷�� ȸ������ ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ���� ȿ�� ����
        Instantiate(exploreEffet, contact.point, rot);


    }

}

