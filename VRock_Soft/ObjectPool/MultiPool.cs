using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPool
{
    // ������Ʈ Ǯ�� �����Ǵ� ������Ʈ ����
    private class PoolItem
    {
        public bool isActive;            // ���ӿ�����Ʈ�� Ȱ��ȭ,��Ȱ��ȭ ����
        public GameObject gameObject;    // ȭ�鿡 ���̴� ���� ���ӿ�����Ʈ
    }

    private readonly int increaseCount = 5;       // ������Ʈ ���� �� �߰� �����Ǵ� ������Ʈ ��
    private int maxCount;                // ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ��
    private int activeCount;             // ���� ���ӿ� ���ǰ� �ִ�(Ȱ��ȭ) ������Ʈ ��
    private GameObject poolObject;       // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList; // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

    public MultiPool(GameObject poolObect)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObect;

        poolItemList = new List<PoolItem>();

        InstantiateObject();
    }

    public void InstantiateObject()   // increaseCount ������ ������Ʈ �����ϴ� �Լ�
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

    public void DestroyObjects()    // ���� ��������(Ȱ��/��Ȱ��) ��� ������Ʈ �ı�
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

        // ���� �����ؼ� �����ϴ� ��� ��������Ʈ ������ ���� Ȱ��ȭ ���� ������Ʈ ���� ��
        // ��� ������Ʈ�� Ȱ��ȭ �����̸� ���ο� ������Ʈ �ʿ�
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


    public void DeactivatePoolItem(GameObject removeObject)  // ���� ��� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ����
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

    public void DeactivateAllPoolItem()  // ���ӿ� ������� ������Ʈ�� ��Ȱ��ȭ ���·� ����
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
  