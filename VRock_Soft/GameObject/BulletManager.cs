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



public class BulletManager : MonoBehaviour                                        // 총알 스크립트
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
        // 터지는 이펙트 보여지고
        if (collision.collider.CompareTag("Cube"))
        {
            //var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect");
            ShowEffect(collision);
            gameObject.SetActive(false);
          
          
            //Destroy(collision.gameObject); // 총알 맞은 오브젝트가 사라짐 
            Debug.Log("태그됨");
        }
    }


    private void ShowEffect(Collision coll)
    {
        // 충돌지점의 정보를 추출
        ContactPoint contact = coll.contacts[0];

        // 법선 벡타가 이루는 회전각도 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // 폭발 효과 생성
        Instantiate(exploreEffet, contact.point, rot);


    }

}

