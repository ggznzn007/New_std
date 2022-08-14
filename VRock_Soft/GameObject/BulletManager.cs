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


public class BulletManager : Poolable, IPunObservable //MonoBehaviourPun   //MonoBehaviour     Poolable                           // �Ѿ� ��ũ��Ʈ 
{
    public float speed;
    Transform tr;
    Rigidbody rb;
    public ParticleSystem exploreEffet;
    // public int actorNumber;
    private PhotonView PV;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        //OP.PrePoolInstantiate();
        // GetComponent<Rigidbody>().AddRelativeForce(GunManager.gunManager.firePoint.forward * speed);        
      //GetComponent<Rigidbody>().AddRelativeForce(transform.forward * speed);        
    }

    private void OnEnable()  //Ǯ���Ҷ� 
    {
        // rb.AddRelativeForce(GunManager.gunManager.firePoint.forward * speed);
        rb.AddForce(transform.forward * speed);

    }

    private void OnBecameInvisible()
    {
        //Enqueue();
        // OP.PoolDestroy(this.gameObject);

        //Destroy(gameObject);
        //PN.Destroy(photonView);
       /* if (GetComponent<PhotonView>().IsMine)
        {
            PN.Destroy(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }*/
    }

    private void OnDisable()
    {
        //tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        rb.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.collider.CompareTag("Cube"))
        {
            // �浹 ���� ����
            var contact = collision.GetContact(0);

            // �浹 ������ ����Ʈ ����
            var effect = Instantiate(exploreEffet, contact.point, Quaternion.LookRotation(-contact.normal));

            Destroy(effect, 0.8f);

           // PV.RPC("LocalDestruction", RpcTarget.All);

            //PN.Destroy(photonView);
            Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            //Destroy(gameObject);
           

            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 
            Debug.Log("��ǥ���� ����");
            Debug.Log("�Ѿ� �ı�");

        }


    }
    private void Update()
    {
        if (!PV.IsMine) return;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            // stream.SendNext(transform.localPosition);
            // stream.SendNext(transform.localRotation);
        }
        else
        {

            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //this.transform.localPosition = (Vector3)stream.ReceiveNext();
            // this.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    /*[PunRPC]
    public void LocalDestruction() => PN.Destroy(gameObject.GetPhotonView());*/
    /*
        [PunRPC]
        void DestroyBullet()
        {
            PN.Destroy(photonView.gameObject);
        }*/

    /*public void ShowEffect(Collision collision)   // ����Ʈ �޼��� ����
    {
        // �浹������ ������ ����
        ContactPoint contact = collision.contacts[0];

        // ���� ��Ÿ�� �̷�� ȸ������ ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ���� ȿ�� ����
        Instantiate(exploreEffet, contact.point, rot);
    }*/
    /*[PunRPC]
    public void SetActiveRPC(bool bull)
    {
        gameObject.SetActive(bull);
    }*/
}

