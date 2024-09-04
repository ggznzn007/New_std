using System.Collections.Generic;

public class EntityDatabase
{
	static	readonly	EntityDatabase	instance = new EntityDatabase();
	public	static		EntityDatabase	Instance => instance;

	// ��� ������Ʈ�� ������ ����Ǵ� �ڷᱸ��
	// <������Ʈ �̸�, BaseGameEntity Ÿ���� ������Ʈ ����>
	private	Dictionary<string, BaseGameEntity>	entityDictionary;

	public void Setup()
	{
		entityDictionary = new Dictionary<string, BaseGameEntity>();
	}

	/// <summary>
	/// ������Ʈ ���
	/// </summary>
	public void RegisterEntity(BaseGameEntity newEntity)
	{
		entityDictionary.Add(newEntity.EntityName, newEntity);
	}

	/// <summary>
	/// ������Ʈ �̸��� �������� ������Ʈ ���� �˻�(BaseGameEntity)
	/// </summary>
	public BaseGameEntity GetEntityFromID(string entityName)
	{
		foreach ( KeyValuePair<string, BaseGameEntity> entity in entityDictionary )
		{
			if ( entity.Key == entityName )
			{
				return entity.Value;
			}
		}
		return null;
	}

	/// <summary>
	/// ������Ʈ ����
	/// </summary>
	public void RemoveEntity(BaseGameEntity removeEntity)
	{
		entityDictionary.Remove(removeEntity.EntityName);
	}
}

/* using System.Linq;
 * public BaseGameEntity[] GetAllEntitys()
	{
		return entityDictionary.Values.ToArray();
	}
*/