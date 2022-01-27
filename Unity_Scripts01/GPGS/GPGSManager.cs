using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


#if UNITY_ANDROID
public class GPGSManager : MonoBehaviour
{
    public Text myName;
    public Text myLog;
    public RawImage myImage;

    private bool bWaitingForAuth = false;

    private void Awake()
    {
        myLog.text = "�غ� ���Դϴ�...";

        // ���� ���Ӽ��� Ȱ��ȭ (�ʱ�ȭ)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        //���ӽ��۽� �ڵ��α���
        //doAutoLogin();
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
            //myLog.text = "Authenticating...";
            myLog.text = "�α��� ���Դϴ�...";
            bWaitingForAuth = true;
            //�α��� ���� ó������ (�ݹ��Լ�)      
            Social.localUser.Authenticate(AuthenticateCallback);

        }
        else
        {
            myLog.text = "�α����� �ʿ��մϴ�.\n";
        }
    }

    // �����α��� 
    public void OnBtnLoginClicked()
    {
        //�̹� ������ ����ڴ� �ٷ� �α��� �����ȴ�. 
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";
            StartCoroutine(UserPictureLoad());
            //Invoke("LoadSelectView", 1.5f);

        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    myLog.text = "name : " + "\n" + Social.localUser.userName + "\n";
                    StartCoroutine(UserPictureLoad());
                    //Invoke("LoadSelectView", 1.5f);
                }
                else
                {
                    Debug.Log("Login Fail");
                    myLog.text = "�α��� ����! �ٽ� �õ����ּ���.\n";
                }
            });
    }

    // ���� �α׾ƿ� 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        myLog.text = "�α����� �ʿ��մϴ�.";
    }

    // ���� callback
    void AuthenticateCallback(bool success)
    {
        myLog.text = "Loading";
        if (success)
        {
            // ����� �̸��� ����� 
            myLog.text = "Welcome" + "\n" + Social.localUser.userName + "\n";

            StartCoroutine(UserPictureLoad());
        }
        else
        {
            myLog.text = "�α����� �ʿ��մϴ�.\n";
        }
    }

    // ���� �̹��� �޾ƿ��� 
    IEnumerator UserPictureLoad()
    {
        myLog.text = "�α��� ���� �ε��� ...";
        // ���� ���� �̹��� ��������
        Texture2D pic = Social.localUser.image;

        // ���� �ƹ�Ÿ �̹��� ���θ� Ȯ�� �� �̹��� ���� 
        while (pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }
        myImage.texture = pic;
        myName.text = Social.localUser.userName;
        myLog.text = "�α��� �Ǿ����ϴ�!!!";
    }

    public void ClickEnter()
    {
        if (Social.localUser.authenticated)
        {
            
            LoadingSceneCtrl.LoadScene("Main");
        }
    }

    public void LoadSelectView()
    {

    }

    public void ClickBack()
    {
        LoadingSceneCtrl.LoadScene("Enter");
    }
}
#endif