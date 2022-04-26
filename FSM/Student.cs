using UnityEngine;

public enum StudentStates { RestAndSleep = 0, StudyHard, TakeAExam, PlayAGame, HitTheBottle }

public class Student : BaseGameEntity
{
	private	int			knowledge;			// ����
	private	int			stress;				// ��Ʈ����
	private	int			fatigue;			// �Ƿ�
	private	int			totalScore;			// ����
	private	Locations	currentLocation;	// ���� ��ġ

	// Student�� ������ �ִ� ��� ����, ���¸� �����ϴ� ���� Ŭ����
	private	State<Student>[]		states;
	private	StateMachine<Student>	stateMachine;

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
		set => totalScore = Mathf.Clamp(0, value, 100);
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
		states									= new State<Student>[5];
		states[(int)StudentStates.RestAndSleep]	= new StudentOwnedStates.RestAndSleep();
		states[(int)StudentStates.StudyHard]	= new StudentOwnedStates.StudyHard();
		states[(int)StudentStates.TakeAExam]	= new StudentOwnedStates.TakeAExam();
		states[(int)StudentStates.PlayAGame]	= new StudentOwnedStates.PlayAGame();
		states[(int)StudentStates.HitTheBottle]	= new StudentOwnedStates.HitTheBottle();

		// ���¸� �����ϴ� StateMachine�� �޸𸮸� �Ҵ��ϰ�, ù ���¸� ����
		stateMachine = new StateMachine<Student>();
		stateMachine.Setup(this, states[(int)StudentStates.RestAndSleep]);

		knowledge		= 0;
		stress			= 0;
		fatigue			= 0;
		totalScore		= 0;
		currentLocation = Locations.SweetHome;
	}

	public override void Updated()
	{
		stateMachine.Execute();
	}

	public void ChangeState(StudentStates newState)
	{
		stateMachine.ChangeState(states[(int)newState]);
	}
}

