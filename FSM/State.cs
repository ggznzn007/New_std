public abstract class State
{
	/// <summary>
	/// �ش� ���¸� ������ �� 1ȸ ȣ��
	/// </summary>
	public abstract void Enter(Student entity);

	/// <summary>
	/// �ش� ���¸� ������Ʈ�� �� �� ������ ȣ��
	/// </summary>
	public abstract void Execute(Student entity);

	/// <summary>
	/// �ش� ���¸� ������ �� 1ȸ ȣ��
	/// </summary>
	public abstract void Exit(Student entity);
}

