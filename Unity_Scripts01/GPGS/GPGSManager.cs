using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
public class GPGSManager : MonoBehaviour
{
    public Text myName;
    public Text myLog;
    public RawImage myImage;

    private bool bWaitingForAuth = false;

    private void Awake()
    {
        myLog.text = "Ready...";

        // ���� ���Ӽ��� Ȱ��ȭ (�ʱ�ȭ)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        //���ӽ��۽� �ڵ��α���
       // doAutoLogin();
    }

    // �ڵ��α���
    public void doAutoLogin()
    {
        myLog.text = "...";
        if (bWaitingForAuth)
            return;

        //���� �α����� �Ǿ����� �ʴٸ� 
        if (!Social.localUser.authenticated)
        {
            myLog.text = "Authenticating...";
            bWaitingForAuth = true;
            //�α��� ���� ó������ (�ݹ��Լ�)
            Social.localUser.Authenticate(AuthenticateCallback);
        }
        else
        {
            myLog.text = "Login Fail\n";
        }
    }

    // �����α��� 
    public void OnBtnLoginClicked()
    {
        //�̹� ������ ����ڴ� �ٷ� �α��� �����ȴ�. 
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            myLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    myLog.text = "name : " + Social.localUser.userName + "\n";
                }
                else
                {
                    Debug.Log("Login Fail");
                    myLog.text = "Login Fail\n";
                }
            });
    }

    // ���� �α׾ƿ� 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        myLog.text = "LogOut...";
    }

    // ���� callback
    void AuthenticateCallback(bool success)
    {
        myLog.text = "Loading";
        if (success)
        {
            // ����� �̸��� ����� 
            myLog.text = "Welcome" + Social.localUser.userName + "\n";

            StartCoroutine(UserPictureLoad());
        }
        else
        {
            myLog.text = "Login Fail\n";
        }
    }

    // ���� �̹��� �޾ƿ��� 
    IEnumerator UserPictureLoad()
    {
        myLog.text = "image Loading ...";
        // ���� ���� �̹��� ��������
        Texture2D pic = Social.localUser.image;

        // ���� �ƹ�Ÿ �̹��� ���θ� Ȯ�� �� �̹��� ���� 
        while (pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }
        myImage.texture = pic;
        myLog.text = "image Create";
    }
}
#endif