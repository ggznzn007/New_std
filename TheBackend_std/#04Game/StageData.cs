using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
	// ���������� ũ��(�����¿� ����)�� limitMin, limitMax ������ ����
	[SerializeField]
	private	Vector2	limitMin;
	[SerializeField]
	private	Vector2	limitMax;

	// �ٸ� Ŭ���������� LimitMin, LimitMax ������Ƽ�� ����
	// ���������� ũ�⸦ Ȯ���� �� �ִ�.
	public	Vector2	LimitMin => limitMin;
	public	Vector2	LimitMax => limitMax;
}

