using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
	// ���� �����̱� ������ 1���� ����
	private static int m_iNextValidID = 0;

	// BaseGameEntity�� ��ӹ޴� ��� ���ӿ�����Ʈ�� ID ��ȣ�� �ο��޴µ�
	// �� ��ȣ�� 0���� �����ؼ� 1�� ���� (������ �ֹε�Ϲ�ȣó�� ���)
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

	private	string	entityName;			// ������Ʈ �̸�
	private	string	personalColor;      // ������Ʈ ���� (�ؽ�Ʈ ��¿�)

	/// <summary>
	/// �Ļ� Ŭ�������� base.Setup()���� ȣ��
	/// </summary>
	public virtual void Setup(string name)
	{
		// ���� ��ȣ ����
		ID = m_iNextValidID;
		// �̸� ����
		entityName = name;
		// ���� ���� ����
		int color = Random.Range(0, 1000000);
		personalColor = $"#{color.ToString("X6")}";
	}

	// GameController Ŭ�������� ��� ������Ʈ�� Updated()�� ȣ���� ������Ʈ�� �����Ѵ�.
	public abstract void Updated();

	/// <summary>
	/// Console View�� [�̸� : "���"] ���
	/// </summary>
	public void PrintText(string text)
	{
		Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
	}
}

