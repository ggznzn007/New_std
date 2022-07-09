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
public class TeamSelectInit : MonoBehaviourPunCallbacks  // 스폰되는 플레이어 바디에 붙어있는 스크립트
{
    public static TeamSelectInit teamSelect;
    public MeshRenderer mesh;
    public Material[] mats;
    public bool isSeleted;
    public TextMesh teamText;

    private PhotonView PV;
    private void Awake()
    {
        if (teamSelect == null)
        {
            teamSelect = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (teamSelect != this)
            {
                Destroy(this.gameObject);
            }
        }

        
    }
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
            teamText.text = "레드팀";
            teamText.color = Color.red;
            isSeleted = true;
            Debug.Log("레드팀으로 태그");            
        }

        if (coll.gameObject.tag == "BlueTeam" && !isSeleted)
        {
            PN.AutomaticallySyncScene = true;
            mesh.materials[0].color = Color.blue;
            teamText.text = "블루팀";
            teamText.color = Color.blue;
            isSeleted = true;
            Debug.Log("블루팀으로 태그");

        }
    }

    
}
