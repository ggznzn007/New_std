using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPool
{
    // 오브젝트 풀로 관리되는 오브젝트 정보
    private class PoolItem
    {
        public bool isActive;            // 게임오브젝트의 활성화,비활성화 정보
        public GameObject gameObject;    // 화면에 보이는 실제 게임오브젝트
    }

    private readonly int increaseCount = 5;       // 오브젝트 부족 시 추가 생성되는 오브젝트 수
    private int maxCount;                // 현재 리스트에 등록되어 있는 오브젝트 수
    private int activeCount;             // 현재 게임에 사용되고 있는(활성화) 오브젝트 수
    private GameObject poolObject;       // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList; // 관리되는 모든 오브젝트를 저장하는 리스트

    public MultiPool(GameObject poolObect)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObect;

        poolItemList = new List<PoolItem>();

        InstantiateObject();
    }

    public void InstantiateObject()   // increaseCount 단위로 오브젝트 생성하는 함수
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; i++)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()    // 현재 관리중인(활성/비활성) 모든 오브젝트 파괴
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        // 현재 생성해서 관리하는 모드 ㄴ오브젝트 개수와 현재 활성화 상태 오브젝트 개수 비교
        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 필요
        if(maxCount==activeCount)
        {
            InstantiateObject();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive==false)
            {
                activeCount++;

                poolItem.isActive=true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }
        return null;
    }


    public void DeactivatePoolItem(GameObject removeObject)  // 현재 사용 완료된 오브젝트를 비활성화 상태로 변경
    {
        if(poolItemList== null|| removeObject==null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject==removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolItem()  // 게임에 사용중인 오브젝트를 비활성화 상태로 변경
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null&&poolItem.isActive==true)
            {
                
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);               
            }
        }
        activeCount = 0;
    }
}
  