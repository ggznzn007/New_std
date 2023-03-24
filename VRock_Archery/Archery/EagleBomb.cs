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

public class EagleBomb : MonoBehaviourPunCallbacks, IPunObservable// ��ó ������ �Ʒ� �����Ǵ� ��ź�� �����ϴ� ��ũ��Ʈ
{
    public PhotonView PV;            // �����
    public Rigidbody rb;             // ������ٵ�
    public SphereCollider bombColl;  // ��ź �ݶ��̴� = �ٴڿ� �������� �� �浹 ����    
    public GameObject myEX;          // ��ź ���߽� �����Ǵ� ��ƼŬ ������
    public GameObject myMesh;        // ��ź �޽�
    public Transform exploPoint;     // ��ź ���� ��ġ
    public string fireCircle;        // ��ź ���� �� ����Ǵ� ����� ���ڿ�
    private Vector3 remotePos;       // ������ ���� �����Ǵ� ����Ʈ ��ġ
    private Quaternion remoteRot;    // ������ ���� �����Ǵ� ����Ʈ ȸ��    

    void Awake()
    {
        PV = GetComponent<PhotonView>();        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bombColl = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)  // �ڽ�(��ź) �̿��� �ٸ� �÷��̾�� �ڽ��� ��ġ �� ȸ������ �ε巴�� ����
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, remotePos, 30 * Time.deltaTime)
                , Quaternion.Lerp(transform.rotation, remoteRot, 30 * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox"))  // ��ź�� �ٴڿ� �������� �±׵Ǹ� �����ϴ� RPC ȣ��
        {
            PV.RPC(nameof(FireCircle), RpcTarget.AllBuffered);           
        }

        if(collision.collider.CompareTag("Cube"))       // ��ź�� �ٴ� �̿ܿ� �������� �±׵Ǹ� ������� RPC ȣ��
        {
            PV.RPC(nameof(FireDelete), RpcTarget.AllBuffered);
        }
    }

    public IEnumerator Explode()
    {
        Destroy(gameObject);
        PN.InstantiateRoomObject(myEX.name, exploPoint.position, exploPoint.rotation);
        yield return new WaitForSeconds(0.1f);       
    }

    [PunRPC]
    public void FireCircle()
    {                
        StartCoroutine(Explode());          
    }

    [PunRPC]
    public void FireDelete()
    {
        Destroy(gameObject);
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
