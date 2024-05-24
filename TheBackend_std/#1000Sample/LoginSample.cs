using UnityEngine;
using BackEnd;

public class LoginSample : MonoBehaviour
{
	private void Awake()
	{
		string ID		= "user01";
		string PW		= "1234";
		string email	= "user01@gmail.com";
		string nickname = "첫번째유저";

		// 회원가입
		Backend.BMember.CustomSignUp(ID, PW);

		// 이메일 설정
		Backend.BMember.UpdateCustomEmail(email);

		// 로그인
		Backend.BMember.CustomLogin(ID, PW);

		// 아이디 찾기
		Backend.BMember.FindCustomID(email);

		// 비밀번호 찾기
		Backend.BMember.ResetPassword(ID, email);

		// 닉네임 설정
		// 닉네임이 없을 때 최초 닉네임 설정
		Backend.BMember.CreateNickname(nickname);
		// 이미 있는 닉네임을 수정 (만약 닉네임이 없으면 CreateNickname()이 호출된다.
		Backend.BMember.UpdateNickname(nickname);
	}
}

