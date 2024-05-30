using UnityEngine;
using System;
using BackEnd;
using System.Collections.Generic;

public class BackendFriendSystem : MonoBehaviour
{
    [SerializeField]
    private FriendSentRequestPage sentRequestPage;
    [SerializeField]
    private FriendReceivedRequestPage receivedRequestPage;
    [SerializeField]
    private FriendPage friendPage;

    private string GetUserInfoBy(string nickname)
    {
        // �ش� �г���(nickname)�� ������ �����ϴ��� ���δ� ����� ����
        var bro = Backend.Social.GetUserInfoByNickName(nickname);
        string inDate = string.Empty;

        if (!bro.IsSuccess())
        {
            Debug.LogError($"���� �˻� ���� ������ �߻��߽��ϴ�. : {bro}");
            return inDate;
        }

        // JSON ������ �Ľ� ����
        try
        {
            LitJson.JsonData jsonData = bro.GetFlattenJSON()["row"];

            // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
            if (jsonData.Count <= 0)
            {
                Debug.LogWarning("������ inDate �����Ͱ� �����ϴ�.");
                return inDate;
            }

            inDate = jsonData["inDate"].ToString();

            Debug.Log($"{nickname}�� inDate ���� {inDate} �Դϴ�.");
        }
        // JSON ������ �Ľ� ����
        catch (Exception e)
        {
            // try-catch ���� ���
            Debug.LogError(e);
        }

        return inDate;
    }

    public void SendRequestFriend(string nickname)
    {
        // RequestFriend() �޼ҵ带 �̿��� ģ�� �߰� ��û�� �� �� �ش� ģ���� inDate ������ �ʿ�
        string inDate = GetUserInfoBy(nickname);

        // inDate ������ ���� �������� "ģ�� ��û"�� ������.
        Backend.Friend.RequestFriend(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{nickname} ģ�� ��û ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"ģ�� ��û�� �����߽��ϴ�. : {callback}");

            // ģ�� ��û�� �����ϸ� ģ�� ��û ��� ��� �ҷ�����
            GetSentRequestList();
        });
    }

    public void GetSentRequestList()
    {
        Backend.Friend.GetSentRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ��û ��� ��� ��ȸ ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("ģ�� ��û ��� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                // ģ�� ��û ��� ��Ͽ� �ִ� ��� UI ��Ȱ��ȭ
                sentRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friend.nickname		= item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    // [ģ�� ��û]�� ���� �ð����κ��� ���� �Ⱓ�� �����ٸ� �ڵ����� ģ�� ��û ���
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RevokeSentRequest(friendData.inDate);
                        continue;
                    }

                    // ���� friend ������ �������� ģ�� ��û ��� UI Ȱ��ȭ
                    sentRequestPage.Activate(friendData);
                }
            }
            // JSON ������ �Ľ� ����
            catch (Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        });
    }

    public void RevokeSentRequest(string inDate)
    {
        Backend.Friend.RevokeSentRequest(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ��û ��� ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"ģ�� ��û ��ҿ� �����߽��ϴ�. : {callback}");
        });
    }

    public void GetReceivedRequestList()
    {
        Backend.Friend.GetReceivedRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ���� ��� ��� ��ȸ ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("ģ�� ���� ��� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                // ģ�� ���� ��� ��Ͽ� �ִ� ��� UI ��Ȱ��ȭ
                receivedRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    // ģ�� ��û�� ������ 3���� �����ٸ� �ڵ����� ģ�� ����
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RejectFriend(friendData);
                        continue;
                    }

                    // ���� friendData ������ �������� ģ�� ���� ��� UI Ȱ��ȭ
                    receivedRequestPage.Activate(friendData);
                }
            }
            // JSON ������ �Ľ� ����
            catch (Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        });
    }

    public void AcceptFriend(FriendData friendData)
    {
        Backend.Friend.AcceptFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}��(��) ģ���� �Ǿ����ϴ�. : {callback}");
        });
    }

    public void RejectFriend(FriendData friendData)
    {
        Backend.Friend.RejectFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}�� ģ�� ��û�� �����߽��ϴ�. : {callback}");
        });
    }

    public void GetFriendList()
    {
        Backend.Friend.GetFriendList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ��� ��ȸ ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("ģ�� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                // ģ�� ��Ͽ� �ִ� ��� UI ��Ȱ��ȭ
                friendPage.DeactivateAll();

                List<TransactionValue> transactionList = new List<TransactionValue>();
                List<FriendData> friendDataList = new List<FriendData>();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();
                    friendData.lastLogin = item["lastLogin"].ToString();

                    friendDataList.Add(friendData);

                    // friendData.inDate�� ������ ģ���� UserGameData ���� �ҷ�����
                    Where where = new Where();
                    where.Equal("owner_inDate", friendData.inDate);
                    transactionList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
                }

                Backend.GameData.TransactionReadV2(transactionList, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError($"Transaction Error : {callback}");
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
                        // ģ�� ���� ���� ����
                        friendDataList[i].level = $"Lv. {userData[i]["level"]}";
                        // ���� friendDataList[i] ������ �������� ģ�� UI Ȱ��ȭ
                        friendPage.Activate(friendDataList[i]);
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

    public void BreakFriend(FriendData friendData)
    {
        Backend.Friend.BreakFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}��(��) ģ���� �����Ǿ����ϴ�. : {callback}");
        });
    }

    private bool IsExpirationDate(string createdAt)
    {
        // GetServerTime() - ���� �ð� �ҷ�����
        var bro = Backend.Utils.GetServerTime();

        if (!bro.IsSuccess())
        {
            Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {bro}");
            return false;
        }

        // JSON ������ �Ľ� ����
        try
        {
            // createdAt �ð����κ��� 3�� ���� �ð�
            DateTime after3Days = DateTime.Parse(createdAt).AddDays(Constants.EXPIRATION_DAYS);
            // ���� ���� �ð�
            string serverTime = bro.GetFlattenJSON()["utcTime"].ToString();
            // ������� ���� �ð� = ���� �ð� - ���� ���� �ð�
            TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

            if (timeSpan.TotalHours < 0)
            {
                return true;
            }
        }
        // JSON ������ �Ľ� ����
        catch (Exception e)
        {
            // try-catch ���� ���
            Debug.LogError(e);
        }

        return false;
    }
}

