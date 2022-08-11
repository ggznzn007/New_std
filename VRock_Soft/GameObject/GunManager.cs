using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using static ObjectPooler;
public class GunManager : MonoBehaviourPun, IPunObservable  // ���� �����ϴ� ��ũ��Ʈ
{
    public static GunManager gunManager;
    //public GameObject bullet;
    public float speed;
    public Transform firePoint;  // �ѱ�        
    private ParticleSystem muzzleFlash;    // �ѱ� ����Ʈ
                                           // private PhotonView PV;
    AudioSource audioSource;             // �Ѿ� �߻� �Ҹ�
    /*private Vector3 remotePos;
    private Quaternion remoteRot;
    private float intervalSpeed = 20;*/
    
    private PhotonView PV;
    public int actorNumber;
    private void Awake()
    {
        gunManager = this;
        //OP.PrePoolInstantiate();
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ����
       
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        //firePoint = FindGun().firePoint ;
    }
    /*public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            Debug.Log("�� ���� ����");
        }
        return null;
    }*/

    public void FireBullet()
    {
        //if (!photonView.IsMine) return;
        if (PV.IsMine)
        {
            //PunFire(PV.Owner.ActorNumber);
            PV.RPC("PunFire", RpcTarget.All, PV.Owner.ActorNumber);
        }




        /* audioSource.Play();// �Ѿ� �߻� �Ҹ� ���
         muzzleFlash.Play();

         var _bullet = PoolManager.PoolingManager.pool.Dequeue();
         if (_bullet != null)
         {
             _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
             _bullet.SetActive(true);
         }*/



        /* var _bullet = BulletPool.BulletPooling.GetBullet();  //1
         if (_bullet != null)
         {
             _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
             _bullet.SetActive(true);
         }*/




        //var bullet = Instantiate(bulletPrefab);
        /* var bullet = ObjectPooler.SpawnFromPool<BulletManager>("Bullet");
             bullet.transform.position= firePoint.position;*/
        // bullet.transform.Translate(firePoint.forward * speed);
        // bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.transform.TransformDirection(new Vector3(0,0,speed));
        //bullet.GetComponentInChildren<Rigidbody>().velocity = firePoint.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
            {
                if (!griped)
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("DestroyGun_Delay", RpcTarget.AllViaServer);
                        //DestroyGun_Delay();
                        //PN.Destroy(this.gameObject);
                        SpawnWeapon_L.leftWeapon.weaponInIt = false;
                        SpawnWeapon_R.rightWeapon.weaponInIt = false;
                        Debug.Log("�� �ı���");
                    }

                }
            }

            Debug.Log("�ٴڿ� �±׵�");

        }
    }

    [PunRPC]
    public void DestroyGun_Delay()
    {
        StartCoroutine(DestoryPN_Gun());
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.1f);
        PN.Destroy(gameObject);
        // Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(firePoint.position);
            stream.SendNext(firePoint.rotation);
        }
        else
        {
            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            firePoint.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

    [PunRPC]
    public void PunFire(int actNumber)
    {
        audioSource.Play();// �Ѿ� �߻� �Ҹ� ���
        muzzleFlash.Play();

        //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
        GameObject _bullet = OP.PoolInstantiate("Bullet");
            _bullet.GetComponent<BulletManager>().actorNumber = actNumber;                           // �Ѿ� ����� ����� ��ȣ
            _bullet.GetComponent<Rigidbody>().AddRelativeForce(this.firePoint.forward * speed);      // �Ѿ� ���� �ӵ�
            _bullet.transform.SetPositionAndRotation(this.firePoint.position, firePoint.rotation);   // �Ѿ� ��ġ
         //   _bullet.SetActive(true);

       /* if (_bullet != null)
        {
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            Debug.Log("RPC_Bullet �߻�");
        }*/
    }



}

