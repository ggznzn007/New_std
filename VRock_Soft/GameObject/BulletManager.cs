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


public class BulletManager : MonoBehaviourPunCallbacks//, IPunObservable //MonoBehaviourPun   //MonoBehaviour     Poolable                           // �Ѿ� ��ũ��Ʈ 
{
    public static BulletManager BM;
    public PhotonView PV;
    public float speed;     
    public Rigidbody rb;
    public ParticleSystem exploreEffect;    
    public Transform firePoint;
    public int actNumber = -1;
    //Transform tr;
    private void Start()
    {
        BM = this;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        
        //tr = GetComponent<Transform>();
        Destroy(gameObject, 1.2f);
      //GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        
    }
   
    private void Update()
    {
        //GunManager.gunManager.FindGun();
        //if (!PV.IsMine) return;

        //transform.Translate(Vector3.forward * speed * Time.deltaTime );
        //transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed ,ForceMode.Force);
        //GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        //transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
        // GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
    }

    /*private void OnTriggerEnter(Collider collider)
    {
        //if (!PV.IsMine) return;
        if (!PV.IsMine && collider.tag == "Player" && collider.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("�÷��̾� ����");           

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, transform.position,transform.rotation);

            Destroy(effect, 0.5f);

            collider.GetComponent<PlayerNetworkSetup>().HitPlayer();

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Destroy(effect, 0.5f);


        }
        // ������ ����Ʈ ��������
        if (collider.tag == "Cube")
        {
            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, transform.position, transform.rotation);

            Destroy(effect, 0.5f);

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

            Debug.Log("��ǥ���� ����");

        }
    }*/
    private void OnCollisionEnter(Collision collsion)
    {              
        if(!AvartarController.ATC.isAlive)
        {
            return;
        }
       
        // ������ ����Ʈ ��������
        if (collsion.collider.CompareTag("Cube") && PV.IsMine)
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);

            transform.position = contact.point;

            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Enqueue(); // ť ��� �Ѿ� Ǯ�� => ����� �Ѿ��� �ٽ� ť�� �ֱ�
            //OP.PoolDestroy(this.gameObject);
            // BulletPool.BulletPooling.ReturnBullet(); // ���� Ǯ��
            // ShowEffect(collision); // ����Ʈ �޼��� 
            //StartCoroutine(ExploreEffect(collision));  // ����Ʈ �ڷ�ƾ         
            //StartCoroutine(DeactiveBullet()); // �Ѿ� ��Ȱ��ȭ �ڷ�ƾ
            //Destroy(collision.gameObject); // �Ѿ� ���� ������Ʈ�� ����� 

           // Debug.Log("��ǥ���� ����");

        }

        if(collsion.collider.CompareTag("Player"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
            //Debug.Log("�÷��̾� ����");
        }
        
        
        if (collsion.collider.CompareTag("Next"))
        {
            // �浹������ ������ ����
            ContactPoint contact = collsion.contacts[0];

            // ���� ��Ÿ�� �̷�� ȸ������ ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);


            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, rot);

            Destroy(effect, 0.5f);
            PV.RPC("DestroyBullet", RpcTarget.AllBuffered);

            PN.Disconnect();
            //Debug.Log("�÷��̾� ����");
        }




    }


    [PunRPC]
   public void BulletDir(float speed)//,int addSpeed)
    {
        this.speed = speed;
       // this.addSpeed = addSpeed;
    }

    [PunRPC]
    public void DestroyBullet()
    {
        Destroy(gameObject);
        Debug.Log("�Ѿ� �ı�");
    }

    /*[PunRPC]
    void DestoroyEffect() =>Destroy(exploreEffect);*/


    /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
      {
          if (stream.IsWriting)
          {
              stream.SendNext(transform.position);
              stream.SendNext(transform.rotation);
              stream.SendNext(rb.position);
              stream.SendNext(rb.rotation);
              // stream.SendNext(transform.localPosition);
              // stream.SendNext(transform.localRotation);
          }
          else
          {

              transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
              rb.position = (Vector3)stream.ReceiveNext();
              rb.rotation = (Quaternion)stream.ReceiveNext();
              //this.transform.localPosition = (Vector3)stream.ReceiveNext();
              // this.transform.localRotation = (Quaternion)stream.ReceiveNext();
          }
      }
  */
    /* private void OnEnable()  //Ǯ���Ҷ� 
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
          if (GetComponent<PhotonView>().IsMine)
          {
              PN.Destroy(gameObject);
          }
          else
          {
              Destroy(this.gameObject);
          }
      }

      private void OnDisable()
      {
          //tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
          rb.Sleep();
      }*/
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

     Collision collision = new Collision();  // �浹���� ���� �޼���
            // �浹 ���� ����
            var contact = collision.GetContact(0);

            // �浹 ������ ����Ʈ ����           
            var effect = Instantiate(exploreEffect, contact.point, Quaternion.LookRotation(-contact.normal));

            Destroy(effect, 0.5f);

    }*/
    /*[PunRPC]
    public void SetActiveRPC(bool bull)
    {
        gameObject.SetActive(bull);
    }*/
}

