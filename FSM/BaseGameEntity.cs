using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
	// 정적 변수이기 때문에 1개만 존재
	private static int m_iNextValidID = 0;

	// BaseGameEntity를 상속받는 모든 게임오브젝트는 ID 번호를 부여받는데
	// 이 번호는 0부터 시작해서 1씩 증가 (현실의 주민등록번호처럼 사용)
	private	int	id;
	public	int	ID
	{
		set
		{
			id = value;
			m_iNextValidID ++;
		}
		get => id;
	}

	private	string	entityName;			// 에이전트 이름
	private	string	personalColor;      // 에이전트 색상 (텍스트 출력용)

	/// <summary>
	/// 파생 클래스에서 base.Setup()으로 호출
	/// </summary>
	public virtual void Setup(string name)
	{
		// 고유 번호 설정
		ID = m_iNextValidID;
		// 이름 설정
		entityName = name;
		// 고유 색상 설정
		int color = Random.Range(0, 1000000);
		personalColor = $"#{color.ToString("X6")}";
	}

	// GameController 클래스에서 모든 에이전트의 Updated()를 호출해 에이전트를 구동한다.
	public abstract void Updated();

	/// <summary>
	/// Console View에 [이름 : "대사"] 출력
	/// </summary>
	public void PrintText(string text)
	{
		Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
	}
}

