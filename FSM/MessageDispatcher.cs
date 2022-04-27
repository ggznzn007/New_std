using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher
{
	static	readonly	MessageDispatcher	instance = new MessageDispatcher();
	public	static		MessageDispatcher	Instance => instance;

	// 지연 발송되어야 하는 메시지 관리
	private	SortedDictionary<float, Telegram>	prioritySD;

	public void Setup()
	{
		prioritySD = new SortedDictionary<float, Telegram>();
	}

	/// <summary>
	/// 에이전트에게 메시지를 보낼 때 호출하는 메소드 (발송시간, 발신자, 수신자, 메시지 정보)
	/// </summary>
	public void DispatchMessage(float delayTime, string senderName, string receiverName, string message)
	{
		// 수신자 정보 검색
		BaseGameEntity receiver = EntityDatabase.Instance.GetEntityFromID(receiverName);
		// 존재하지 않는 에이전트이면 경고문 출력
		if ( receiver == null )
		{
			Debug.Log($"<color=red>Warning! No Receiver with ID of <b><i>{receiverName}</i></b> found</color>");
			return;
		}

		// 전보 데이터 생성
		Telegram telegram = new Telegram();
		telegram.SetTelegram(0, senderName, receiverName, message);

		// 지연 시간이 없는 메시지는 바로 보냄
		if ( delayTime <= 0 )
		{
			Discharge(receiver, telegram);
		}
		// 지연시간이 있는 메시지는 보내야 할 시간을 기록하여 prioritySD에 저장
		else
		{
			// 현재시간으로 부터 얼마 뒤에 가능한지 시간 계산
			telegram.dispatchTime = Time.time + delayTime;
			// SortedDictionary에 저장하여 관리
			prioritySD.Add(telegram.dispatchTime, telegram);
		}
	}

	/// <summary>
	/// 수신자에게 메시지를 발송
	/// </summary>
	private void Discharge(BaseGameEntity receiver, Telegram telegram)
	{
		receiver.HandleMessage(telegram);
	}

	/// <summary>
	/// 지연 발송되는 메시지 관리 (게임의 업데이트 메소드에서 호출)
	/// </summary>
	public void DispatchDelayedMessages()
	{
		// 현재 대기 중인 메시지 중에 보낼 시간이 된 메시지를 발송함
		foreach ( KeyValuePair<float, Telegram> entity in prioritySD )
		{
			if ( entity.Key <= Time.time )
			{
				BaseGameEntity receiver = EntityDatabase.Instance.GetEntityFromID(entity.Value.receiver);

				Discharge(receiver, entity.Value);	// receiver에게 전보 전송
				prioritySD.Remove(entity.Key);		// 우선순위 Dictionary 자료구조에서 방금 보낸 전보 삭제
				
				return;
			}
		}
	}
}

