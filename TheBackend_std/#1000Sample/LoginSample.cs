using UnityEngine;
using BackEnd;

public class LoginSample : MonoBehaviour
{
	private void Awake()
	{
		string ID		= "user01";
		string PW		= "1234";
		string email	= "user01@gmail.com";
		string nickname = "ù��°����";

		// ȸ������
		Backend.BMember.CustomSignUp(ID, PW);

		// �̸��� ����
		Backend.BMember.UpdateCustomEmail(email);

		// �α���
		Backend.BMember.CustomLogin(ID, PW);

		// ���̵� ã��
		Backend.BMember.FindCustomID(email);

		// ��й�ȣ ã��
		Backend.BMember.ResetPassword(ID, email);

		// �г��� ����
		// �г����� ���� �� ���� �г��� ����
		Backend.BMember.CreateNickname(nickname);
		// �̹� �ִ� �г����� ���� (���� �г����� ������ CreateNickname()�� ȣ��ȴ�.
		Backend.BMember.UpdateNickname(nickname);
	}
}

