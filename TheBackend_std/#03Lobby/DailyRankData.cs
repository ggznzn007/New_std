using UnityEngine;
using TMPro;

public class DailyRankData : MonoBehaviour
{
	[SerializeField]
	private	TextMeshProUGUI	textRank;
	[SerializeField]
	private	TextMeshProUGUI	textNickName;
	[SerializeField]
	private	TextMeshProUGUI	textScore;

	private	int		rank;
	private	string	nickname;
	private	int		score;

	public	int		Rank
	{
		set
		{
			if ( value <= Constants.MAX_RANK_LIST )
			{
				rank			= value;
				textRank.text	= rank.ToString();
			}
			else
			{
				textRank.text	= "������ ����";
			}
		}
		get => rank;
	}

	public	string	Nickname
	{
		set
		{
			nickname			= value;
			textNickName.text	= nickname;
		}
		get => nickname;
	}

	public	int		Score
	{
		set
		{
			score			= value;
			textScore.text	= score.ToString();
		}
		get => score;
	}
}

