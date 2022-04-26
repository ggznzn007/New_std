public class StateMachine<T> where T : class
{
	private	T			ownerEntity;	// StateMachine�� ������
	private	State<T>	currentState;	// ���� ����
	private	State<T>	previousState;	// ���� ����
	private	State<T>	globalState;	// ���� ����

	public void Setup(T owner, State<T> entryState)
	{
		ownerEntity		= owner;
		currentState	= null;
		previousState	= null;
		globalState		= null;

		// entryState ���·� ���� ����
		ChangeState(entryState);
	}

	public void Execute()
	{
		if ( globalState != null )
		{
			globalState.Execute(ownerEntity);
		}

		if ( currentState != null )
		{
			currentState.Execute(ownerEntity);
		}
	}

	public void ChangeState(State<T> newState)
	{
		// ���� �ٲٷ��� ���°� ��������� ���¸� �ٲ��� �ʴ´�
		if ( newState == null ) return;

		// ���� ������� ���°� ������ Exit() �޼ҵ� ȣ��
		if ( currentState != null )
		{
			// ���°� ����Ǹ� ���� ���´� ���� ���°� �Ǳ� ������ previousState�� ����
			previousState = currentState;

			currentState.Exit(ownerEntity);
		}

		// ���ο� ���·� �����ϰ�, ���� �ٲ� ������ Enter() �޼ҵ� ȣ��
		currentState = newState;
		currentState.Enter(ownerEntity);
	}

	public void SetGlobalState(State<T> newState)
	{
		globalState = newState;
	}

	public void RevertToPreviousState()
	{
		ChangeState(previousState);
	}
}

