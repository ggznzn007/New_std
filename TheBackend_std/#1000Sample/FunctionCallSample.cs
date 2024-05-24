using UnityEngine;
using BackEnd;

public class FunctionCallSample : MonoBehaviour
{
	private void Awake()
	{
		string ID = "user01";
		string PW = "1234";

		// ����
		var bro = Backend.BMember.CustomLogin(ID, PW);
		// �α��ο� �������� �� ó��
		if ( bro.IsSuccess() )
		{
		}
		// �α��ο� �������� �� ó��
		else
		{
			// statusCode�� ���� ���� ���� ������ �� �� �ִ�.
			int statusCode = int.Parse(bro.GetStatusCode());
		}

		// �񵿱� - �ݹ� Ǯ�� �Լ�
		// ������ ť�� ����� �ݹ� �Լ��� �����Ϸ��� AsnycPoll() �޼ҵ� ȣ���� �ʿ��ϴ�.
		// https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
		Backend.BMember.CustomLogin(ID, PW, callback =>
		{
			// �α��ο� �������� ��
			if ( callback.IsSuccess() )
			{
			}
			// �α��ο� �������� ��
			else
			{
				// statusCode�� ���� ���� ���� ������ �� �� �ִ�.
				int statusCode = int.Parse(callback.GetStatusCode());
			}
		});

		// SendQueue
		// SendQueue�� Enqueue()�� ȣ���� �޼ҵ带 ����ϰ� ������ ���� ó���� �ʿ��ϴ�.
		// https://developer.thebackend.io/unity3d/guide/Async/SendQueueDetail/
		SendQueue.Enqueue(Backend.BMember.CustomLogin, ID, PW, callback =>
		{
			// �α��ο� �������� ��
			if ( callback.IsSuccess() )
			{
			}
			// �α��ο� �������� ��
			else
			{
				// statusCode�� ���� ���� ���� ������ �� �� �ִ�.
				int statusCode = int.Parse(callback.GetStatusCode());
			}
		});
	}
}

