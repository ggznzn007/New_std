using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
   ADMIN,BLUE,RED
}

public enum Map
{
    TUTORIAL_T,
    TOY,
    TUTORIAL_W,
    WESTERN
}

public class DataManager : MonoBehaviour
{
    public static DataManager DM;

    private void Awake()
    {
        if(DM == null)DM=this;
        else if (DM !=null) return;
        DontDestroyOnLoad(gameObject);
    }

    public Team currentTeam;  // ���� ������

    public Map currentMap;    // ���� �ʱ���

    public bool isSelected;   // ���� �����ߴ��� ����

    public int startingNum;   // ó�� ������ �����ϱ����� ��
}
