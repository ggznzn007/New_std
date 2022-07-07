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
public class TeamSelectInit : MonoBehaviourPunCallbacks  // �����Ǵ� �÷��̾� �ٵ� �پ��ִ� ��ũ��Ʈ
{
    
    MeshRenderer mesh;
    Material[] mats;
    public bool isSeleted;
    public TextMesh teamText;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mats = mesh.materials;
        isSeleted = false;
       
    }
    
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag =="RedTeam"&&!isSeleted)
        {
            PN.AutomaticallySyncScene = true;
            mesh.materials[0].color = Color.red;
            teamText.text = "������";
            teamText.color = Color.red;
            isSeleted = true;
            Debug.Log("���������� �±�");            
        }

        if (coll.gameObject.tag == "BlueTeam" && !isSeleted)
        {
            PN.AutomaticallySyncScene = true;
            mesh.materials[0].color = Color.blue;
            teamText.text = "�����";
            teamText.color = Color.blue;
            isSeleted = true;
            Debug.Log("��������� �±�");

        }
    }
}
