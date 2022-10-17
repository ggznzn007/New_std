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

    public Team currentTeam;  // 현재 팀구분

    public Map currentMap;    // 현재 맵구분

    public bool isSelected;   // 팀을 선택했는지 여부

    public int startingNum;   // 처음 시작을 구분하기위한 수
}
