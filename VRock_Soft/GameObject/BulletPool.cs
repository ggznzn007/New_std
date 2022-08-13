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

public class BulletPool : MonoBehaviourPun // 싱글 플레이 풀방식
{
    [SerializeField] private Poolable poolBullet;
    [SerializeField] private int allocateCount;

    //private readonly Stack<Poolable> poolStack = new Stack<Poolable>();  // 스택 방식
    private readonly Queue<Poolable> poolQueue = new Queue<Poolable>();    // 큐   방식

    private void Start()
    {
        Allocate();
    }

    public void Allocate()
    {
        for (int i = 0; i < allocateCount; i++)
        {
            Poolable allocateBull = Instantiate(poolBullet, this.gameObject.transform);
            allocateBull.CreateBullet(this);
            allocateBull.GetComponent<PhotonView>();
            poolQueue.Enqueue(allocateBull);
            //Poolable allocateBull = Instantiate(poolBullet);
            // poolStack.Push(allocateBull);
        }
    }

    public GameObject Dequeue()
    {

        Poolable bullet = poolQueue.Dequeue();
        // bullet.gameObject.SetActive(true);
        return bullet.gameObject;
        // return null;
    }

    public void Enqueue(Poolable bullet)
    {

        bullet.gameObject.SetActive(false);
        poolQueue.Enqueue(bullet);


    }  // 큐 방식

    /* public GameObject Pop()
     {
         Poolable bullet = poolStack.Pop();
         //bullet.gameObject.SetActive(true);
         return bullet.gameObject;
     }
 */
    /*  public void Push(Poolable bullet)
      {
          poolStack.Push(bullet);
          bullet.gameObject.SetActive(false);
      }  // 스택방식
  */

    /*   public static BulletPool BulletPooling;
       public GameObject bulletPrefab;
       public int maxbulletPool = 10;
       public List<GameObject> bulletPool = new List<GameObject>();
      // public Stack<GameObject> bulletPoolStack = new Stack<GameObject>();
       //private GameObject bulletPools = new GameObject("BulletPools");
       private void Awake()
       {
           BulletPooling = this;
           CreateBulletPooling();
       }

       public GameObject GetBullet()
       {
           for (int i = 0; i < bulletPool.Count; i++)
           {
               if (!bulletPool[i].activeSelf)
               {
                   return bulletPool[i];
               }
           }
           return null;
       }

       public void ReturnBullet()
       {
           gameObject.SetActive(false);
       }

       public void CreateBulletPooling()
       {
           GameObject bulletPools = new GameObject("BulletPools");
           for (int i = 0; i < maxbulletPool; i++)
           {
               GameObject obj = Instantiate(bulletPrefab, bulletPools.transform);
               obj.name = "Bullet_" + i.ToString("00");
               obj.SetActive(false);
               bulletPool.Add(obj);
              // bulletPoolStack.Push(obj);
           }


       }  // 기존 1*/




    /* private static BulletPool bulletPooling;
     public static BulletPool BulletPooling { get { return bulletPooling; } }
     public GameObject bulletPrefab;
     private List<GameObject> bullets;
     public int bullCount = 20;

     private void Awake()
     {
         bulletPooling = this;
         bullets = new List<GameObject>(bullCount);
         for (int i = 0; i < bullCount; i++)
         {
             GameObject bullInstance = Instantiate(bulletPrefab);
             bullInstance.transform.SetParent(transform);
             bullInstance.SetActive(false);
             bullets.Add(bullInstance);
         }
     }

     public GameObject GetBullet()
     {
         foreach (GameObject bullet in bullets)
         {
             if(!bullet.activeInHierarchy)
             {
                 bullet.SetActive(true);
                 return bullet;
             }
         }

         GameObject bullInstance = Instantiate(bulletPrefab);
         bullInstance.transform.SetParent(transform);
         bullets.Add(bullInstance);
         return bullInstance;
     }*/ // otherPooling 1


}
