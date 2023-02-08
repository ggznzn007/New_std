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
public class BowManager : MonoBehaviourPun, IPunObservable
{
    public static BowManager BowM;
    public PhotonView PV;
   // public Transform pull;
   // public Transform notch;
    public Transform bowString;
    //private Vector3 remotePos;
    //private Quaternion remoteRot;
   // public List<Collider> bowColls;
    public bool isBeingHeld = false;
    public bool isGrip;
    Rigidbody rb;
    public GameObject shield_R;
    public GameObject shield_L;
    public GameObject bow;    
    public Notch notch;    
    public Collider pullColl;
    public bool isRight;

    private void Awake()
    {
        BowM = this;
        PV = GetComponent<PhotonView>();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shield_R.SetActive(false);
        shield_L.SetActive(false);
        bow.SetActive(true);
        pullColl.enabled = true;
        notch.enabled = true;
        isGrip = true;       
        isRight= false;
    }

    
    void Update()
    {
       /* if (!PV.IsMine)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 10 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.deltaTime));
        }*/

        if (isBeingHeld)               
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(notch.position);
            //stream.SendNext(notch.rotation);
            //stream.SendNext(pull.position);
            //stream.SendNext(pull.rotation);
            stream.SendNext(bowString.position);
            stream.SendNext(bowString.rotation);
        }
        else
        {
            //notch.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            //pull.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            bowString.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Cube")|| collision.collider.CompareTag("Finish"))
        {
            if(PV.IsMine)
            {
                if(!isGrip)
                {
                    try
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                        Debug.Log("활이 파괴되었습니다.");
                    }
                    finally
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                    }
                }
            }
           /* try
            {
                if (!isBeingHeld && !isGrip)
                {
                    if (PV.IsMine)
                    {
                        PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                        Debug.Log("활이 파괴되었습니다.");
                    }
                }
            }
            finally
            {
                if (PV.IsMine)
                {
                    PV.RPC(nameof(DestroyBow), RpcTarget.AllBuffered);
                }
            }*/
        }
              
    }

    private void OnTriggerStay(Collider coll)
    {
        if(coll.CompareTag("RightHand"))
        {
            //if (coll.CompareTag("LeftHand")) return;
            if (PV.IsMine)
            {
                isRight = true;
                
            }
        }

        if (coll.CompareTag("LeftHand"))
        {
            //if (coll.CompareTag("RightHand")) return;
            if (PV.IsMine)
            {
                isRight = false;
            }
        }
    }

    [PunRPC]
    public void DestroyBow()=> Destroy(PV.gameObject);


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

    [PunRPC]
    public void ShieldOn()
    {
        if(isRight)
        {
            shield_R.SetActive(true);
            bow.SetActive(false);
            pullColl.enabled= false;
            notch.enabled = false;
        }
        else
        {
            shield_L.SetActive(true);
            bow.SetActive(false);
            pullColl.enabled = false;
            notch.enabled = false;
        }
        
      
    }

    [PunRPC]
    public void ShieldOff()
    {
        if (isRight)
        {
            shield_R.SetActive(false);
            bow.SetActive(true);
            pullColl.enabled = true;
            notch.enabled = true;
        }
        else
        {
            shield_L.SetActive(false);
            bow.SetActive(true);
            pullColl.enabled = true;
            notch.enabled = true;
        }       
      
    }

    public void OnSelectedEntered()
    {
        Debug.Log("활을 잡았습니다.");
        PV.RPC(nameof(StartGrabbing), RpcTarget.AllBuffered);       
    }

    public void OnSelectedExited()
    {
        Debug.Log("활을 놓았습니다.");
        PV.RPC(nameof(StopGrabbing), RpcTarget.AllBuffered);
    }

    public void OnActive()
    {
        PV.RPC(nameof(ShieldOn), RpcTarget.AllBuffered);
    }

    public void OnDeactive()
    {
        PV.RPC(nameof(ShieldOff), RpcTarget.AllBuffered);
    }

  
}
