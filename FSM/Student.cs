using UnityEngine;

public enum StudentStates { RestAndSleep = 0, StudyHard, TakeAExam, PlayAGame, HitTheBottle }

public class Student : BaseGameEntity
{
	private	int			knowledge;			// ����
	private	int			stress;				// ��Ʈ����
	private	int			fatigue;			// �Ƿ�
	private	int			totalScore;			// ����
	private	Locations	currentLocation;	// ���� ��ġ

	// Student�� ������ �ִ� ��� ����, ���� ����
	private	State[]		states;
	private	State		currentState;

	public int Knowledge
	{
		set => knowledge = Mathf.Max(0, value);
		get => knowledge;
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
	public int TotalScore
	{
		set => totalScore = Mathf.Clamp(value, 0, 100);
		get => totalScore;
	}
	public Locations CurrentLocation
	{
		set => currentLocation = value;
		get => currentLocation;
	}

	public override void Setup(string name)
	{
		// ��� Ŭ������ Setup �޼ҵ� ȣ�� (ID, �̸�, ���� ����)
		base.Setup(name);

		// �����Ǵ� ������Ʈ �̸� ����
		gameObject.name = $"{ID:D2}_Student_{name}";

		// Student�� ���� �� �ִ� ���� ������ŭ �޸� �Ҵ�, �� ���¿� Ŭ���� �޸� �Ҵ�
		states									= new State[5];
		states[(int)StudentStates.RestAndSleep]	= new StudentOwnedStates.RestAndSleep();
		states[(int)StudentStates.StudyHard]	= new StudentOwnedStates.StudyHard();
		states[(int)StudentStates.TakeAExam]	= new StudentOwnedStates.TakeAExam();
		states[(int)StudentStates.PlayAGame]	= new StudentOwnedStates.PlayAGame();
		states[(int)StudentStates.HitTheBottle]	= new StudentOwnedStates.HitTheBottle();

		// ���� ���¸� ������ ���� "RestAndSleep" ���·� ����
		ChangeState(StudentStates.RestAndSleep);

		knowledge		= 0;
		stress			= 0;
		fatigue			= 0;
		totalScore		= 0;
		currentLocation = Locations.SweetHome;
	}

	public override void Updated()
	{
		if ( currentState != null )
		{
			currentState.Execute(this);
		}
	}

	public void ChangeState(StudentStates newState)
	{
		// ���� �ٲٷ��� ���°� ��������� ���¸� �ٲ��� �ʴ´�
		if ( states[(int)newState] == null ) return;

		// ���� ������� ���°� ������ Exit() �޼ҵ� ȣ��
		if ( currentState != null )
		{
			currentState.Exit(this);
		}

		// ���ο� ���·� �����ϰ�, ���� �ٲ� ������ Enter() �޼ҵ� ȣ��
		currentState = states[(int)newState];
		currentState.Enter(this);
	}
}

