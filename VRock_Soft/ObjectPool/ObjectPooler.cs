using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;


public class ObjectPooler : MonoBehaviourPun
{
    [System.Serializable]

    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler OP;
    //public Transform firePoint;
    void Awake() => OP = this;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Queue<GameObject> myObj = new Queue<GameObject>();

    public void PrePoolInstantiate()                                             // 포톤서버에서 총알을 미리 생성해서 풀에 저장
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = PN.Instantiate(pool.tag, Vector3.zero, Quaternion.identity);
                //GameObject obj = PN.Instantiate(pool.tag, firePoint.position, firePoint.rotation);
                obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false);  // RPC총알을 비활성화
                objectPool.Enqueue(obj);                                                   // 생성한 총알을 큐에 저장
                Debug.Log("총알 생성(비활성화)");
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject PoolInstantiate(string tag)//,Vector3 position, Quaternion rotation)  // 풀에 저장된 총알을 활성화하여 사용
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"풀에 해당 {tag}가 존재하지 않습니다.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, true);      // RPC총알을 활성화
       // obj.transform.position = GunManager.gunManager.firePoint.position;
       // obj.transform.rotation = GunManager.gunManager.firePoint.rotation;
        //obj.transform.SetPositionAndRotation(position, rotation);                     // RPC총알의 위치
        poolDictionary[tag].Enqueue(obj);                                            // 사용된 총알 다시 큐로 저장
        Debug.Log("총알 생성(활성화)");
        return obj;
    }

    public void PoolDestroy(GameObject obj)                                                // 사용한 총알을 다시 풀에 저장
    {
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false);    // 사용된 총알 다시 비활성화
        Debug.Log("사용된 총알(비활성화)");
    }
  
}
