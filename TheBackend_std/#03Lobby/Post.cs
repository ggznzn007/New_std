using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System;       // TimeSpan

public class Post : MonoBehaviour
{
	[SerializeField]
	private	Sprite[]			spriteItemIcons;		// ���� ���Ե� ������ �����ܿ� ����� �̹��� �迭
	[SerializeField]
	private	Image				imageItemIcon;			// ���� ���Ե� ������ ������ ���
	[SerializeField]
	private	TextMeshProUGUI		textItemCount;			// ���� ���Ե� �������� ����
	[SerializeField]
	private	TextMeshProUGUI		textTitle;				// ���� ����
	[SerializeField]
	private	TextMeshProUGUI		textContent;			// ���� ����
	[SerializeField]
	private	TextMeshProUGUI		textExpirationDate;		// ���� ������� ���� �ð� ���

	[SerializeField]
	private	Button				buttonReceive;			// ���� "����" ��ư ó��

	private	BackendPostSystem	backendPostSystem;
	private	PopupPostBox		popupPostBox;
	private	PostData			postData;

	public void Setup(BackendPostSystem postSystem, PopupPostBox postBox, PostData postData)
	{
		// ���� "����" ��ư�� ������ �� ó��
		buttonReceive.onClick.AddListener(OnClickPostReceive);

		backendPostSystem	= postSystem;
		popupPostBox		= postBox;
		this.postData		= postData;

		// ���� ����� ���� ����
		textTitle.text	 = postData.title;
		textContent.text = postData.content;

		// ù ��° ������ ������ ���� ���
		foreach ( string itemKey in postData.postReward.Keys )
		{
			// ���� ���Ե� ������ �̹��� ���
			if ( itemKey.Equals("heart") )		imageItemIcon.sprite = spriteItemIcons[0];
			else if ( itemKey.Equals("gold") )	imageItemIcon.sprite = spriteItemIcons[1];
			else if ( itemKey.Equals("jewel") )	imageItemIcon.sprite = spriteItemIcons[2];

			// ���� ���Ե� ������ ���� ���
			textItemCount.text = postData.postReward[itemKey].ToString();

			// �ϳ��� ���� ���Ե� �������� ���� �� �� ���� �ִµ� ���� ���������� ù ��° ������ ������ ���
			break;
		}

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
			
				// ���� ������� ���� �ð� = ���� ���� �ð� - ���� ���� �ð�
				TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);
			
				// timeSpan.TotalHours�� ���� �Ⱓ�� ��(hour)�� ǥ��
				textExpirationDate.text = $"{timeSpan.TotalHours:F0}�ð� �� ����";
			}
			// JSON ������ �Ľ� ����
			catch ( Exception e )
			{
				// try-catch ���� ���
				Debug.LogError(e);
			}
		});
	}

	private void OnClickPostReceive()
	{
		// ���� ���� UI ������Ʈ ����
		popupPostBox.DestroyPost(gameObject);
		// ���� ����
		backendPostSystem.PostReceive(PostType.Admin, postData.inDate);
	}
}

