using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class AcheryEagle : MonoBehaviourPunCallbacks//, IPunObservable
{
    public GameObject eagleBomb;
    public GameObject effect;
    public Transform spawnPoint;
    public Transform[] wayPos;
    public float speed;
    public string eagleDam;
    int wayNum = 0;
    private GameObject myBomb;
    private Animator animator;
    private PhotonView PV;

    public void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        transform.position = wayPos[wayNum].transform.position;
        if (PN.IsMasterClient)
        {
           InvokeRepeating(nameof(SpawnEB), 2, 4);
        }
    }

    private void FixedUpdate()
    {
        MovetoWay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            if (PV.IsMine)
            {
                PV.RPC(nameof(EDamage), RpcTarget.AllBuffered);
            }
        }
    }


    [PunRPC]
    public void EDamage()
    {
        StartCoroutine(EagleDamage());
        myBomb.transform.SetParent(transform, false);
        myBomb.GetComponent<Rigidbody>().useGravity = true;
        myBomb.GetComponent<SphereCollider>().enabled = true;
    }
    public IEnumerator EagleDamage()
    {
        animator.SetBool("Damaged", true);
        AudioManager.AM.PlaySE(eagleDam);
        PN.Instantiate(effect.name, transform.position + new Vector3(0, 1.2f, 0), transform.rotation, 0);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Damaged", false);
    }


    public void MovetoWay()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPos[wayNum].position, speed * Time.deltaTime);
        transform.LookAt(wayPos[wayNum].position);

        if (transform.position == wayPos[wayNum].transform.position)
        {
            wayNum++;
        }

        if (wayNum == wayPos.Length) { wayNum = 0; }
    }

    public void SpawnEB()
    {
        if (myBomb == null)
        {
            if (myBomb != null) return;
            myBomb = PN.InstantiateRoomObject(eagleBomb.name, spawnPoint.position, spawnPoint.rotation, 0);
            myBomb.transform.SetParent(transform, true);
            myBomb.GetComponent<Rigidbody>().useGravity = false;
            myBomb.GetComponent<SphereCollider>().enabled = false;
        }

        // PV.RPC(nameof(SpawnBarrel), RpcTarget.AllBuffered);
    }

    /*[PunRPC]
    public void SpawnBarrel()
    {
       
    }*/


    /*



        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(myBomb.transform.position);
                stream.SendNext(myBomb.transform.rotation);
            }
            else
            {
                myBomb.transform.SetPositionAndRotation((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            }
        }*/
}
