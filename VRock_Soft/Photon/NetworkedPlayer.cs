using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PhotonNetwork;
using Random = UnityEngine.Random;

public class NetworkedPlayer : MonoBehaviourPunCallbacks
{

	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;
	private PhotonView pv;
	

	void Start () 
	{
		pv = GetComponent<PhotonView>();
		Debug.Log("I am Networked Player");	

		if (pv.IsMine)
		{
			Debug.Log("photonview is mine");	
			//playerGlobal = GameObject.Find("CameraController").transform;
			//playerLocal = playerGlobal.Find("Dive_Camera");

			this.transform.SetParent(playerLocal);
			this.transform.localPosition = Vector3.zero;

			//avatar.SetActive(false);
			//avatar =  Resources.Load("NetworkedPlayer/avatar") as GameObject;
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Debug.Log ("send the others my data");

			// stream.SendNext(playerGlobal.position);
			// stream.SendNext(playerGlobal.rotation);
			// stream.SendNext(playerLocal.localPosition);
			// stream.SendNext(playerLocal.localRotation);
		}
		else
		{
			this.transform.position = (Vector3)stream.ReceiveNext();
			this.transform.rotation = (Quaternion)stream.ReceiveNext();
			avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
			avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
