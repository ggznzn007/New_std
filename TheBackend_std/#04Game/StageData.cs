using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
	// 스테이지의 크기(상하좌우 범위)를 limitMin, limitMax 변수에 저장
	[SerializeField]
	private	Vector2	limitMin;
	[SerializeField]
	private	Vector2	limitMax;

	// 다른 클래스에서는 LimitMin, LimitMax 프로퍼티를 통해
	// 스테이지의 크기를 확인할 수 있다.
	public	Vector2	LimitMin => limitMin;
	public	Vector2	LimitMax => limitMax;
}

