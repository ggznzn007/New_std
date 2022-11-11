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

public class GunManager : MonoBehaviourPun, IPunObservable  // ���� �����ϴ� ��ũ��Ʈ
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
        if (!PV.IsMine) return;
        GetTarget();
        Reload();
       // OwnerTransferGun();
       /* switch (DataManager.DM.currentMap)
        {
            case Map.TUTORIAL_T:
            case Map.TOY:
                DroptheGunT();
                break;
            case Map.TUTORIAL_W:
            case Map.WESTERN:
                DropGunW();
                break;
        }*/

        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.All);
        }

    }

    public void OwnerTransferGun()  // ���� ��������� ������ ���ϰ�
    {
        if (SpawnWeapon_L.leftWeapon.weaponInIt || SpawnWeapon_R.rightWeapon.weaponInIt)
        {
            PV.OwnershipTransfer = OwnershipOption.Fixed;
        }
        else
        {
            PV.OwnershipTransfer = OwnershipOption.Request;
        }
    }


    /*public void DroptheGunT()                                            // ���� ������ �� �ڵ� �ı�
    {
        if (SpawnWeapon_R.rightWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_L.leftWeapon.targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_R)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
        }


    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if(DataManager.DM.currentTeam!=Team.ADMIN)
        {
            if (collision.collider.CompareTag("Cube") && (!SpawnWeapon_R.rightWeapon.weaponInIt || !SpawnWeapon_L.leftWeapon.weaponInIt
            || !SpawnWeapon_RW.RW.weaponInIt || !SpawnWeapon_LW.LW.weaponInIt))
            {
                PV.RPC("DestroyGun_Delay", RpcTarget.All);
            }
        }
        
    }
    /*public void DropGunW()
    {
        if (SpawnWeapon_RW.RW.DeviceR.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_R) &&
            SpawnWeapon_LW.LW.DeviceL.TryGetFeatureValue(CommonUsages.gripButton, out bool griped_L))
        {
            if (!griped_R && !griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_L && griped_R)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
            else if (!griped_R && griped_L)
            {
                if (PV.IsMine)
                {
                    PV.RPC("DestroyGun_Delay", RpcTarget.All);
                }
            }
        }
    }*/

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
            PV.RPC("PunFire", RpcTarget.All);
            fireTime = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }

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

    [PunRPC]
    public void DestroyGun_Delay()                  // �� �ı� �ð� ������ �޼���
    {
        StartCoroutine(DestoryPN_Gun());
    }

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    /// <summary>
    /// �Ѿ� ������ ����
    /// fireTime�� 0�� �� ���� �Ѿ��� �߻��� �� �ְ� ���� �δ� �� 
    /// fireTime�� delayfireTime�� �Ѿ�� �߻� �Ұ�
    /// �Ѿ��� �߻��ϰ� ���� �ð��� �ٽ� 0���� �ʱ�ȭ
    /// </summary>

    /*public void FireBullet_Red()
    {
        //if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit,fireDistance) && AvartarController.ATC.isAlive)
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            Debug.Log(" �������� : " + hit.point + "\n �Ÿ� : "
                + hit.distance + "\n �̸� : " + hit.collider.name + "\n �±� : " + hit.transform.tag);
            audioSource.Play();
            muzzleFlash.Play();

            GameObject bullet = PN.Instantiate("Bullet_Red", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
            bullet.GetPhotonView().OwnerActorNr = actorNumber;
            PV.RPC("PunFire", RpcTarget.All);


            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // �Ѿ� ��ġ
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            //_bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            //_bullet.GetPhotonView().ControllerActorNr = actorNumber;    
            //_bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //_bullet.SetActive(true);

            //Debug.Log("�Ѿ� �߻�");
        }
    }
    public void FireBullet_Blue()
    {
        //if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit,fireDistance) && AvartarController.ATC.isAlive)
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            Debug.Log(" �������� : " + hit.point + "\n �Ÿ� : "
                + hit.distance + "\n �̸� : " + hit.collider.name + "\n �±� : " + hit.transform.tag);
            audioSource.Play();
            muzzleFlash.Play();

            GameObject bullet = PN.Instantiate("Bullet_Blue", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
            bullet.GetPhotonView().OwnerActorNr = actorNumber;
            PV.RPC("PunFire", RpcTarget.All);


            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // �Ѿ� ��ġ
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            //_bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            //_bullet.GetPhotonView().ControllerActorNr = actorNumber;    
            //_bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //_bullet.SetActive(true);

            //Debug.Log("�Ѿ� �߻�");
        }
    }*/


    /* [PunRPC]
     public void TakeDamage(float damage,PhotonMessageInfo info)
     {
         AvartarController.ATC.curHP -= damage;
         AvartarController.ATC.HP.fillAmount = AvartarController.ATC.curHP / AvartarController.ATC.inItHP;

         if(AvartarController.ATC.curHP<=0.0f)
         {
             Debug.Log(info.Sender.NickName + "����" + info.photonView.Owner.NickName);
         }
     }*/

    /* [PunRPC]
     public void FireBull(int actorNumber)
     {
         audioSource.Play();
         muzzleFlash.Play();
         GameObject bullet = Instantiate(bulletPrefab, ray.origin, Quaternion.identity);
         bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
        // bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
         bullet.GetComponent<BulletManager>().actNumber = actorNumber;
     }*/

}

