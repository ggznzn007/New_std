using UnityEngine;
using BackEnd;
using LitJson;

public class UserInfo : MonoBehaviour
{
	[System.Serializable]
	public class UserInfoEvent : UnityEngine.Events.UnityEvent { }
	public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

	private static	UserInfoData data = new UserInfoData();
	public	static	UserInfoData Data => data;

	public void GetUserInfoFromBackend()
	{
		// ���� �α����� ����� ���� �ҷ�����
		// https://developer.thebackend.io/unity3d/guide/bmember/userInfo/
		Backend.BMember.GetUserInfo(callback =>
		{
			// ���� �ҷ����� ����
			if ( callback.IsSuccess() )
			{
				// JSON ������ �Ľ� ����
				try
				{
					JsonData json = callback.GetReturnValuetoJSON()["row"];

					data.gamerId				= json["gamerId"].ToString();
					data.countryCode			= json["countryCode"]?.ToString();
					data.nickname				= json["nickname"]?.ToString();
					data.inDate					= json["inDate"].ToString();
					data.emailForFindPassword	= json["emailForFindPassword"]?.ToString();
					data.subscriptionType		= json["subscriptionType"].ToString();
					data.federationId			= json["federationId"]?.ToString();
				}
				// JSON ������ �Ľ� ����
				catch ( System.Exception e )
				{
					// ���� ������ �⺻ ���·� ����
					data.Reset();
					// try-catch ���� ���
					Debug.LogError(e);
				}
			}
			// ���� �ҷ����� ����
			else
			{
				// ���� ������ �⺻ ���·� ����
				// Tip. �Ϲ������� �������� ���¸� ����� �⺻���� ������ �����صΰ� ���������� �� �ҷ��ͼ� ���
				data.Reset();
				Debug.LogError(callback.GetMessage());
			}

			// ���� ���� �ҷ����⿡ �������� �� onUserInfoEvent�� ��ϵǾ� �ִ� �̺�Ʈ �޼ҵ� ȣ��
			onUserInfoEvent?.Invoke();
		});
	}
}

public class UserInfoData
{
	public	string	gamerId;				// ������ gamerID
	public	string	countryCode;			// �����ڵ�. ���� �������� null
	public	string	nickname;				// �г���. ���� �������� null
	public	string	inDate;					// ������ inDate
	public	string	emailForFindPassword;	// �̸����ּ�. ���� �������� null
	public	string	subscriptionType;		// Ŀ����, ������̼� Ÿ��
	public	string	federationId;			// ����, ����, ���̽��� ������̼� ID. Ŀ���� ������ null

	public void Reset()
	{
		gamerId					= "Offline";
		countryCode				= "Unknown";
		nickname				= "Noname";
		inDate					= string.Empty;
		emailForFindPassword	= string.Empty;
		subscriptionType		= string.Empty;
		federationId			= string.Empty;
	}
}

