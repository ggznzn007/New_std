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
using UnityEngine.XR;

public class BulletPool : MonoBehaviour
{
    public static BulletPool BulletPooling;
    public GameObject bulletPrefab;    
    public int maxbulletPool = 10;   
    public List<GameObject> bulletPool = new List<GameObject>();  

    private void Awake()
    {
        BulletPooling = this;
        CreateBulletPooling();       
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (bulletPool[i].activeSelf==false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
    public void CreateBulletPooling()
    {
        GameObject objectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxbulletPool; i++)
        {
            var obj = Instantiate(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }

  

}
