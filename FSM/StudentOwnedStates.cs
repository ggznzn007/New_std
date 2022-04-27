using UnityEngine;

namespace StudentOwnedStates
{
	public class RestAndSleep : State<Student>
	{
		public override void Enter(Student entity)
		{
			// ��Ҹ� ������ �����ϰ�, �������� ��Ʈ������ 0�� �ȴ�.
			entity.CurrentLocation	= Locations.SweetHome;
			entity.Stress			= 0;

			entity.PrintText("���� ���´�. �ູ�� �츮��~ ���� ���� ��Ʈ������ �������.");
			entity.PrintText("ħ�뿡 ���� ���� �ܴ�.");
		}

		public override void Execute(Student entity)
		{
			entity.PrintText("zzZ~ zz~ zzzzZ~~");

			// �Ƿΰ� 0�� �ƴϸ�
			if ( entity.Fatigue > 0 )
			{
				// �Ƿ� 10�� ����
				entity.Fatigue -= 10;
			}
			// �Ƿΰ� 0�̸�
			else
			{
				// �������� ���� ���θ� �ϴ� "StudyHard" ���·� ����
				entity.ChangeState(StudentStates.StudyHard);
			}
		}

		public override void Exit(Student entity)
		{
			entity.PrintText("ħ�븦 �����ϰ� �� ������ ������.");
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			return false;
		}
	}

	public class StudyHard : State<Student>
	{
		public override void Enter(Student entity)
		{
			entity.CurrentLocation = Locations.Library;

			entity.PrintText("���θ� �ϱ� ���� �������� �Դ�.. ����..��.. ���θ� ����");
		}

		public override void Execute(Student entity)
		{
			entity.PrintText("���� ���� ���� ����...");

			// ����, ��Ʈ����, �Ƿΰ� 1�� ����
			entity.Knowledge ++;
			entity.Stress ++;
			entity.Fatigue ++;

			// ������ 3~10 ���̰� �Ǹ� 
			if ( entity.Knowledge >= 3 && entity.Knowledge <= 10 )
			{
				int isExit = Random.Range(0, 2);
				if ( isExit == 1 || entity.Knowledge == 10 )
				{
					// ���ǽǿ� ���� ������ ���� "TakeAExam" ���·� ����
					entity.ChangeState(StudentStates.TakeAExam);
				}
			}

			// ��Ʈ������ 20 �̻��� �Ǹ�
			if ( entity.Stress >= 20 )
			{
				// PC�濡 ���� ������ �ϴ� "PlayAGame" ���·� ����
				entity.ChangeState(StudentStates.PlayAGame);
			}

			// �Ƿΰ� 50 �̻��� �Ǹ�
			if ( entity.Fatigue >= 50 )
			{
				// ���� ���� ���� "RestAndSleep" ���·� ����
				entity.ChangeState(StudentStates.RestAndSleep);
			}
		}

		public override void Exit(Student entity)
		{
			entity.PrintText("�ڸ��� �����ϰ� �������� ������.");
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			return false;
		}
	}

	public class TakeAExam : State<Student>
	{
		public override void Enter(Student entity)
		{
			entity.CurrentLocation = Locations.LectureRoom;

			entity.PrintText("���ǽǿ� ����. �������� �޾Ҵ�.");
		}

		public override void Execute(Student entity)
		{
			int examScore = 0;

			// ������ 10�̸� ȹ�������� 10
			if ( entity.Knowledge == 10 )
			{
				examScore = 10;
			}
			else
			{
				// randIndex�� ���� ��ġ���� ������ 6~10��, ���� ��ġ���� ������ 1~5��
				// ��, ������ �������� ���� ������ ���� Ȯ���� ����
				int randIndex = Random.Range(0, 10);
				examScore = randIndex < entity.Knowledge ? Random.Range(6, 11) : Random.Range(1, 6);
			}

			// ���� ���� ������ 0���� �ʱ�ȭ, �Ƿδ� 5 ~ 10 ����
			entity.Knowledge = 0;
			entity.Fatigue += Random.Range(5, 11);

			// ���迡�� ȹ���� ������ TotalScore�� �߰��ϰ�, ��� ���
			entity.TotalScore += examScore;
			entity.PrintText($"���� ����({examScore}), ����({entity.TotalScore})");

			if ( entity.TotalScore >= 100 )
			{
				GameController.Stop(entity);
				return;
			}

			// ���� ������ ���� ���� �ൿ ����
			if ( examScore <= 3 )
			{
				// ������ ���� ���� ���ô� "HitTheBottle" ���·� ����
				entity.ChangeState(StudentStates.HitTheBottle);
			}
			else if ( examScore <= 7 )
			{
				// �������� ���� ���θ� �ϴ� "StudyHard" ���·� ����
				entity.ChangeState(StudentStates.StudyHard);
			}
			else
			{
				// PC�濡 ���� ������ �ϴ� "PlayAGame" ���·� ����
				entity.ChangeState(StudentStates.PlayAGame);
			}
		}

		public override void Exit(Student entity)
		{
			entity.PrintText("���ǽ� ���� ���� ������ ���´�.");
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			return false;
		}
	}

	public class PlayAGame : State<Student>
	{
		public override void Enter(Student entity)
		{
			entity.CurrentLocation = Locations.PCRoom;

			entity.PrintText("�ѽð���.. �� �ѽð��� ��ƾ���.. PC������ ����.");
		}

		public override void Execute(Student entity)
		{
			entity.PrintText("�����ϰ�?? ������ ����..");

			int randState = Random.Range(0, 10);
			if ( randState == 0 || randState == 9 )
			{
				entity.Stress += 20;
				
				// ������ ���� ���� ���ô� "HitTheBottle" ���·� ����
				entity.ChangeState(StudentStates.HitTheBottle);
			}
			else
			{
				entity.Stress --;
				entity.Fatigue += 2;

				if ( entity.Stress <= 0 )
				{
					// �������� ���� ���θ� �ϴ� "StudyHard" ���·� ����
					entity.ChangeState(StudentStates.StudyHard);
				}
			}
		}

		public override void Exit(Student entity)
		{
			entity.PrintText("PC�濡�� ���´�.");
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			return false;
		}
	}

	public class HitTheBottle : State<Student>
	{
		public override void Enter(Student entity)
		{
			entity.CurrentLocation = Locations.Pub;

			entity.PrintText("���̳� �����ұ�? �������� ����.");
		}

		public override void Execute(Student entity)
		{
			entity.PrintText("���� ���Ŵ�.");

			entity.Stress -= 5;
			entity.Fatigue += 5;

			if ( entity.Stress <= 0 || entity.Fatigue >= 50 )
			{
				// ���� ���� ���� "RestAndSleep" ���·� ����
				entity.ChangeState(StudentStates.RestAndSleep);
			}
		}

		public override void Exit(Student entity)
		{
			entity.PrintText("�׸� ���ž���.. �������� ���´�.");
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			return false;
		}
	}

	public class GlobalMessageReceive : State<Student>
	{
		public override void Enter(Student entity)
		{
		}

		public override void Execute(Student entity)
		{
		}

		public override void Exit(Student entity)
		{
		}

		public override bool OnMessage(Student entity, Telegram telegram)
		{
			entity.PrintText($"Receive Message : sender({telegram.sender}), receiver({telegram.receiver})");

			if ( telegram.message.Equals("GO_PCROOM") )
			{
				if ( entity.CurrentState == StudentStates.PlayAGame )
				{
					entity.PrintText($"Send Message {telegram.receiver}���� {telegram.sender}�Կ��� ALREADY_PCROOM ����");
					MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "ALREADY_PCROOM");
				}
				else
				{
					int stateIndex = Random.Range(0, 2);
					if ( stateIndex == 0 )
					{
						entity.PrintText($"Send Message {telegram.receiver}���� {telegram.sender}�Կ��� GO_PCROOM ����");
						MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "GO_PCROOM");
					
						// PC�濡 ���� ������ �ϴ� "PlayAGame" ���·� ����
						entity.ChangeState(StudentStates.PlayAGame);
					}
					else
					{
						entity.PrintText($"Send Message {telegram.receiver}���� {telegram.sender}�Կ��� SORRY ����");
						MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "SORRY");
					}
				}

				return true;
			}

			return false;
		}
	}
}

