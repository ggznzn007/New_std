using UnityEngine;
using System.Collections.Generic;

public class MemoryPool
{
	// �޸� Ǯ�� �����Ǵ� ������Ʈ ����
	private class PoolItem
	{
		public	bool		isActive;		// "gameObject"�� Ȱ��ȭ/��Ȱ��ȭ ����
		public	GameObject	gameObject;		// ȭ�鿡 ���̴� ���� ���ӿ�����Ʈ
	}
	
	private	int	increaseCount = 5;			// ������Ʈ�� ������ �� Instantiate()�� �߰� �����Ǵ� ������Ʈ ����
	private	int	maxCount;					// ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ����
	private	int	activeCount;				// ���� ���ӿ� ���ǰ� �ִ�(Ȱ��ȭ) ������Ʈ ����

	private	GameObject		poolObject;		// ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
	private	Transform		poolParent;		// �����ϴ� ���� ������Ʈ�� �θ� Transform
	private	List<PoolItem>	poolItemList;	// �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

	public	int	MaxCount	=> maxCount;	// �ܺο��� ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ
	public	int	ActiveCount	=> activeCount;	// �ܺο��� ���� Ȱ��ȭ �Ǿ� �ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ

	public MemoryPool(GameObject poolObject, Transform poolParent=null)
	{
		maxCount		= 0;
		activeCount		= 0;
		this.poolObject	= poolObject;
		this.poolParent = poolParent;
		
		poolItemList	= new List<PoolItem>();
		
		InstantiateObjects();
	}

	/// <summary>
	/// increaseCount ������ ������Ʈ�� ����
	/// </summary>
	public void InstantiateObjects()
	{
		maxCount += increaseCount;

		for ( int i = 0; i < increaseCount; ++ i )
		{
			PoolItem poolItem = new PoolItem();

			poolItem.isActive = false;
			poolItem.gameObject	= GameObject.Instantiate(poolObject);
			poolItem.gameObject.SetActive(false);
			poolItem.gameObject.transform.SetParent(poolParent);

			poolItemList.Add(poolItem);
		}
	}

	/// <summary>
	/// ���� ��������(Ȱ��/��Ȱ��) ��� ������Ʈ�� ����
	/// </summary>
	public void DestroyObjects()
	{
		if ( poolItemList == null ) return;

		int count = poolItemList.Count;
		for ( int i = 0; i < count; ++ i )
		{
			GameObject.Destroy(poolItemList[i].gameObject);
		}

		poolItemList.Clear();
	}

	/// <summary>
	/// poolItemList�� ����Ǿ� �ִ� ������Ʈ�� Ȱ��ȭ�ؼ� ���
	/// ���� ��� ������Ʈ�� ������̸� InstantiateObjects()�� �߰� ����
	/// </summary>
	public GameObject ActivatePoolItem()
	{
		if ( poolItemList == null ) return null;

		// ���� �����ؼ� �����ϴ� ��� ������Ʈ ������ ���� Ȱ��ȭ ������ ������Ʈ ���� ��
		// ��� ������Ʈ�� Ȱ��ȭ �����̸� ���ο� ������Ʈ �ʿ�
		if ( maxCount == activeCount ) InstantiateObjects();

		int count = poolItemList.Count;
		for ( int i = 0; i < count; ++ i )
		{
			PoolItem poolItem = poolItemList[i];

			if ( poolItem.isActive == false )
			{
				activeCount ++;
				
				poolItem.isActive = true;
				poolItem.gameObject.SetActive(true);
				
				return poolItem.gameObject;
			}
		}

		return null;
	}

	/// <summary>
	/// ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ����
	/// </summary>
	public void DeactivatePoolItem(GameObject removeObject)
	{
		if ( poolItemList == null || removeObject == null ) return;

		int count = poolItemList.Count;
		for ( int i = 0; i < count; ++ i )
		{
			PoolItem poolItem = poolItemList[i];

			if ( poolItem.gameObject == removeObject )
			{
				activeCount --;

				poolItem.isActive = false;
				poolItem.gameObject.SetActive(false);

				return;
			}
		}
	}

	/// <summary>
	/// ���� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ �ϳ��� ��Ȱ��ȭ ���·� ����
	/// </summary>
	public GameObject DeactivatePoolItem()
	{
		if ( poolItemList == null ) return null;

		int count = poolItemList.Count;
		for ( int i = 0; i < count; ++ i )
		{
			PoolItem poolItem = poolItemList[i];

			if ( poolItem.gameObject.activeSelf == true )
			{
				activeCount --;

				poolItem.isActive = false;
				poolItem.gameObject.SetActive(false);

				return poolItem.gameObject;
			}
		}

		return null;
	}

	/// <summary>
	/// ���ӿ� ������� ��� ������Ʈ�� ��Ȱ��ȭ ���·� ����
	/// </summary>
	public void DeactivateAllPoolItems()
	{
		if ( poolItemList == null ) return;

		int count = poolItemList.Count;
		for ( int i = 0; i < count; ++ i )
		{
			PoolItem poolItem = poolItemList[i];

			if ( poolItem.gameObject != null && poolItem.isActive == true )
			{
				poolItem.isActive = false;
				poolItem.gameObject.SetActive(false);
			}
		}

		activeCount = 0;
	}
}

