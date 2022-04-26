using UnityEngine;

public enum UnemployedStates { RestAndSleep = 0, PlayAGame, HitTheBottle, VisitBathroom, Global }

public class Unemployed : BaseGameEntity
{
	private	int					bored;				// ������
	private	int					stress;				// ��Ʈ����
	private	int					fatigue;			// �Ƿ�
	private	Locations			currentLocation;	// ���� ��ġ

	// Unemployed�� ������ �ִ� ��� ����, ���� ������
	private	State<Unemployed>[]			states;
	private	StateMachine<Unemployed>	stateMachine;

	public int Bored
	{
		set => bored = Mathf.Max(0, value);
		get => bored;
	}
	public int Stress
	{
		set => stress = Mathf.Max(0, value);
		get => stress;
	}
	public int	Fatigue
	{
		set => fatigue = Mathf.Max(0, value);
		get => fatigue;
	}
	public Locations CurrentLocation
	{
		set => currentLocation = value;
		get => currentLocation;
	}
	
	public UnemployedStates CurrentState { private set; get; }	// ���� ����

	public override void Setup(string name)
	{
		// ��� Ŭ������ Setup �޼ҵ� ȣ�� (ID, �̸�, ���� ����)
		base.Setup(name);

		// �����Ǵ� ������Ʈ �̸� ����
		gameObject.name = $"{ID:D2}_Unemployed_{name}";

		// Unemployed�� ���� �� �ִ� ���� ������ŭ �޸� �Ҵ�, �� ���¿� Ŭ���� �޸� �Ҵ�
		states										= new State<Unemployed>[5];
		states[(int)UnemployedStates.RestAndSleep]	= new UnemployedOwnedStates.RestAndSleep();
		states[(int)UnemployedStates.PlayAGame]		= new UnemployedOwnedStates.PlayAGame();
		states[(int)UnemployedStates.HitTheBottle]	= new UnemployedOwnedStates.HitTheBottle();
		states[(int)UnemployedStates.VisitBathroom]	= new UnemployedOwnedStates.VisitBathroom();
		states[(int)UnemployedStates.Global]		= new UnemployedOwnedStates.StateGlobal();

		// ���¸� �����ϴ� StateMachine�� �޸𸮸� �Ҵ��ϰ�, ù ���¸� ����
		stateMachine = new StateMachine<Unemployed>();
		stateMachine.Setup(this, states[(int)UnemployedStates.RestAndSleep]);
		// ���� ���� ����
		stateMachine.SetGlobalState(states[(int)UnemployedStates.Global]);

		bored			= 0;
		stress			= 0;
		fatigue			= 0;
		currentLocation = Locations.SweetHome;
	}

	public override void Updated()
	{
		stateMachine.Execute();
	}

	public void ChangeState(UnemployedStates newState)
	{
		CurrentState = newState;

		stateMachine.ChangeState(states[(int)newState]);
	}

	public void RevertToPreviousState()
	{
		stateMachine.RevertToPreviousState();
	}
}

