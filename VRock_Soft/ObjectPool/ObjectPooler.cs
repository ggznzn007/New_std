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

    public void PrePoolInstantiate()                                             // ���漭������ �Ѿ��� �̸� �����ؼ� Ǯ�� ����
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = PN.Instantiate(pool.tag, Vector3.zero, Quaternion.identity);
                //GameObject obj = PN.Instantiate(pool.tag, firePoint.position, firePoint.rotation);
                obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false);  // RPC�Ѿ��� ��Ȱ��ȭ
                objectPool.Enqueue(obj);                                                   // ������ �Ѿ��� ť�� ����
                Debug.Log("�Ѿ� ����(��Ȱ��ȭ)");
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject PoolInstantiate(string tag)//,Vector3 position, Quaternion rotation)  // Ǯ�� ����� �Ѿ��� Ȱ��ȭ�Ͽ� ���
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Ǯ�� �ش� {tag}�� �������� �ʽ��ϴ�.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, true);      // RPC�Ѿ��� Ȱ��ȭ
       // obj.transform.position = GunManager.gunManager.firePoint.position;
       // obj.transform.rotation = GunManager.gunManager.firePoint.rotation;
        //obj.transform.SetPositionAndRotation(position, rotation);                     // RPC�Ѿ��� ��ġ
        poolDictionary[tag].Enqueue(obj);                                            // ���� �Ѿ� �ٽ� ť�� ����
        Debug.Log("�Ѿ� ����(Ȱ��ȭ)");
        return obj;
    }

    public void PoolDestroy(GameObject obj)                                                // ����� �Ѿ��� �ٽ� Ǯ�� ����
    {
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false);    // ���� �Ѿ� �ٽ� ��Ȱ��ȭ
        Debug.Log("���� �Ѿ�(��Ȱ��ȭ)");
    }
  
}
