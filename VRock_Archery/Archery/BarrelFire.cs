using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using System.Collections;

public class BarrelFire : MonoBehaviourPunCallbacks, IPunObservable
{
    public Collider exColl;
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;    

    [System.Obsolete]
    private IEnumerator Start()
    {
        PV = GetComponent<PhotonView>();        
        StartCoroutine(CollOnOff());
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(PV.gameObject);
    }
    public IEnumerator CollOnOff()
    {
        while(true)
        {
            exColl.enabled = true;
            yield return new WaitForSeconds(0.03f);
            exColl.enabled = false;
            yield return new WaitForSeconds(0.8f);
        }        
    }
   
    private void Update()
    {
        if (!PV.IsMine)
        {
            float t = Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, t)
                , Quaternion.Lerp(transform.rotation, remoteRot, t));
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
}
