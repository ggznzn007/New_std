public abstract class State<T> where T : BaseGameEntity //class
{
	/// <summary>
	/// �ش� ���¸� ������ �� 1ȸ ȣ��
	/// </summary>
	public abstract void Enter(T entity);

	/// <summary>
	/// �ش� ���¸� ������Ʈ�� �� �� ������ ȣ��
	/// </summary>
	public abstract void Execute(T entity);

	/// <summary>
	/// �ش� ���¸� ������ �� 1ȸ ȣ��
	/// </summary>
	public abstract void Exit(T entity);

	/// <summary>
	/// �޽����� �޾��� �� 1ȸ ȣ��
	/// </summary>
	public abstract bool OnMessage(T entity, Telegram telegram);
}

