using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class DailyRankLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject rankDataPrefab;      // 랭킹 정보 출력을 위한 UI 프리팹 원본
    [SerializeField]
    private Scrollbar scrollbar;            // scrollBar의 value 설정 (활성화될 때 1위가 보이도록)
    [SerializeField]
    private Transform rankDataParent;       // ScrollView의 Content 오브젝트
    [SerializeField]
    private DailyRankData myRankData;           // 내 랭킹 정보를 출력하는 UI 게임오브젝트

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        // 1 ~ 20위 랭킹 출력을 위한 UI 오브젝트 생성
        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        // 1위 랭킹이 보이도록 scroll 값 설정
        scrollbar.value = 1;
        // 1 ~ 20위의 랭킹 정보 불러오기
        GetRankList();
        // 내 랭킹 정보 불러오기
        GetMyRank();
    }

    private void GetRankList()
    {
        // 1 ~ 20위 랭킹 정보 불러오기
        Backend.URank.User.GetRankList(Constants.DAILY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                // JSON 데이터 파싱 성공
                try
                {
                    Debug.Log($"랭킹 조회에 성공했습니다 : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    // 받아온 데이터의 개수가 0이면 데이터가 없는 것
                    if (rankDataJson.Count <= 0)
                    {
                        // 1 ~ 20위까지 데이터를 빈 데이터로 설정
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }

                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }

                    else
                    {
                        int rankerCount = rankDataJson.Count;

                        // 랭킹 정보를 불러와 출력할 수 있도록 설정
                        for (int i = 0; i < rankerCount; ++i)
                        {
                            rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

                            // 닉네임은 별도로 설정하지 않은 유저도 존재할 수 있기 때문에
                            // 닉네임이 존재하지 않는 유저는 닉네임 대신 gamerId를 출력
                            rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ?
                                                       rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                        }
                        // 만약 limitCount에 설정된 숫자보다 현재 랭킹에 등록된 숫자가 적으면 나머지는 빈 값으로 설정
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                // JSON 데이터 파싱 실패
                catch (System.Exception e)
                {
                    // try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            else
            {
                // 1 ~ 20위까지 데이터를 빈 데이터로 설정
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"랭킹 조회 중 오류가 발생했습니다 : {callback}");
            }
        });
    }

    private void GetMyRank()
    {
        // 내 랭킹 정보 불러오기
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            // 닉네임이 없으면 gamerId, 닉네임이 있으면 nickname 사용
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                // JSON 데이터 파싱 성공
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    // 받아온 데이터의 개수가 0이면 데이터가 없는 것
                    if (rankDataJson.Count <= 0)
                    {
                        // ["순위에 없음", "닉네임", 0]과 같이 출력
                        SetRankData(myRankData, 1000000000, nickname, 0);

                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        // 닉네임은 별도로 설정하지 않은 유저도 존재할 수 있기 때문에
                        // 닉네임이 존재하지 않는 유저는 닉네임 대신 gamerId를 출력
                        myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
                                              rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                    }
                }
                // 자신의 랭킹 정보 JSON 데이터 파싱에 실패했을 때
                catch (System.Exception e)
                {
                    // ["순위에 없음", "닉네임", 0]과 같이 출력
                    SetRankData(myRankData, 1000000000, nickname, 0);

                    // try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            else
            {
                // 자신의 랭킹 정보 데이터가 존재하지 않을 때
                if (callback.GetMessage().Contains("userRank"))
                {
                    // ["순위에 없음", "닉네임", 0]과 같이 출력
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }
        });
    }

    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}

