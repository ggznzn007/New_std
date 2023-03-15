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

public class DestroySnow : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView PV;
    private Vector3 remotePos;
    private Quaternion remoteRot;

    [System.Obsolete]
    private IEnumerator Start()
    {
        PV = GetComponent<PhotonView>();
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(PV.gameObject);
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
