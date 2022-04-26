using System.Collections.Generic;
using UnityEngine;

public enum Locations { SweetHome = 0, Library, LectureRoom, PCRoom, Pub };

public class GameController : MonoBehaviour
{
	[SerializeField]
	private	string[]	arrayStudents;	// Student���� �̸� �迭
	[SerializeField]
	private	GameObject	studentPrefab;	// Student Ÿ���� ������
	
	// ��� ��� ���� ��� ������Ʈ ����Ʈ
	private	List<BaseGameEntity>	entitys;

	public static bool	IsGameStop { set; get; } = false;

	private void Awake()
	{
		entitys = new List<BaseGameEntity>();

		for ( int i = 0; i < arrayStudents.Length; ++ i )
		{
			// ������Ʈ ����, �ʱ�ȭ �޼ҵ� ȣ��
			GameObject	clone	= Instantiate(studentPrefab);
			Student		entity	= clone.GetComponent<Student>();
			entity.Setup(arrayStudents[i]);

			// ������Ʈ���� ��� ��� ���� ����Ʈ�� ����
			entitys.Add(entity);
		}
	}

	private void Update()
	{
		if ( IsGameStop == true ) return;

		// ��� ������Ʈ�� Updated()�� ȣ���� ������Ʈ ����
		for ( int i = 0; i < entitys.Count; ++ i )
		{
			entitys[i].Updated();
		}
	}

	public static void Stop(BaseGameEntity entity)
	{
		IsGameStop = true;

		entity.PrintText("100�� ȹ������ ���α׷��� �����մϴ�.");
	}
}

