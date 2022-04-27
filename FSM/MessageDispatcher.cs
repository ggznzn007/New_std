using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher
{
	static	readonly	MessageDispatcher	instance = new MessageDispatcher();
	public	static		MessageDispatcher	Instance => instance;

	// ���� �߼۵Ǿ�� �ϴ� �޽��� ����
	private	SortedDictionary<float, Telegram>	prioritySD;

	public void Setup()
	{
		prioritySD = new SortedDictionary<float, Telegram>();
	}

	/// <summary>
	/// ������Ʈ���� �޽����� ���� �� ȣ���ϴ� �޼ҵ� (�߼۽ð�, �߽���, ������, �޽��� ����)
	/// </summary>
	public void DispatchMessage(float delayTime, string senderName, string receiverName, string message)
	{
		// ������ ���� �˻�
		BaseGameEntity receiver = EntityDatabase.Instance.GetEntityFromID(receiverName);
		// �������� �ʴ� ������Ʈ�̸� ��� ���
		if ( receiver == null )
		{
			Debug.Log($"<color=red>Warning! No Receiver with ID of <b><i>{receiverName}</i></b> found</color>");
			return;
		}

		// ���� ������ ����
		Telegram telegram = new Telegram();
		telegram.SetTelegram(0, senderName, receiverName, message);

		// ���� �ð��� ���� �޽����� �ٷ� ����
		if ( delayTime <= 0 )
		{
			Discharge(receiver, telegram);
		}
		// �����ð��� �ִ� �޽����� ������ �� �ð��� ����Ͽ� prioritySD�� ����
		else
		{
			// ����ð����� ���� �� �ڿ� �������� �ð� ���
			telegram.dispatchTime = Time.time + delayTime;
			// SortedDictionary�� �����Ͽ� ����
			prioritySD.Add(telegram.dispatchTime, telegram);
		}
	}

	/// <summary>
	/// �����ڿ��� �޽����� �߼�
	/// </summary>
	private void Discharge(BaseGameEntity receiver, Telegram telegram)
	{
		receiver.HandleMessage(telegram);
	}

	/// <summary>
	/// ���� �߼۵Ǵ� �޽��� ���� (������ ������Ʈ �޼ҵ忡�� ȣ��)
	/// </summary>
	public void DispatchDelayedMessages()
	{
		// ���� ��� ���� �޽��� �߿� ���� �ð��� �� �޽����� �߼���
		foreach ( KeyValuePair<float, Telegram> entity in prioritySD )
		{
			if ( entity.Key <= Time.time )
			{
				BaseGameEntity receiver = EntityDatabase.Instance.GetEntityFromID(entity.Value.receiver);

				Discharge(receiver, entity.Value);	// receiver���� ���� ����
				prioritySD.Remove(entity.Key);		// �켱���� Dictionary �ڷᱸ������ ��� ���� ���� ����
				
				return;
			}
		}
	}
}

