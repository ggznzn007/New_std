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
    public PhotonView PV;                           // �����
    public bool isBeingHeld = false;
    public bool isGrip;

    private RaycastHit hit;                          // ����ĳ��Ʈ���� ��Ʈ
    private Ray ray;                                 // ����ĳ��Ʈ ����
    private ParticleSystem muzzleFlash;              // �ѱ� ����Ʈ                                           
    private AudioSource audioSource;                 // �Ѿ� �߻� �Ҹ�
    private GameObject myBull;                       // �ڱ� �Ѿ�    
    private float fireTime = 0;                      // �Ѿ� ������ Ÿ�� 
    private readonly float delayfireTime = 0.3f;    // �Ѿ� ������ ���ѽð�
    private readonly float fireDistance = 1000f;     // �Ѿ� ��Ÿ�   
    private bool triggerBtnR;
    private bool triggerBtnL;
    private Vector3 remotePos;
    private Quaternion remoteRot;
    private Rigidbody rb;

    private void Awake()
    {
        RM = this;        
    }
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ���� ������Ʈ ���� 
        actorNumber = PV.OwnerActorNr;
        isGrip = true;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }       
        GetTarget();       // ǥ���� ����ĳ��Ʈ�� ���� Ÿ�����ϴ� �޼���
        Reload();          // ���� �������ϴ� �ð� �޼���       
        //WhenDead();       // �÷��̾ �׾����� ���� ������� �޼���
        ActivateHaptic();

        if (isBeingHeld)               // ���� ���忡�� �տ� ��������
        {
            isGrip = true;
            rb.isKinematic = true;
        }
        else
        {
            isGrip = false;
            rb.isKinematic = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Cube") || collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Shield"))
        {
            if (PV.IsMine)
            {
                if (!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }
                    finally
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }                    
                }
            }
            /*try
            {
                if (PV.IsMine)
                {
                    if (!isBeingHeld && !isGrip)
                    {
                        PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                    }
                }
            }
            finally
            {
                if (PV.IsMine)
                {
                    PV.RPC(nameof(DestroyGun), RpcTarget.AllBuffered);
                }
            }*/
        }        
    }

    public void FireBullet()                                              // ��Ʈ�ѷ� Ʈ���Ÿ� �̿��� �Ѿ� �߻����
    {
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit) && AvartarController.ATC.isAlive)
        {
            if (fireTime < delayfireTime) { return; }
            PV.RPC(nameof(Fire_EX), RpcTarget.All);   
            myBull = PN.Instantiate(bullet.name, ray.origin, Quaternion.identity);
            myBull.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);// �������� �������� ���� ����
            myBull.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.Others, speed, PV.Owner.ActorNumber);
            myBull.GetComponent<Bullet_rManager>().actNumber = actorNumber;
            fireTime = 0;
        }
    }
    
    void ActivateHaptic()
    {    // �տ� ���� ������ ����
         // �ڿ� ���� ������ ����
         // xt.SendHapticImpulse(0.2f, 0.3f); //��ŧ����

        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnR) && triggerBtnR)
        {
            PXR_Input.SetControllerVibration(0.25f, 2, PXR_Input.Controller.RightController);
        }
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerBtnL) && triggerBtnL)
        {
            PXR_Input.SetControllerVibration(0.25f, 2, PXR_Input.Controller.LeftController);
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
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Reload()                                   // �Ѿ� ������ �ð�
    {
        fireTime += Time.deltaTime;
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

    [PunRPC]
    public void DestroyGun()
    {
        Destroy(gameObject);
        //Destroy(PV.gameObject);
    }

    [PunRPC]
    public void StartGrabbing()
    {
        isBeingHeld = true;
    }

    [PunRPC]
    public void StopGrabbing()
    {
        isBeingHeld = false;
    }

    public void OnSelectedEntered()
    {
        Debug.Log("��Ҵ�");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);
    }

    public void OnSelectedExited()
    {
        Debug.Log("���Ҵ�");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Fire_EX()
    {
        audioSource.Play();
        muzzleFlash.Play();
    }
   
    
}
