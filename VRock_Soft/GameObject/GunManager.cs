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
public class GunManager : MonoBehaviourPun, IPunObservable  // ÃÑÀ» °ü¸®ÇÏ´Â ½ºÅ©¸³Æ®
{
    public static GunManager gunManager;
    public GameObject bullet;
    public float speed;
    public float fireDistance = 20f;
    RaycastHit hit;
    Ray ray;
    public Transform firePoint;  // ÃÑ±¸        
    private ParticleSystem muzzleFlash;    // ÃÑ±¸ ÀÌÆåÆ®
                                           // private PhotonView PV;
    AudioSource audioSource;             // ÃÑ¾Ë ¹ß»ç ¼Ò¸®
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
        muzzleFlash = firePoint.GetComponentInChildren<ParticleSystem>();  // ÇÏÀ§ ÄÄÆ÷³ÍÆ® ÃßÃâ
                                                                           // firePoint.GetComponent<BulletManager>();
        FindGun();
        //OP.PrePoolInstantiate();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        ray = new Ray(firePoint.position, firePoint.forward);
        ray.origin = firePoint.position;
        ray.direction = firePoint.forward;
    }
    public GunManager FindGun()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            if (gun.GetPhotonView().IsMine) return gun.GetComponent<GunManager>();
            Debug.Log("ÀÌ ÃÑÀº ³»²¨");
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.red);
    }
    public void FireBullet()
    {

        //if (!PV.IsMine) { return; }
        if (PV.IsMine && Physics.Raycast(ray.origin, ray.direction, out hit, fireDistance))
        {
            audioSource.Play();
            muzzleFlash.Play();
            GameObject bullet = PN.Instantiate("Bullet", ray.origin, Quaternion.identity);
            bullet.GetComponent<PhotonView>().RPC("BulletDir", RpcTarget.All, speed);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(ray.direction * speed, ForceMode.Force);
            PV.RPC("PunFire", RpcTarget.All);
            Debug.Log(hit.transform.name);
            ray.origin = hit.point;
            //GameObject _bullet = OP.PoolInstantiate("Bullet");
            //GameObject _bullet = PN.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
            //GameObject _bullet = PoolManager.PoolingManager.pool.Dequeue();
            //GameObject _bullet = PN.Instantiate("Bullet", firePoint.transform.position, firePoint.transform.rotation);
            //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);   // ÃÑ¾Ë À§Ä¡
            //_bullet.GetPhotonView().ViewID = photonView.ViewID++;         
            // _bullet.GetPhotonView().OwnerActorNr = actorNumber;            
            // _bullet.GetPhotonView().ControllerActorNr = actorNumber;            

            // _bullet.transform.GetComponent<Rigidbody>().AddForce(firePoint.forward * speed);
            //  _bullet.SetActive(true);
            Debug.Log("Bullet ¹ß»ç");
            //PV.RPC("PunFire", RpcTarget.Others);
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
                        PV.RPC("DestroyGun_Delay", RpcTarget.AllBuffered);
                        //DestroyGun_Delay();
                        //PN.Destroy(this.gameObject);
                        SpawnWeapon_L.leftWeapon.weaponInIt = false;
                        SpawnWeapon_R.rightWeapon.weaponInIt = false;
                        //Debug.Log("ÃÑ ÆÄ±«µÊ");
                    }

                }
            }

            Debug.Log("¹Ù´Ú¿¡ ÅÂ±×µÊ");

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

        //_bullet.GetComponent<BulletManager>().actorNumber = actNumber;                           // ÃÑ¾Ë Æ÷Åæºä »ç¿ëÀÚ ¹øÈ£

        /* if (_bullet != null)
         {
             //_bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

         }*/
    }

    [PunRPC]
    public void DestroyGun_Delay()                  // ÃÑ ÆÄ±« ½Ã°£ µô·¹ÀÌ ¸Þ¼­µå
    {
        StartCoroutine(DestoryPN_Gun());
    }
    public IEnumerator DestoryPN_Gun()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
        Debug.Log("ÃÑ ÆÄ±«");
        // Destroy(gameObject);
    }


}

