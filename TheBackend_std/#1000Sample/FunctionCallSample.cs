using UnityEngine;
using BackEnd;

public class FunctionCallSample : MonoBehaviour
{
	private void Awake()
	{
		string ID = "user01";
		string PW = "1234";

		// 동기
		var bro = Backend.BMember.CustomLogin(ID, PW);
		// 로그인에 성공했을 때 처리
		if ( bro.IsSuccess() )
		{
		}
		// 로그인에 실패했을 때 처리
		else
		{
			// statusCode의 값에 따라 실패 원인을 알 수 있다.
			int statusCode = int.Parse(bro.GetStatusCode());
		}

		// 비동기 - 콜백 풀링 함수
		// 별도의 큐에 저장된 콜백 함수를 실행하려면 AsnycPoll() 메소드 호출이 필요하다.
		// https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
		Backend.BMember.CustomLogin(ID, PW, callback =>
		{
			// 로그인에 성공했을 때
			if ( callback.IsSuccess() )
			{
			}
			// 로그인에 실패했을 때
			else
			{
				// statusCode의 값에 따라 실패 원인을 알 수 있다.
				int statusCode = int.Parse(callback.GetStatusCode());
			}
		});

		// SendQueue
		// SendQueue의 Enqueue()에 호출할 메소드를 등록하고 별도의 시작 처리가 필요하다.
		// https://developer.thebackend.io/unity3d/guide/Async/SendQueueDetail/
		SendQueue.Enqueue(Backend.BMember.CustomLogin, ID, PW, callback =>
		{
			// 로그인에 성공했을 때
			if ( callback.IsSuccess() )
			{
			}
			// 로그인에 실패했을 때
			else
			{
				// statusCode의 값에 따라 실패 원인을 알 수 있다.
				int statusCode = int.Parse(callback.GetStatusCode());
			}
		});
	}
}

