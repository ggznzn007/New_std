using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Nickname : LoginBase
{
	[System.Serializable]
	public class NicknameEvent : UnityEngine.Events.UnityEvent { }
	public NicknameEvent		onNicknameEvent = new NicknameEvent();

	[SerializeField]
	private	Image				imageNickname;		// 닉네임 필드 색상 변경
	[SerializeField]
	private	TMP_InputField		inputFieldNickname;	// 닉네임 필드 텍스트 정보 추출

	[SerializeField]
	private	Button				btnUpdateNickname;  // "닉네임 설정" 버튼 (상호작용 가능/불가능)

	private void OnEnable()
	{
		// 닉네임 변경에 실패해 에러 메시지를 출력한 상태에서
		// 닉네임 변경 팝업을 닫았다가 열 수 있기 때문에 상태를 초기화
		ResetUI(imageNickname);
		SetMessage("닉네임을 입력하세요");
	}

	public void OnClickUpdateNickname()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageNickname);

		// 필드 값이 비어있는지 체크
		if ( IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname") )	return;

		// "닉네임 변경" 버튼의 상호작용 비활성화
		btnUpdateNickname.interactable = false;
		SetMessage("닉네임 변경중입니다..");

		// 뒤끝 서버 닉네임 변경 시도
		UpdateNickname();
	}

	private void UpdateNickname()
	{
		// 닉네임 설정
		Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
		{
			// "닉네임 변경" 버튼의 상호작용 활성화
			btnUpdateNickname.interactable = true;

			// 닉네임 변경 성공
			if ( callback.IsSuccess() )
			{
				SetMessage($"{inputFieldNickname.text}(으)로 닉네임이 변경되었습니다.");

				// 닉네임 변경에 성공했을 때 onNicknameEvent에 등록되어 있는 이벤트 메소드 호출
				onNicknameEvent?.Invoke();
			}
			// 닉네임 변경 실패
			else
			{
				string message = string.Empty;

				switch ( int.Parse(callback.GetStatusCode()) )
				{
					case 400:	// 빈 닉네임 혹은 string.Empty, 20자 이상의 닉네임, 닉네임 앞/뒤에 공백이 있는 경우
						message = "닉네임이 비어있거나 | 20자 이상 이거나 | 앞/뒤에 공백이 있습니다.";
						break;
					case 409:	// 이미 중복된 닉네임이 있는 경우
						message = "이미 존재하는 닉네임입니다.";
						break;
					default:
						message = callback.GetMessage();
						break;
				}

				GuideForIncorrectlyEnteredData(imageNickname, message);
			}
		});
	}
}

