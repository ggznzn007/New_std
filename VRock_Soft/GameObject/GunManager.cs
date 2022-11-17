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

public class GunManager : MonoBehaviourPun//, IPunObservable  // ���� �����ϴ� ��ũ��Ʈ
{
    public static GunManager gunManager;
    [Header("�Ѿ� ������")][SerializeField] GameObject bullet;

    [Header("�ѱ� ��ġ")][SerializeField] Transform firePoint;

    [Header("�Ѿ� �ӵ�")][SerializeField] float speed;

    [Header("�Ѿ� ������")][SerializeField] bool isBulletMine;

    [Header("���ͳѹ�")][SerializeField] int actorNumber;

    private RaycastHit hit;                          // ����ĳ��Ʈ���� ��Ʈ
    private Ray ray;                                 // ����ĳ��Ʈ ����
    private ParticleSystem muzzleFlash;              // �ѱ� ����Ʈ                                           
    private AudioSource audioSource;                 // �Ѿ� �߻� �Ҹ�
    private PhotonView PV;                           // �����
    private GameObject myBull;                       // �ڱ� �Ѿ�    
    private float fireTime = 0;                      // �Ѿ� ������ Ÿ�� 
    private readonly float delayfireTime = 0.15f;    // �Ѿ� ������ ���ѽð�
    private readonly float fireDistance = 1000f;     // �Ѿ� ��Ÿ�
    private Vector3 remotePos;
    private Quaternion remoteRot;

    private void Awake()
    {
        gunManager = this;
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ���� 
        actorNumber = PV.OwnerActorNr;
    }

    private void FixedUpdate()
    {
       /* if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 20 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 20 * Time.deltaTime));
        }*/
        GetTarget();
        Reload();
        WhenDead();
    }

    public void WhenDead()
    {
        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC(nameof(DestroyGun), RpcTarget.AllViaServer);
        }
    }



    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider == null) return;
        if (collision.collider.CompareTag("Cube"))
        {
            try
            {
                if (!SpawnWeapon_R.rightWeapon.weaponInIt && !SpawnWeapon_L.leftWeapon.weaponInIt)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllViaServer);
                    Debug.Log("��� ���� ���� ���������� �ı���");
                }
                if (!SpawnWeapon_R.rightWeapon.weaponInIt || SpawnWeapon_L.leftWeapon.weaponInIt)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllViaServer);
                    Debug.Log("�������� ���������� �ı���");
                }
                if (SpawnWeapon_R.rightWeapon.weaponInIt || !SpawnWeapon_L.leftWeapon.weaponInIt)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllViaServer);
                    Debug.Log("���������� ���������� �ı���");
                }

            }

            finally
            {
                Destroy(gameObject);
            }
        }
        /*else
        {
            Debug.Log("���� �ı����� �ʾ���");
        }*/

    }


    public void GetTarget()
    {
        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
        // ray.origin    ������ġ
        // ray.direction ����
        // fireDistance  ����
        // Color.red     ���̱��� ��
        // 10f           �����ð�
        //Debug.Log(" �������� : " + hit.point + "\n �Ÿ� : "
        //    + hit.distance + "\n �̸� : " + hit.collider.name + "\n �±� : " + hit.transform.tag);
    }
    public void FireBullet()                                              // ��Ʈ�ѷ� Ʈ���Ÿ� �̿��� �Ѿ� �߻����
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            audioSource.Play();
            muzzleFlash.Play();
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            PV.RPC("PunFire", RpcTarget.AllViaServer);
            fireTime = 0;
        }
    }

 /*   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }*/

    void Reload()                                   // �Ѿ� ������ �ð�
    {
        fireTime += Time.deltaTime;
    }
    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun_Pun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            //Debug.Log("�� ���� ����");
        }
        return null;
    }

    [PunRPC]
    public void PunFire()
    {
        audioSource.Play();
        muzzleFlash.Play();
    }

    /*[PunRPC]
    public void DestroyGun_Delay()                  // �� �ı� �ð� ������ �޼���
    {
        StartCoroutine(DestoryPN_Gun());
    }*/

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
    }
 /*   public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }*/

    /// <summary>
    /// �Ѿ� ������ ����
    /// fireTime�� 0�� �� ���� �Ѿ��� �߻��� �� �ְ� ���� �δ� �� 
    /// fireTime�� delayfireTime�� �Ѿ�� �߻� �Ұ�
    /// �Ѿ��� �߻��ϰ� ���� �ð��� �ٽ� 0���� �ʱ�ȭ
    /// </summary>
        
}

