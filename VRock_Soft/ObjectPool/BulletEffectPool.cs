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

public class BulletEffectPool : MonoBehaviour
{
    public static BulletEffectPool EffectPooling;
    public GameObject bulletEffecPrefab;
    public int maxbulletEffecPool = 10;
    public List<GameObject> bulletEffecPool = new List<GameObject>();

    private void Awake()
    {
        EffectPooling = this;
        CreateBulletEffectPooling();
    }

    public GameObject GetBulletEffect()
    {
        for (int i = 0; i < bulletEffecPool.Count; i++)
        {
            if (bulletEffecPool[i].activeSelf == false)
            {
                return bulletEffecPool[i];
            }
        }
        return null;
    }

    public void CreateBulletEffectPooling()
    {
        GameObject objectPools = new GameObject("EffectPools");

        for (int i = 0; i < maxbulletEffecPool; i++)
        {
            var obj = Instantiate(bulletEffecPrefab, objectPools.transform);
            obj.name = "BulletEffect_" + i.ToString("00");
            obj.SetActive(false);
            bulletEffecPool.Add(obj);
        }
    }
}
