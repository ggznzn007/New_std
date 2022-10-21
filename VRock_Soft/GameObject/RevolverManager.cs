using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.PXR;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;

public class RevolverManager : MonoBehaviourPun, IPunObservable
{
    public static RevolverManager RM;

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
    private readonly float delayfireTime = 0.5f;    // �Ѿ� ������ ���ѽð�
    private readonly float fireDistance = 1000f;     // �Ѿ� ��Ÿ�
    private XRController xt;
    private bool triggerBtnR;
    private bool triggerBtnL;

    private void Awake()
    {
        RM = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ���� 
        actorNumber = PV.OwnerActorNr;
        xt = (XRController)GameObject.FindObjectOfType(typeof(XRController));

    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        GetTarget();
        Reload();
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnR) && triggerBtnR)
        {
            PXR_Input.SetControllerVibration(0.5f, 8, PXR_Input.Controller.RightController);
        }
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnL) && triggerBtnL)
        {
            PXR_Input.SetControllerVibration(0.5f, 8, PXR_Input.Controller.LeftController);
        }

        if (!AvartarController.ATC.isAlive && PV.IsMine)
        {
            PV.RPC("DestroyGun", RpcTarget.All);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube") && (!SpawnWeapon_R.rightWeapon.weaponInIt || !SpawnWeapon_L.leftWeapon.weaponInIt
            || !SpawnWeapon_RW.RW.weaponInIt || !SpawnWeapon_LW.LW.weaponInIt))
        {
            PV.RPC("DestroyGun_Delay", RpcTarget.All);
        }
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
    }

    public void FireBullet()                                              // ��Ʈ�ѷ� Ʈ���Ÿ� �̿��� �Ѿ� �߻����
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            ActivateHaptic();
            audioSource.Play();
            muzzleFlash.Play();
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            PV.RPC("PunFire", RpcTarget.All);
            fireTime = 0;
        }
    }

    void ActivateHaptic()
    {    // �տ� ���� ������ ����
         // �ڿ� ���� ������ ����
        xt.SendHapticImpulse(0.2f, 0.3f);
        
       

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
}
