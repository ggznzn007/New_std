using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public class BackendGuildSystem : MonoBehaviour
{
    [SerializeField]
    private FadeEffect_TMP textLog;
    [SerializeField]
    private GuildDefaultPage guildDefaultPage;
    [SerializeField]
    private GuildCreatePage guildCreatePage;
    [SerializeField]
    private GuildApplicantsPage guildApplicantsPage;
    [SerializeField]
    private GuildPage guildPage;

    public GuildData myGuildData { private set; get; } = new GuildData();
    public GuildData otherGuildData { private set; get; } = new GuildData();

    public void CreateGuild(string guildName, int goodsCount = 1)
    {
        Backend.Guild.CreateGuildV3(guildName, goodsCount, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLogCreateGuild(callback);

                return;
            }

            Debug.Log($"��尡 �����Ǿ����ϴ�. : {callback}");

            // ��带 ������ �� "��� ���������Դϴ�."�� ������������ �⺻ ����
            SetNotice("��� ���������Դϴ�.");

            // ��� ������ �������� �� ȣ��
            guildCreatePage.SuccessCreateGuild();

            Backend.RandomInfo.SetRandomData(RandomType.Guild, Constants.RANDOM_GUILD_UUID, 0, callback =>
            {
                if (!callback.IsSuccess())
                {
                    ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "CreateGuild():SetRandomData");

                    return;
                }

                Debug.Log($"������ ��带 ��� ���� ��ȸ ��Ͽ� �߰��Ͽ����ϴ�. : {callback}");
            });
        });
    }

    public void ApplyGuild(string guildName)
    {
        // GetGuildIndateByGuildNameV3() �޼ҵ带 ȣ���� ���ϴ� ���(guildName)�� guildInDate ���� ��ȯ
        string guildInDate = GetGuildInfoBy(guildName);

        // guildInDate ������ ���� ��忡 ���� ��û�� ������.
        Backend.Guild.ApplyGuildV3(guildInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLogApplyGuild(callback);

                return;
            }

            Debug.Log($"��� ���� ��û�� �����߽��ϴ�. : {callback}");
        });
    }

    public void GetApplicants()
    {
        Backend.Guild.GetApplicantsV3(callback =>
        {
            if (!callback.IsSuccess())
            {
                // ���� ������ 403 �ϳ� �ۿ� ���� ������ ������ �޼ҵ� ���� X
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants");

                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("��� ���� ��û ����� ����ֽ��ϴ�.");
                    return;
                }

                // ��� ���� ��û ��Ͽ� �ִ� ��� UI ��Ȱ��ȭ
                guildApplicantsPage.DeactivateAll();

                List<TransactionValue> transactionList = new List<TransactionValue>();
                List<GuildMemberData> guildMemberDataList = new List<GuildMemberData>();

                foreach (LitJson.JsonData item in jsonData)
                {
                    GuildMemberData guildMember = new GuildMemberData();

                    guildMember.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    guildMember.inDate = item["inDate"].ToString();

                    guildMemberDataList.Add(guildMember);

                    // guildMember.inDate�� ������ ģ���� UserGameData ���� �ҷ�����
                    Where where = new Where();
                    where.Equal("owner_inDate", guildMember.inDate);
                    transactionList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
                }

                Backend.GameData.TransactionReadV2(transactionList, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetApplicants-TransactionReadV2");
                        return;
                    }

                    LitJson.JsonData userData = callback.GetFlattenJSON()["Responses"];

                    if (userData.Count <= 0)
                    {
                        Debug.LogWarning($"�����Ͱ� �������� �ʽ��ϴ�.");
                        return;
                    }

                    for (int i = 0; i < userData.Count; ++i)
                    {
                        guildMemberDataList[i].level = userData[i]["level"].ToString();
                        guildApplicantsPage.Activate(guildMemberDataList[i]);
                        Debug.Log(guildMemberDataList[i].ToString());
                    }
                });
            }
            // JSON ������ �Ľ� ����
            catch (Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        });
    }

    public void ApproveApplicant(string gamerInDate)
    {
        Backend.Guild.ApproveApplicantV3(gamerInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLogApproveApplicant(callback);

                return;
            }

            Debug.Log($"��� ���� ��û ������ �����߽��ϴ�. : {callback}");
        });
    }

    public void RejectApplicant(string gamerInDate)
    {
        Backend.Guild.RejectApplicantV3(gamerInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                // ������Ը� �ش� ��ư�� ���� ��������
                // ���� ������ 404 �ϳ� �ۿ� ���� ������ ������ �޼ҵ� ���� X
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "RejectApplicant");

                return;
            }

            Debug.Log($"��� ���� ��û ������ �����߽��ϴ�. : {callback}");
        });
    }

    public void GetMyGuildInfo()
    {
        Backend.Guild.GetMyGuildInfoV3(callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetMyGuildInfo");
                return;
            }

            try
            {
                LitJson.JsonData guildJson = callback.GetFlattenJSON()["guild"];

                if (guildJson.Count <= 0)
                {
                    Debug.LogWarning("�ҷ��� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                myGuildData.guildName = guildJson["guildName"].ToString();
                myGuildData.guildInDate = guildJson["inDate"].ToString();
                myGuildData.notice = guildJson["NOTICE"].ToString();
                myGuildData.memberCount = int.Parse(guildJson["memberCount"].ToString());

                myGuildData.master = new GuildMemberData();
                myGuildData.master.nickname = guildJson["masterNickname"].ToString();
                myGuildData.master.inDate = guildJson["masterInDate"].ToString();

                myGuildData.viceMasterList = new List<GuildMemberData>();

                LitJson.JsonData viceJson = guildJson["viceMasterList"];
                for (int i = 0; i < viceJson.Count; ++i)
                {
                    GuildMemberData vice = new GuildMemberData();
                    vice.nickname = viceJson[i]["nickname"].ToString();
                    vice.inDate = viceJson[i]["inDate"].ToString();

                    myGuildData.viceMasterList.Add(vice);
                }

                // �� ��� ���� �ҷ����� �Ϸ� �� ó��
                guildDefaultPage.SuccessMyGuildInfo();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void SetNotice(string notice)
    {
        Param param = new Param { { "NOTICE", notice } };

        Backend.Guild.ModifyGuildV3(param, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "SetNotice");

                return;
            }

            Debug.Log($"��� ��Ÿ ������[��������] ���濡 �����߽��ϴ�. : {callback}");
        });
    }

    public void GetGuildMemeberList(string guildInDate)
    {
        Backend.Guild.GetGuildMemberListV3(guildInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetGuildMemberList");
                return;
            }

            try
            {
                LitJson.JsonData memberJson = callback.GetFlattenJSON()["rows"];

                if (memberJson.Count <= 0)
                {
                    Debug.LogWarning("�ҷ��� ���� �����Ͱ� �����ϴ�.");
                    return;
                }

                guildPage.DeactivateAll();

                foreach (LitJson.JsonData member in memberJson)
                {
                    GuildMemberData guildMember = new GuildMemberData();

                    guildMember.position = member["position"].ToString();
                    guildMember.nickname = member["nickname"].ToString();
                    guildMember.goodsCount = int.Parse(member["totalGoods1Amount"].ToString());
                    guildMember.lastLogin = member["lastLogin"].ToString();

                    guildPage.Activate(guildMember);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void GetGuildInfo(string guildInDate)
    {
        Backend.Guild.GetGuildInfoV3(guildInDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetGuildInfo");
                return;
            }

            try
            {
                LitJson.JsonData guildJson = callback.GetFlattenJSON()["guild"];

                if (guildJson.Count <= 0)
                {
                    Debug.LogWarning("�ҷ��� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                otherGuildData.guildName = guildJson["guildName"].ToString();
                otherGuildData.guildInDate = guildJson["inDate"].ToString();
                otherGuildData.notice = guildJson["NOTICE"].ToString();
                otherGuildData.memberCount = int.Parse(guildJson["memberCount"].ToString());

                otherGuildData.master = new GuildMemberData();
                otherGuildData.master.nickname = guildJson["masterNickname"].ToString();
                otherGuildData.master.inDate = guildJson["masterInDate"].ToString();

                otherGuildData.viceMasterList = new List<GuildMemberData>();

                LitJson.JsonData viceJson = guildJson["viceMasterList"];
                for (int i = 0; i < viceJson.Count; ++i)
                {
                    GuildMemberData vice = new GuildMemberData();
                    vice.nickname = viceJson[i]["nickname"].ToString();
                    vice.inDate = viceJson[i]["inDate"].ToString();

                    otherGuildData.viceMasterList.Add(vice);
                }

                // ��� ���� �ҷ����� �Ϸ� �� ó��
                guildDefaultPage.SuccessGuildInfo();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void GetRandomGuildList()
    {
        Backend.RandomInfo.GetRandomData(RandomType.Guild, Constants.RANDOM_GUILD_UUID, 0, 0, 20, callback =>
        {
            if (!callback.IsSuccess())
            {
                ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetRandomGuildList");

                return;
            }

            try
            {
                LitJson.JsonData guildJson = callback.GetFlattenJSON()["rows"];

                if (guildJson.Count <= 0)
                {
                    Debug.LogWarning("�ҷ��� ���� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                guildDefaultPage.DeactivateAll();

                foreach (LitJson.JsonData guild in guildJson)
                {
                    Backend.Guild.GetGuildInfoV3(guild["guildInDate"].ToString(), callback =>
                    {
                        if (!callback.IsSuccess())
                        {
                            ErrorLog(callback.GetMessage(), "Guild_Failed_Log", "GetRandomGuildList():GetGuildInfoV3");
                            return;
                        }

                        LitJson.JsonData guildJson = callback.GetFlattenJSON()["guild"];

                        if (guildJson.Count <= 0)
                        {
                            Debug.LogWarning("�ҷ��� ��� �����Ͱ� �����ϴ�.");
                            return;
                        }

                        GuildData guildData = new GuildData();
                        guildData.guildName = guildJson["guildName"].ToString();
                        guildData.guildInDate = guildJson["inDate"].ToString();
                        guildData.master = new GuildMemberData();
                        guildData.master.nickname = guildJson["masterNickname"].ToString();
                        guildData.memberCount = int.Parse(guildJson["memberCount"].ToString());

                        guildDefaultPage.Activate(guildData);
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public string GetGuildInfoBy(string guildName)
    {
        // �ش� ����(guildName)�� ��尡 �����ϴ��� ���δ� ����� ����
        var bro = Backend.Guild.GetGuildIndateByGuildNameV3(guildName);
        string inDate = string.Empty;

        if (!bro.IsSuccess())
        {
            Debug.LogError($"��� �˻� ���� ������ �߻��߽��ϴ�. : {bro}");
            return inDate;
        }

        try
        {
            inDate = bro.GetFlattenJSON()["guildInDate"].ToString();

            Debug.Log($"{guildName}�� inDate ���� {inDate} �Դϴ�.");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return inDate;
    }

    private void ErrorLogCreateGuild(BackendReturnObject callback)
    {
        string message = string.Empty;

        switch (int.Parse(callback.GetStatusCode()))
        {
            case 403:   // Backend Console�� ������ ������ �������� ������ ��
                message = "��� ������ ���� ������ �����մϴ�.";
                break;
            case 409:   // �ߺ��� �������� ���� �õ��� ���
                message = "�̹� ������ �̸��� ��尡 �����մϴ�.";
                break;
            default:
                message = callback.GetMessage();
                break;
        }

        ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
    }

    private void ErrorLogApplyGuild(BackendReturnObject callback)
    {
        string message = string.Empty;

        switch (int.Parse(callback.GetStatusCode()))
        {
            case 403:   // Backend Console�� ������ ������ �������� ������ ��
                message = "��� ������ ���� ������ �����մϴ�.";
                break;
            case 409:
                message = "�̹� ���� ��û�� ����Դϴ�.";
                break;
            case 412:
                message = "�̹� �ٸ� ��忡 �ҼӵǾ� �ֽ��ϴ�.";
                break;
            case 429:
                message = "��忡 �� �̻� �ڸ��� �����ϴ�.";
                break;
        }

        ErrorLog(message, "Guild_Failed_Log", "ApplyGuild");
    }

    private void ErrorLogApproveApplicant(BackendReturnObject callback)
    {
        string message = string.Empty;

        switch (int.Parse(callback.GetStatusCode()))
        {
            case 412:
                message = "��� ���� ��û�� �����Ϸ��� ������ �̹� �ٸ� ��� �Ҽ��Դϴ�.";
                break;
            case 429:
                message = "��忡 �� �̻� �ڸ��� �����ϴ�.";
                break;
        }

        ErrorLog(message, "Guild_Failed_Log", "ApproveApplicant");
    }

    private void ErrorLog(string message, string behaviorType = "", string paramKey = "")
    {
        // ���� ������ Console View�� ���
        Debug.LogError($"{paramKey} : {message}");

        // ���� ������ UI�� ���
        textLog.FadeOut(message);

        // ���� ������ Backend Console�� ����
        Param param = new Param() { { paramKey, message } };
        // InsertLogV2(�ൿ ����, Key&Value)
        Backend.GameLog.InsertLogV2(behaviorType, param);
    }
}


// Backend.GameLog.InsertLogV2()�� 5.9.6 �̻� �������� ��� ����