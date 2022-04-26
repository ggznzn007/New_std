using UnityEngine;

namespace UnemployedOwnedStates
{
	public class RestAndSleep : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			// 장소를 집으로 설정하고, 집에오면 스트레스, 피로가 0이 된다.
			entity.CurrentLocation	= Locations.SweetHome;
			entity.Stress			= 0;
			entity.Fatigue			= 0;

			entity.PrintText("소파에 눕는다");
		}

		public override void Execute(Unemployed entity)
		{
			string state = Random.Range(0, 2) == 0 ? "zzZ~ zz~ zzzzZ~~" : "뒹굴뒹굴.. TV를 본다";
			entity.PrintText(state);

			// 지루함은 70%의 확률로 증가, 30%의 확률로 감소
			entity.Bored += Random.Range(0, 100) < 70 ? 1 : -1;

			// 지루함이 4 이상이면
			if ( entity.Bored >= 4 )
			{
				// PC방에 가서 게임을 하는 "PlayAGame" 상태로 변경
				entity.ChangeState(UnemployedStates.PlayAGame);
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("정리를 하지 않고 그냥 나간다.");
		}
	}

	public class PlayAGame : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.CurrentLocation = Locations.PCRoom;

			entity.PrintText("PC방으로 들어간다.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("게임을 열정적으로 한다. 이길 판은 이기고, 질 판은 지는거야..");

			int randState = Random.Range(0, 10);
			if ( randState == 0 || randState == 9 )
			{
				entity.Stress += 20;
				
				// 술집에 가서 술을 마시는 "HitTheBottle" 상태로 변경
				entity.ChangeState(UnemployedStates.HitTheBottle);
			}
			else
			{
				entity.Bored --;
				entity.Fatigue += 2;

				if ( entity.Bored <= 0 || entity.Fatigue >= 50 )
				{
					// 집에 가서 쉬는 "RestAndSleep" 상태로 변경
					entity.ChangeState(UnemployedStates.RestAndSleep);
				}
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("잘 즐겼다.");
		}
	}

	public class HitTheBottle : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.CurrentLocation = Locations.Pub;

			entity.PrintText("술집으로 들어간다.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("취할때까지 술을 마신다.");

			// 지루함은 50%의 확률로 증가, 50%의 확률로 감소
			entity.Bored += Random.Range(0, 2) == 0 ? 1 : -1;

			entity.Stress -= 4;
			entity.Fatigue += 4;

			if ( entity.Stress <= 0 || entity.Fatigue >= 50 )
			{
				// 집에 가서 쉬는 "RestAndSleep" 상태로 변경
				entity.ChangeState(UnemployedStates.RestAndSleep);
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("술집에서 나온다.");
		}
	}

	public class VisitBathroom : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.PrintText("화장실에 들어간다.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("볼일을 본다.");

			// 바로 직전 상태로 되돌아간다.
			entity.RevertToPreviousState();
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("손을 씻고 화장실에서 나간다.");
		}
	}

	public class StateGlobal : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
		}

		public override void Execute(Unemployed entity)
		{
			if ( entity.CurrentState == UnemployedStates.VisitBathroom )
			{
				return;
			}

			int bathroomState = Random.Range(0, 100);
			if ( bathroomState < 10 )
			{
				entity.ChangeState(UnemployedStates.VisitBathroom);
			}
		}

		public override void Exit(Unemployed entity)
		{
		}
	}
}

