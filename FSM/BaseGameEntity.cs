using UnityEngine; // �ϳ��� ĳ���͸� ���� FSM

public abstract class BaseGameEntity : MonoBehaviour
{
    // ���� �����̱� ������ 1���� ����
    private static int m_iNextValidID = 0;

    // �� ����Ŭ������ ��ӹ޴� ��� ������Ʈ�� ID��ȣ�� �ο��޴µ�
    // 0���� �����ؼ� 1�� ���� ( ������ �ֹε�Ϲ�ȣ ó�� ���)
    private int id;
    public int ID
    {
        set
        {
            id = value;
            m_iNextValidID++;
        }
        get => id;
    }

    private string entityName; // ĳ���� �̸�
    private string personalColor; // ĳ���ͻ���(��¿�)

    // �Ļ�Ŭ�������� base.Setup()���� ȣ��
    public virtual void Setup(string name)
    {
        // ������ȣ ����
        ID = m_iNextValidID;
        // �̸� ����
        entityName = name;
        // ���� ����
        int color = Random.Range(0, 1000000);
        personalColor = $"#{color.ToString("X6")}";
    }

    // ������Ʈ�ѷ� Ŭ�������� ��� ĳ������ Updated()�� ȣ���� ĳ���� ����
    public abstract void Updated();

    // �ֿܼ� �̸��� ��� ���
    public void PrintText(string text)
    {
        Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
    }
}
