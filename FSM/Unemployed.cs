using UnityEngine;

public enum UnemployedStates { RestAndSleep = 0, PlayAGame, HitTheBottle, VisitBathroom, Global }

public class Unemployed : BaseGameEntity
{
	private	int					bored;				// 지루함
	private	int					stress;				// 스트레스
	private	int					fatigue;			// 피로
	private	Locations			currentLocation;	// 현재 위치

	// Unemployed가 가지고 있는 모든 상태, 상태 관리자
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
	
	public UnemployedStates CurrentState { private set; get; }	// 현재 상태

	public override void Setup(string name)
	{
		// 기반 클래스의 Setup 메소드 호출 (ID, 이름, 색상 설정)
		base.Setup(name);

		// 생성되는 오브젝트 이름 설정
		gameObject.name = $"{ID:D2}_Unemployed_{name}";

		// Unemployed가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
		states										= new State<Unemployed>[5];
		states[(int)UnemployedStates.RestAndSleep]	= new UnemployedOwnedStates.RestAndSleep();
		states[(int)UnemployedStates.PlayAGame]		= new UnemployedOwnedStates.PlayAGame();
		states[(int)UnemployedStates.HitTheBottle]	= new UnemployedOwnedStates.HitTheBottle();
		states[(int)UnemployedStates.VisitBathroom]	= new UnemployedOwnedStates.VisitBathroom();
		states[(int)UnemployedStates.Global]		= new UnemployedOwnedStates.StateGlobal();

		// 상태를 관리하는 StateMachine에 메모리를 할당하고, 첫 상태를 설정
		stateMachine = new StateMachine<Unemployed>();
		stateMachine.Setup(this, states[(int)UnemployedStates.RestAndSleep]);
		// 전역 상태 설정
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

