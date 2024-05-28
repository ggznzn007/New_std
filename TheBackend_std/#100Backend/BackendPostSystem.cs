using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BackEnd;
//using System.Diagnostics;

public class BackendPostSystem : MonoBehaviour
{
    [System.Serializable]
    public class PostEvent : UnityEvent<List<PostData>> { }
    public PostEvent onGetPostListEvent = new PostEvent();

    private List<PostData> postList = new List<PostData>();

    public void PostListGet()
    {
        PostListGet(PostType.Admin);
    }

    public void PostReceive(PostType postType, string inDate)
    {
        PostReceive(postType, postList.FindIndex(item => item.inDate.Equals(inDate)));
    }

    public void PostReceiveAll()
    {
        PostReceiveAll(PostType.Admin);
    }

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ҷ����� �� ������ �߻��߽��ϴ� : {callback}");
                return;
            }

            Debug.Log($"���� ����Ʈ �ҷ����� ��û�� �����߽��ϴ� : {callback}");

            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("�������� ����ֽ��ϴ�.");
                    return;
                }

                // �Ź� ���� ����Ʈ�� �ҷ��� �� postList �ʱ�ȭ
                postList.Clear();

                // ���� ���� ������ ��� ���� ���� �ҷ�����
                for (int i = 0; i < jsonData.Count; ++i)
                {
                    PostData post = new PostData();

                    post.title = jsonData[i]["title"].ToString();
                    post.content = jsonData[i]["content"].ToString();
                    post.inDate = jsonData[i]["inDate"].ToString();
                    post.expirationDate = jsonData[i]["expirationDate"].ToString();

                    // ���� �Բ� �߼۵� ��� ������ ���� �ҷ�����
                    foreach (LitJson.JsonData itemJson in jsonData[i]["items"])
                    {
                        // ���� �Բ� �߼۵� �������� ��Ʈ �̸��� "��ȭ��Ʈ" �� ��
                        if (itemJson["chartName"].ToString() == Constants.GOODS_CHART_NAME)
                        {
                            string itemName = itemJson["item"]["itemName"].ToString();
                            int itemCount = int.Parse(itemJson["itemCount"].ToString());

                            // ���� ���Ե� �������� ���� �� �� ��
                            // �̹� postReward�� �ش� ������ ������ ������ ���� �߰�
                            if (post.postReward.ContainsKey(itemName))
                            {
                                post.postReward[itemName] += itemCount;
                            }
                            // postReward�� ���� ������ �����̸� ��� �߰�
                            else
                            {
                                post.postReward.Add(itemName, itemCount);
                            }

                            post.isCanReceive = true;
                        }
                        else
                        {
                            Debug.LogWarning($"���� �������� �ʴ� ��Ʈ �����Դϴ�. : {itemJson["chartName"].ToString()}");

                            post.isCanReceive = false;
                        }
                    }

                    postList.Add(post);
                }

                // ���� ����Ʈ �ҷ����Ⱑ �Ϸ�Ǿ��� �� �̺�Ʈ �޼ҵ� ȣ��
                onGetPostListEvent?.Invoke(postList);

                // ���� ������ ��� ����(postList) ���� ���
                for (int i = 0; i < postList.Count; ++i)
                {
                    Debug.Log($"{i}��° ����\n{postList[i].ToString()}");
                }
            }
            // JSON ������ �Ľ� ����
            catch (System.Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        });
    }

    public void PostReceive(PostType postType, int index)
    {
        if (postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        if (index >= postList.Count)
        {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�. : ��û index{index} / ���� �ִ� ���� : {postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ��������� ��û�մϴ�.");

        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}�� {postList[index].inDate} ������� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ������ɿ� �����߽��ϴ�. : {callback}");

            postList.RemoveAt(index);

            // ���� ������ �������� ���� ��
            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                // ������ ����
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]);
                // �÷��̾��� ��ȭ ������ ������ ������Ʈ
                BackendGameData.Instance.GameDataUpdate();
            }
            else
            {
                Debug.LogWarning("���� ������ ���� �������� �������� �ʽ��ϴ�.");
            }
        });
    }

    public void PostReceiveAll(PostType postType)
    {
        if (postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        Debug.Log($"{postType.ToString()} ���� ��ü ������ ��û�մϴ�.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()} ���� ��ü ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"���� ��ü ���ɿ� �����߽��ϴ�. : {callback}");

            postList.Clear();       // ��� ������ �����߱� ������ postList�� �ʱ�ȭ�Ѵ�

            // ��� ������ ������ ����
            foreach (LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
            {
                SavePostToLocal(postItemsJson);
            }

            // �÷��̾��� ��ȭ ������ ������ ������Ʈ
            BackendGameData.Instance.GameDataUpdate();
        });
    }

    public void SavePostToLocal(LitJson.JsonData item)
    {
        // JSON ������ �Ľ� ����
        try
        {
            foreach (LitJson.JsonData itemJson in item)
            {
                // ��Ʈ ���� �̸�(*.xlsx)�� Backend Console�� ����� ��Ʈ �̸�
                string chartFileName = itemJson["item"]["chartFileName"].ToString();
                string chartName = itemJson["chartName"].ToString();

                // GoodsChart.xlsx�� ����� ù��° �� �̸�
                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemName = itemJson["item"]["itemName"].ToString();
                string itemInfo = itemJson["item"]["itemInfo"].ToString();

                // ���� �߼��� �� �ۼ��ϴ� ������ ����
                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                // �������� ���� ��ȭ�� ���� �� �����Ϳ� ����
                if (chartName.Equals(Constants.GOODS_CHART_NAME))
                {
                    if (itemName.Equals("heart"))
                    {
                        BackendGameData.Instance.UserGameData.heart += itemCount;
                    }
                    else if (itemName.Equals("gold"))
                    {
                        BackendGameData.Instance.UserGameData.gold += itemCount;
                    }
                    else if (itemName.Equals("jewel"))
                    {
                        BackendGameData.Instance.UserGameData.jewel += itemCount;
                    }
                }

                Debug.Log($"{chartName} - {chartFileName}");
                Debug.Log($"[{itemId}] {itemName} : {itemInfo}, ȹ�� ���� : {itemCount}");
                Debug.Log($"�������� �����߽��ϴ�. : {itemName} - {itemCount}��");
            }
        }
        // JSON ������ �Ľ� ����
        catch (System.Exception e)
        {
            // try-catch ���� ���
            Debug.LogError(e);
        }
    }
}


/*
		*/
/**/