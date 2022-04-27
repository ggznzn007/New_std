using UnityEngine;

public enum StudentStates { RestAndSleep = 0, StudyHard, TakeAExam, PlayAGame, HitTheBottle, GMessageReceive }

public class Student : BaseGameEntity
{
	private	int				knowledge;			// 지식
	private	int				stress;				// 스트레스
	private	int				fatigue;			// 피로
	private	int				totalScore;			// 점수
	private	Locations		currentLocation;	// 현재 위치
	private	StudentStates	currentState;		// 현재 상태

	// Student가 가지고 있는 모든 상태, 상태를 관리하는 참조 클래스
	private	State<Student>[]		states;
	private	StateMachine<Student>	stateMachine;

	public	StudentStates	CurrentState => currentState;

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
		// 기반 클래스의 Setup 메소드 호출 (ID, 이름, 색상 설정)
		base.Setup(name);

		// 생성되는 오브젝트 이름 설정
		gameObject.name = $"{ID:D2}_Student_{name}";

		// Student가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
		states										= new State<Student>[6];
		states[(int)StudentStates.RestAndSleep]		= new StudentOwnedStates.RestAndSleep();
		states[(int)StudentStates.StudyHard]		= new StudentOwnedStates.StudyHard();
		states[(int)StudentStates.TakeAExam]		= new StudentOwnedStates.TakeAExam();
		states[(int)StudentStates.PlayAGame]		= new StudentOwnedStates.PlayAGame();
		states[(int)StudentStates.HitTheBottle]		= new StudentOwnedStates.HitTheBottle();
		states[(int)StudentStates.GMessageReceive]	= new StudentOwnedStates.GlobalMessageReceive();

		// 상태를 관리하는 StateMachine에 메모리를 할당하고, 첫 상태를 설정
		stateMachine = new StateMachine<Student>();
		stateMachine.Setup(this, states[(int)StudentStates.RestAndSleep]);
		// 전역 상태 설정
		stateMachine.SetGlobalState(states[(int)StudentStates.GMessageReceive]);

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
		currentState = newState;

		stateMachine.ChangeState(states[(int)newState]);
	}

	public override bool HandleMessage(Telegram telegram)
	{
		return stateMachine.HandleMessage(telegram);
	}
}

