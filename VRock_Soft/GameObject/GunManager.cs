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
    public GameObject bullet;
    public float speed;
    public Transform firePoint;  // �ѱ�        
    private ParticleSystem muzzleFlash;    // �ѱ� ����Ʈ
                                           // private PhotonView PV;
    AudioSource audioSource;             // �Ѿ� �߻� �Ҹ�
    public bool isBulletMine;
    /*private Vector3 remotePos;
    private Quaternion remoteRot;
    private float intervalSpeed = 20;*/

    private PhotonView PV;
    public int actorNumber;
    private void Awake()
    {
        gunManager = this;
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ����
                                                                           // firePoint.GetComponent<BulletManager>();
        FindGun();
        //OP.PrePoolInstantiate();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

    }
    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            Debug.Log("�� ���� ����");
        }
        return null;
    }

    public void FireBullet()
    {
        //if (!PV.IsMine) { return; }
        if (PV.IsMine)
        {
            audioSource.Play();
            muzzleFlash.Play();

            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            _bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // �Ѿ� ��ġ
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            _bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            _bullet.GetPhotonView().ControllerActorNr = actorNumber;            
            
            _bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            _bullet.SetActive(true);
            Debug.Log("Bullet �߻�");
            PV.RPC("PunFire", RpcTarget.Others);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped))// && !SpawnWeapon_R.rightWeapon.weaponInIt)
            {
                if (!griped)
                {
                    if (PV.IsMine)
                    {
                        PV.RPC("DestroyGun_Delay", RpcTarget.All);
                        //DestroyGun_Delay();
                        //PN.Destroy(this.gameObject);
                        SpawnWeapon_L.leftWeapon.weaponInIt = false;
                        SpawnWeapon_R.rightWeapon.weaponInIt = false;
                        //Debug.Log("�� �ı���");
                    }

                }
            }

            Debug.Log("�ٴڿ� �±׵�");

        }
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
    public void PunFire()
    {
        audioSource.Play();
        muzzleFlash.Play();

        // _bullet.SetActive(true);

        //_bullet.GetComponent<BulletManager>().actorNumber = actNumber;                           // �Ѿ� ����� ����� ��ȣ

        /* if (_bullet != null)
         {
             //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

         }*/
    }

    [PunRPC]
    public void DestroyGun_Delay()                  // �� �ı� �ð� ������ �޼���
    {
        StartCoroutine(DestoryPN_Gun());

    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
        //Debug.Log("����������Ǯ�ı�");
        // Destroy(gameObject);
    }


}

