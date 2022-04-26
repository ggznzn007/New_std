using UnityEngine;

namespace UnemployedOwnedStates
{
	public class RestAndSleep : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			// ��Ҹ� ������ �����ϰ�, �������� ��Ʈ����, �Ƿΰ� 0�� �ȴ�.
			entity.CurrentLocation	= Locations.SweetHome;
			entity.Stress			= 0;
			entity.Fatigue			= 0;

			entity.PrintText("���Ŀ� ���´�");
		}

		public override void Execute(Unemployed entity)
		{
			string state = Random.Range(0, 2) == 0 ? "zzZ~ zz~ zzzzZ~~" : "�߱��߱�.. TV�� ����";
			entity.PrintText(state);

			// �������� 70%�� Ȯ���� ����, 30%�� Ȯ���� ����
			entity.Bored += Random.Range(0, 100) < 70 ? 1 : -1;

			// �������� 4 �̻��̸�
			if ( entity.Bored >= 4 )
			{
				// PC�濡 ���� ������ �ϴ� "PlayAGame" ���·� ����
				entity.ChangeState(UnemployedStates.PlayAGame);
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("������ ���� �ʰ� �׳� ������.");
		}
	}

	public class PlayAGame : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.CurrentLocation = Locations.PCRoom;

			entity.PrintText("PC������ ����.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("������ ���������� �Ѵ�. �̱� ���� �̱��, �� ���� ���°ž�..");

			int randState = Random.Range(0, 10);
			if ( randState == 0 || randState == 9 )
			{
				entity.Stress += 20;
				
				// ������ ���� ���� ���ô� "HitTheBottle" ���·� ����
				entity.ChangeState(UnemployedStates.HitTheBottle);
			}
			else
			{
				entity.Bored --;
				entity.Fatigue += 2;

				if ( entity.Bored <= 0 || entity.Fatigue >= 50 )
				{
					// ���� ���� ���� "RestAndSleep" ���·� ����
					entity.ChangeState(UnemployedStates.RestAndSleep);
				}
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("�� ����.");
		}
	}

	public class HitTheBottle : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.CurrentLocation = Locations.Pub;

			entity.PrintText("�������� ����.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("���Ҷ����� ���� ���Ŵ�.");

			// �������� 50%�� Ȯ���� ����, 50%�� Ȯ���� ����
			entity.Bored += Random.Range(0, 2) == 0 ? 1 : -1;

			entity.Stress -= 4;
			entity.Fatigue += 4;

			if ( entity.Stress <= 0 || entity.Fatigue >= 50 )
			{
				// ���� ���� ���� "RestAndSleep" ���·� ����
				entity.ChangeState(UnemployedStates.RestAndSleep);
			}
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("�������� ���´�.");
		}
	}

	public class VisitBathroom : State<Unemployed>
	{
		public override void Enter(Unemployed entity)
		{
			entity.PrintText("ȭ��ǿ� ����.");
		}

		public override void Execute(Unemployed entity)
		{
			entity.PrintText("������ ����.");

			// �ٷ� ���� ���·� �ǵ��ư���.
			entity.RevertToPreviousState();
		}

		public override void Exit(Unemployed entity)
		{
			entity.PrintText("���� �İ� ȭ��ǿ��� ������.");
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

