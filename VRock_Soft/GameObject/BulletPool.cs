using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletPool : MonoBehaviour
{
    public static BulletPool _instance;
    [Header("Bullet Pool")]
    public GameObject bulletPrefab;    
    public int maxPool = 10;
    public List<GameObject> bulletPool = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
        CreatePooling();
    }
    public void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate(bulletPrefab,objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletPool.Add(obj);           
        }
        
    }

    public GameObject GetBullet() // 총알을 발사할 때 오브젝트를 추출하기 위한 메서드
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
}