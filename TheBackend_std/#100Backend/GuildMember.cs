using UnityEngine;
using TMPro;
using BackEnd;
using System;

public class GuildMember : MonoBehaviour
{
	[SerializeField]
	private	TextMeshProUGUI		textPosition;
	[SerializeField]
	private	TextMeshProUGUI		textNickname;
	[SerializeField]
	private	TextMeshProUGUI		textGoodsCount;
	[SerializeField]
	private	TextMeshProUGUI		textLastLogin;

	public void Setup(GuildMemberData memberData)
	{
		SetPosition(memberData.position);
		SetDate(memberData.lastLogin);

		textNickname.text	= memberData.nickname;
		textGoodsCount.text	= memberData.goodsCount.ToString();
	}

	private void SetPosition(string position)
	{
		if ( position.Equals("master") )			position = "��帶����";
		else if ( position.Equals("viceMaster") )	position = "�ӿ�";
		else if ( position.Equals("member") )		position = "����";

		textPosition.text = position;
	}

	private void SetDate(string lastLogin)
	{
		// GetServerTime() - ���� �ð� �ҷ�����
		Backend.Utils.GetServerTime(callback =>
		{
			if ( !callback.IsSuccess() )
			{
				Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {callback}");
				return;
			}

			// JSON ������ �Ľ� ����
			try
			{
				// ���� ���� �ð�
				string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
				// ���� �ð� - ������ ���� �ð�
				TimeSpan timeSpan = DateTime.Parse(serverTime) - DateTime.Parse(lastLogin);

				// timeSpan.TotalHours�� ���� �Ⱓ�� ��(hour)�� ǥ��
				if ( timeSpan.TotalHours < 24 )	textLastLogin.text = $"{timeSpan.TotalHours:F0}�ð� ��";
				else							textLastLogin.text = $"{timeSpan.TotalDays:F0}�� ��";
			}
			// JSON ������ �Ľ� ����
			catch ( Exception e )
			{
				// try-catch ���� ���
				Debug.LogError(e);
			}
		});
	}
}


/*
	public void OnClickApproveApplicant()
	{
		// �ش� ������ UI ������Ʈ ����
		guildApplicantsPage.Deactivate(gameObject);
		// �ش� ���� ��� ���� ��û ����
		backendGuildSystem.ApproveApplicant(guildMemberData.inDate);
	}

	public void OnClickRejectApplicant()
	{
		// �ش� ������ UI ������Ʈ ����
		guildApplicantsPage.Deactivate(gameObject);
		// �ش� ���� ��� ���� ��û ����
		backendGuildSystem.RejectApplicant(guildMemberData.inDate);
	}
}

*/