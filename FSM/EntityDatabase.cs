using System.Collections.Generic;

public class EntityDatabase
{
	static	readonly	EntityDatabase	instance = new EntityDatabase();
	public	static		EntityDatabase	Instance => instance;

	// 모든 에이전트의 정보가 저장되는 자료구조
	// <에이전트 이름, BaseGameEntity 타입의 에이전트 정보>
	private	Dictionary<string, BaseGameEntity>	entityDictionary;

	public void Setup()
	{
		entityDictionary = new Dictionary<string, BaseGameEntity>();
	}

	/// <summary>
	/// 에이전트 등록
	/// </summary>
	public void RegisterEntity(BaseGameEntity newEntity)
	{
		entityDictionary.Add(newEntity.EntityName, newEntity);
	}

	/// <summary>
	/// 에이전트 이름을 기준으로 에이전트 정보 검색(BaseGameEntity)
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
	/// 에이전트 삭제
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