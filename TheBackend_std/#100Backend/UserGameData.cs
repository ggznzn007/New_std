[System.Serializable]
public class UserGameData
{
	public	int		level;			// Lobby Scene�� ���̴� �÷��̾� ����
	public	float	experience;		// Lobby Scene�� ���̴� �÷��̾� ����ġ
	public	int		gold;			// ���� ��ȭ
	public	int		jewel;			// ���� ��ȭ
	public	int		heart;			// ���� �÷��̿� �Ҹ�Ǵ� ��ȭ
	
	public void Reset()
	{
		level		= 1;
		experience	= 0;
		gold		= 0;
		jewel		= 0;
		heart		= 30;
	}
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
*/